using UnityEngine;
using UnityEngine.UI;

public class MorceauAffiche : MonoBehaviour
{
    public enum TypeMorceau { Chandail, Pantalon, Accessoire }

    [Header("Boutique (scène de sélection)")]
    public SelectCharacter selecteur;   // l'objet qui a SelectCharacter
    public TypeMorceau type;            // type de morceau vendu dans cette boutique
    public Sprite[] icones;             // une image 2D par option, dans le même ordre que Characters

    [Header("Ville (images du canvas)")]
    public Image imageChandail;
    public Image imagePantalon;
    public Image imageAccessoire;

    static Sprite[] choisis = new Sprite[3];

    // Remet tout à zéro au lancement du jeu (peut aussi être appelée pour recommencer)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Vider()
    {
        choisis = new Sprite[3];
    }

    void Start()
    {
        Afficher(imageChandail, 0);
        Afficher(imagePantalon, 1);
        Afficher(imageAccessoire, 2);
    }

    // Une image est visible seulement si un morceau a été choisi
    void Afficher(Image image, int index)
    {
        if (image == null)
            return;

        image.sprite = choisis[index];
        image.gameObject.SetActive(choisis[index] != null);
    }

    // À brancher sur le bouton "Sélectionner" de la boutique
    public void Choisir()
    {
        if (selecteur == null || icones == null)
            return;

        int n = selecteur.Number;

        if (n < 0 || n >= icones.Length)
            return;

        choisis[(int)type] = icones[n];
    }
}