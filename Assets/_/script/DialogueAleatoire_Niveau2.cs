using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DialogueAleatoire_Niveau2 : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text texteDialogue;

    [TextArea(2, 4)]
    public string[] dialogues = new string[3]
    {
        "C'est la fête d'Élodie! Elle aime les tartes et les aventures en forêt. Trouve-lui un cadeau et un dessert...",
        "C'est la fête d'Élodie! Elle aime les muffins et les étoiles. Trouve-lui un cadeau et un dessert...",
        "C'est la fête d'Élodie! Elle aime les beignes et les plantes. Trouve-lui un cadeau et un dessert..."
    };

    [Header("Vitesse du texte")]
    public float delaiEntreMots = 0.4f;

    [Header("Boutons")]
    public Button boutonDemarrer;
    public Button boutonVerifier; // apparaît quand les 2 objets sont choisis

    [Header("Chronomètre")]
    public TMP_Text texteChrono;
    public float dureeChrono = 60f;

    [Header("Résultat")]
    public TMP_Text texteResultat; // optionnel

    [Header("Scènes")]
    public string nomSceneEchec = "SceneEchec";
    public string nomSceneProchainNiveau = "Niveau3";

    [Header("Souris")]
    [Tooltip("Garde la souris libre tant que le script est actif (utile si un autre script la re-verrouille).")]
    public bool forcerSourisLibre = true;

    // Nombre d'objets à choisir (cadeau + dessert)
    private const int NOMBRE_OBJETS = 2;

    // ---------- ÉTAT PERSISTANT ENTRE LES SCÈNES ----------
    private static bool chronoDejaDemarre = false;
    private static float tempsFinChrono = 0f;
    private static int indexDestination = -1;

    // Pour chaque dialogue, les 2 objets requis (même ordre que "dialogues")
    private static readonly Categorie[][] objetsRequis =
    {
        new Categorie[] { Categorie.Tarte,  Categorie.Foret  }, // dialogue 0
        new Categorie[] { Categorie.Muffin, Categorie.Etoile }, // dialogue 1
        new Categorie[] { Categorie.Beigne, Categorie.Plante }  // dialogue 2
    };

    public static Categorie[] ObjetsRequis
    {
        get { return objetsRequis[Mathf.Max(0, indexDestination)]; }
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

        LibererSouris();

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

        // --- Retour sur la scène : le chrono continue ---
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
            }
            else
            {
                chronoEnCours = true;
                ActualiserChrono();
                ActualiserBoutonVerifier();
            }

            return;
        }

        // --- Première visite ---
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

        LibererSouris();

        if (texteDialogue != null)
            texteDialogue.text = "";

        if (boutonDemarrer != null)
            boutonDemarrer.gameObject.SetActive(false);

        chronoDejaDemarre = true;
        tempsFinChrono = Time.time + dureeChrono;

        chronoEnCours = true;
        ShoppingState.StartShopping();
        ActualiserChrono();
    }

    void Update()
    {
        if (forcerSourisLibre)
            LibererSouris();

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

    // Le bouton apparaît dès que 2 objets sont choisis
    void ActualiserBoutonVerifier()
    {
        if (boutonVerifier == null)
            return;

        bool objetsChoisis = ChoixJoueur.choix.Count >= NOMBRE_OBJETS;

        if (boutonVerifier.gameObject.activeSelf != objetsChoisis)
            boutonVerifier.gameObject.SetActive(objetsChoisis);
    }

    public void VerifierEtTerminer()
    {
        if (chronoTermine)
            return;

        chronoEnCours = false;
        chronoTermine = true;
        TerminerNiveau();
    }

    // Il faut exactement les 2 objets requis (ex. Tarte + Foret)
    bool TenueCorrecte()
    {
        if (ChoixJoueur.choix.Count != NOMBRE_OBJETS)
            return false;

        foreach (Categorie requis in ObjetsRequis)
        {
            if (!ChoixJoueur.choix.Contains(requis))
                return false;
        }
        return true;
    }

    void TerminerNiveau()
    {
        bool reussi = TenueCorrecte();

        Debug.Log(reussi
            ? "Bons objets choisis : niveau suivant"
            : "Objets incorrects ou incomplets : échec");

        ReinitialiserEtat();
        ChoixJoueur.Reinitialiser();

        SceneManager.LoadScene(reussi ? nomSceneProchainNiveau : nomSceneEchec);
    }

    void LibererSouris()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}