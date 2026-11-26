using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DialogueAleatoire : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text texteDialogue;

    [TextArea(2, 4)]
    public string[] dialogues = new string[3]
    {
        "L'aventure en forêt approche ! Prépare ton équipement avant la fin du chrono pour l'activité de camping!",
        "Une journée à la plage t'attend ! Prépare-toi avant que le chrono arrive à zéro!",
        "Direction les montagnes enneigées ! Tu dois te préparer avant la fin du chrono pour aller au ski!"
    };

    [Header("Vitesse du texte")]
    public float delaiEntreMots = 0.4f;

    [Header("Boutons")]
    public Button boutonDemarrer;
    public Button boutonVerifier; // apparaît quand les 3 morceaux sont choisis

    [Header("Chronomètre")]
    public TMP_Text texteChrono;
    public float dureeChrono = 60f;

    [Header("Résultat")]
    public TMP_Text texteResultat; // optionnel

    [Header("Scènes")]
    public string nomSceneEchec = "SceneEchec";
    public string nomSceneProchainNiveau = "Niveau2";

    [Header("Souris")]
    [Tooltip("Si activé, ce script gère la souris : libre quand un bouton est affiché, verrouillée pendant le jeu.")]
    public bool forcerSourisLibre = true; // nom conservé pour ne pas perdre la valeur dans l'Inspector

    // ---------- ÉTAT PERSISTANT ENTRE LES SCÈNES ----------
    private static bool chronoDejaDemarre = false;
    private static float tempsFinChrono = 0f;
    private static int indexDestination = -1;

    // Même ordre que le tableau "dialogues"
    private static readonly Categorie[] categoriesDialogues =
    {
        Categorie.Camping, // dialogue 0
        Categorie.Plage,   // dialogue 1
        Categorie.Ski      // dialogue 2
    };

    public static Categorie Destination
    {
        get { return categoriesDialogues[Mathf.Max(0, indexDestination)]; }
    }

    public static bool ChronoFini
    {
        get { return chronoDejaDemarre && Time.time >= tempsFinChrono; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ReinitialiserEtat()
    {
        chronoDejaDemarre = false;
        tempsFinChrono = 0f;
        indexDestination = -1;
    }
    // ------------------------------------------------------

    private float tempsRestant;
    private bool chronoEnCours = false;
    private bool chronoTermine = false;

    void Start()
    {
        tempsRestant = dureeChrono;

        if (boutonDemarrer != null)
        {
            boutonDemarrer.gameObject.SetActive(false);
            boutonDemarrer.onClick.RemoveListener(DemarrerChrono);
            boutonDemarrer.onClick.AddListener(DemarrerChrono);
        }

        if (boutonVerifier != null)
        {
            boutonVerifier.gameObject.SetActive(false);
            boutonVerifier.onClick.RemoveListener(VerifierEtTerminer);
            boutonVerifier.onClick.AddListener(VerifierEtTerminer);
        }

        if (texteChrono != null)
            texteChrono.text = "";

        if (texteResultat != null)
            texteResultat.text = "";

        // --- Retour sur la scène : pas de dialogue ni de bouton démarrer, le chrono continue ---
        if (chronoDejaDemarre)
        {
            if (texteDialogue != null)
                texteDialogue.text = "";

            tempsRestant = tempsFinChrono - Time.time;

            if (tempsRestant <= 0f)
            {
                tempsRestant = 0f;
                chronoTermine = true;

                if (texteChrono != null)
                    texteChrono.text = "Temps écoulé !";

                TerminerNiveau();
                return;
            }
            else
            {
                chronoEnCours = true;
                ActualiserChrono();
                ActualiserBoutonVerifier();
            }

            ActualiserSouris();
            return;
        }

        // --- Première visite : comportement normal ---
        LibererSouris();

        if (texteDialogue == null)
        {
            Debug.LogError("Le texte du dialogue n'est pas assigné !");
            return;
        }

        if (dialogues == null || dialogues.Length != 3)
        {
            Debug.LogError("Il faut exactement 3 dialogues !");
            return;
        }

        if (indexDestination < 0)
        {
            indexDestination = Random.Range(0, dialogues.Length);
        }

        StartCoroutine(AfficherDialogue(dialogues[indexDestination]));
    }

    IEnumerator AfficherDialogue(string dialogue)
    {
        texteDialogue.text = "";

        string[] mots = dialogue.Split(' ');

        foreach (string mot in mots)
        {
            texteDialogue.text +=
                (texteDialogue.text.Length > 0 ? " " : "") + mot;

            yield return new WaitForSeconds(delaiEntreMots);
        }

        LibererSouris();

        if (boutonDemarrer != null)
            boutonDemarrer.gameObject.SetActive(true);
    }

    public void DemarrerChrono()
    {
        if (chronoEnCours || chronoTermine)
            return;

        Debug.Log("Bouton cliqué : démarrage du chrono !");

        if (texteDialogue != null)
            texteDialogue.text = "";

        if (boutonDemarrer != null)
            boutonDemarrer.gameObject.SetActive(false);

        chronoDejaDemarre = true;
        tempsFinChrono = Time.time + dureeChrono;

        chronoEnCours = true;
        ShoppingState.StartShopping();
        ActualiserChrono();

        // Le bouton est caché : la souris se verrouille pour jouer
        ActualiserSouris();
    }

    void Update()
    {
        if (forcerSourisLibre)
            ActualiserSouris();

        if (!chronoEnCours)
            return;

        ActualiserBoutonVerifier();

        tempsRestant = tempsFinChrono - Time.time;

        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            chronoEnCours = false;
            chronoTermine = true;

            if (texteChrono != null)
                texteChrono.text = "Temps écoulé !";

            Debug.Log("Le chrono est terminé !");
            TerminerNiveau();
            return;
        }

        ActualiserChrono();
    }

    void ActualiserChrono()
    {
        if (texteChrono != null)
        {
            texteChrono.text =
                "Temps restant : " +
                Mathf.CeilToInt(tempsRestant) + " s";
        }
    }

    // Affiche le bouton seulement quand les 3 morceaux sont choisis
    void ActualiserBoutonVerifier()
    {
        if (boutonVerifier == null)
            return;

        bool troisChoisis = ChoixJoueur.choix.Count >= 3;

        if (boutonVerifier.gameObject.activeSelf != troisChoisis)
            boutonVerifier.gameObject.SetActive(troisChoisis);
    }

    // Clic sur "Vérifier" -> réussite = niveau suivant, sinon échec
    public void VerifierEtTerminer()
    {
        if (chronoTermine)
            return;

        chronoEnCours = false;
        chronoTermine = true;
        TerminerNiveau();
    }

    bool TenueCorrecte()
    {
        if (ChoixJoueur.choix.Count < 3)
            return false;

        foreach (Categorie c in ChoixJoueur.choix)
        {
            if (c != Destination)
                return false;
        }
        return true;
    }

    // Vérification finale : échec ou niveau suivant
    void TerminerNiveau()
    {
        bool reussi = TenueCorrecte();

        // ---- LOG DE DIAGNOSTIC ----
        string contenu = "";
        foreach (Categorie c in ChoixJoueur.choix)
            contenu += c + " ";
        Debug.Log("Destination : " + Destination +
                  " | Choix (" + ChoixJoueur.choix.Count + ") : " + contenu);
        // ---------------------------

        Debug.Log(reussi
            ? "Tenue correcte : niveau suivant"
            : "Tenue incorrecte ou incomplète pour l'activité " + Destination + " : échec");

        // Remise à zéro pour le prochain niveau ou le prochain essai
        ReinitialiserEtat();
        ChoixJoueur.Reinitialiser();
        MorceauAffiche.Vider();

        SceneManager.LoadScene(reussi ? nomSceneProchainNiveau : nomSceneEchec);
    }

    // ---------- GESTION DE LA SOURIS ----------
    void LibererSouris()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void VerrouillerSouris()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Souris libre si le chrono ne tourne pas ou si un bouton est affiché,
    // verrouillée le reste du temps pour pouvoir jouer
    void ActualiserSouris()
    {
        bool boutonDemarrerVisible = boutonDemarrer != null && boutonDemarrer.gameObject.activeSelf;
        bool boutonVerifierVisible = boutonVerifier != null && boutonVerifier.gameObject.activeSelf;

        bool besoinSouris = !chronoEnCours || boutonDemarrerVisible || boutonVerifierVisible;

        if (besoinSouris)
            LibererSouris();
        else
            VerrouillerSouris();
    }
    // ------------------------------------------
}