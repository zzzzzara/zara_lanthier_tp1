
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

    private float tempsRestant;
    private bool chronoEnCours = false;
    private bool chronoTermine = false;

    void Start()
    {
        tempsRestant = dureeChrono;

        // Cacher le bouton au début
        if (boutonDemarrer != null)
        {
            boutonDemarrer.gameObject.SetActive(false);

            // Relier le clic au démarrage du chrono
            boutonDemarrer.onClick.RemoveListener(DemarrerChrono);
            boutonDemarrer.onClick.AddListener(DemarrerChrono);
        }

        // Cacher le chrono avant le démarrage
        if (texteChrono != null)
        {
            texteChrono.text = "";
        }

        // Vérifier le texte
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

        // Choisir un dialogue au hasard
        int choix = Random.Range(0, dialogues.Length);

        // Afficher le dialogue progressivement
        StartCoroutine(AfficherDialogue(dialogues[choix]));
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

        // Le dialogue est complètement affiché :
        // faire apparaître le bouton
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

        // Afficher et libérer la souris
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Effacer le dialogue
        if (texteDialogue != null)
        {
            texteDialogue.text = "";
        }

        // Faire disparaître le bouton
        if (boutonDemarrer != null)
        {
            boutonDemarrer.gameObject.SetActive(false);
        }

        // Démarrer le chrono
        chronoEnCours = true;
        ActualiserChrono();
    }

    void Update()
    {
        if (!chronoEnCours)
            return;

        tempsRestant -= Time.deltaTime;

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
}