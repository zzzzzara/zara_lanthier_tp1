using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueAleatoire : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text texteDialogue;

    [TextArea(2, 4)]
    public string[] dialogues = new string[3]
    {
        "L'aventure en forêt approche ! Prépare ton équipement avant la fin du chrono et n'oublie pas d'aller magasiner à la boutique de camping pour acheter tout ce qu'il te faut !",
        "Une journée à la plage t'attend ! Prépare-toi avant que le chrono arrive à zéro et passe à la boutique de plage pour acheter tout le nécessaire !",
        "Direction les montagnes enneigées ! Tu dois te préparer avant la fin du chrono. Va magasiner à la boutique de ski pour être prêt à dévaler les pistes !"
    };

    [Header("Vitesse du texte")]
    public float delaiEntreMots = 0.4f;

    [Header("Bouton")]
    public Button boutonDemarrer;

    [Header("Chronomètre")]
    public TMP_Text texteChrono;
    public float dureeChrono = 60f;

    [Header("Résultat")]
    public TMP_Text texteResultat; // optionnel : affiche le verdict

    [Header("Souris")]
    [Tooltip("Garde la souris libre tant que le script est actif (utile si un autre script la re-verrouille).")]
    public bool forcerSourisLibre = true;

    // ---------- ÉTAT PERSISTANT ENTRE LES SCÈNES ----------
    private static bool chronoDejaDemarre = false; // le joueur a déjà cliqué sur le bouton
    private static float tempsFinChrono = 0f;      // moment (Time.time) où le chrono se termine
    private static int indexDestination = -1;      // -1 = pas encore tirée

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

    // Réinitialise l'état à chaque lancement du jeu (utile dans l'éditeur
    // si le "Domain Reload" est désactivé)
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

        // Cacher le bouton au début
        if (boutonDemarrer != null)
        {
            boutonDemarrer.gameObject.SetActive(false);
            boutonDemarrer.onClick.RemoveListener(DemarrerChrono);
            boutonDemarrer.onClick.AddListener(DemarrerChrono);
        }

        if (texteChrono != null)
        {
            texteChrono.text = "";
        }

        if (texteResultat != null)
        {
            texteResultat.text = "";
        }

        // --- Retour sur la scène : pas de dialogue ni de bouton, le chrono continue ---
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

                VerifierChoix();
            }
            else
            {
                chronoEnCours = true;
                ActualiserChrono();

                // Si le joueur revient de la sélection, on vérifie son choix
                if (ChoixJoueur.aFaitUnChoix)
                {
                    VerifierChoix();
                }
            }

            return;
        }

        // --- Première visite : comportement normal ---
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

        // On ne tire qu'une seule fois : la destination reste la même au retour sur la scène
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
        {
            boutonDemarrer.gameObject.SetActive(true);
        }
    }

    public void DemarrerChrono()
    {
        if (chronoEnCours || chronoTermine)
            return;

        Debug.Log("Bouton cliqué : démarrage du chrono !");

        LibererSouris();

        if (texteDialogue != null)
        {
            texteDialogue.text = "";
        }

        if (boutonDemarrer != null)
        {
            boutonDemarrer.gameObject.SetActive(false);
        }

        // Mémoriser le démarrage pour les prochaines visites de la scène
        chronoDejaDemarre = true;
        tempsFinChrono = Time.time + dureeChrono;

        chronoEnCours = true;
        ShoppingState.StartShopping();
        ActualiserChrono();
    }

    void Update()
    {
        if (forcerSourisLibre)
        {
            LibererSouris();
        }

        if (!chronoEnCours)
            return;

        // Calculé à partir de l'heure de fin : reste exact même après un changement de scène
        tempsRestant = tempsFinChrono - Time.time;

        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            chronoEnCours = false;
            chronoTermine = true;

            if (texteChrono != null)
            {
                texteChrono.text = "Temps écoulé !";
            }

            Debug.Log("Le chrono est terminé !");
            VerifierChoix();
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

    void VerifierChoix()
    {
        string message;

        if (!ChoixJoueur.aFaitUnChoix)
        {
            message = "Tu n'as rien choisi !";
        }
        else if (ChoixJoueur.categorieChoisie == Destination)
        {
            message = "Bravo ! Tu as choisi la bonne tenue !";
        }
        else
        {
            message = "Oups, ce n'est pas la bonne tenue pour " + Destination + " !";
        }

        Debug.Log(message);

        if (texteResultat != null)
        {
            texteResultat.text = message;
        }
    }

    void LibererSouris()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}