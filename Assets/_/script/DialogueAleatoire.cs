
using UnityEngine;
using TMPro;

public class DialogueAleatoire : MonoBehaviour
{
    [Header("Texte du dialogue")]
    public TMP_Text texteDialogue;

    [Header("Les 3 dialogues possibles")]
    [TextArea(2, 4)]
    public string[] dialogues = new string[3]
    {
        "L'aventure en forêt approche ! Prépare ton équipement avant la fin du chrono et n'oublie pas d'aller magasiner à la boutique de camping pour acheter tout ce qu'il te faut !",
        "Une journée à la plage t'attend ! Prépare-toi avant que le chrono arrive à zéro et passe à la boutique de plage pour acheter tout le nécessaire !",
        "Direction les montagnes enneigées ! Tu dois te préparer avant la fin du chrono. Va magasiner à la boutique de ski pour être prêt à dévaler les pistes !"
    };

    void Start()
    {
        // Vérifie que les 3 dialogues sont présents
        if (dialogues == null || dialogues.Length != 3)
        {
            Debug.LogWarning("Il faut exactement 3 dialogues !");
            return;
        }

        // Choisit un dialogue au hasard parmi les trois
        int choix = Random.Range(0, 3);

        // Affiche le dialogue choisi
        if (texteDialogue != null)
        {
            texteDialogue.text = dialogues[choix];
        }
        else
        {
            Debug.LogWarning("Le texte du dialogue n'est pas assigné !");
        }
    }
}