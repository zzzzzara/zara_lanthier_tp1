using UnityEngine;
using UnityEngine.UI;

public class MorceauAffiche : MonoBehaviour
{
    // Les nouveaux types sont à la FIN pour ne pas changer les valeurs déjà réglées dans l'Inspector
    public enum TypeMorceau { Chandail, Pantalon, Accessoire, Cadeau, Dessert }

    private const int NOMBRE_TYPES = 5;

    [Header("Boutique (scène de sélection)")]
    public SelectCharacter selecteur;   // l'objet qui a SelectCharacter
    public TypeMorceau type;            // type de morceau vendu dans cette boutique
    public Sprite[] icones;             // une image 2D par option, dans le même ordre que Characters

    [Header("Ville niveau 1 (images du canvas)")]
    public Image imageChandail;
    public Image imagePantalon;
    public Image imageAccessoire;

    [Header("Ville niveau 2 (images du canvas)")]
    public Image imageCadeau;
    public Image imageDessert;

    static Sprite[] choisis = new Sprite[NOMBRE_TYPES];

    // Remet tout à zéro au lancement du jeu et à chaque changement de niveau
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Vider()
    {
        choisis = new Sprite[NOMBRE_TYPES];
    }

    void Start()
    {
        Afficher(imageChandail, TypeMorceau.Chandail);
        Afficher(imagePantalon, TypeMorceau.Pantalon);
        Afficher(imageAccessoire, TypeMorceau.Accessoire);
        Afficher(imageCadeau, TypeMorceau.Cadeau);
        Afficher(imageDessert, TypeMorceau.Dessert);
    }

    // Une image est visible seulement si un morceau a été choisi
    void Afficher(Image image, TypeMorceau t)
    {
        if (image == null)
            return;

        Sprite s = choisis[(int)t];

        image.sprite = s;
        image.gameObject.SetActive(s != null);
    }

    // À brancher sur le bouton "Sélectionner" de la boutique
    public void Choisir()
    {
        if (selecteur == null)
        {
            Debug.LogWarning("MorceauAffiche : le champ 'Selecteur' n'est pas assigné.", this);
            return;
        }

        if (icones == null || icones.Length == 0)
        {
            Debug.LogWarning("MorceauAffiche : la liste 'Icones' est vide.", this);
            return;
        }

        int n = selecteur.Number;

        if (n < 0 || n >= icones.Length)
        {
            Debug.LogWarning("MorceauAffiche : l'option " + n + " n'a pas d'icône (" + icones.Length + " icônes).", this);
            return;
        }

        choisis[(int)type] = icones[n];
    }
}