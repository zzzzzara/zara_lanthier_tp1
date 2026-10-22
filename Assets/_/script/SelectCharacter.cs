using UnityEngine;

public enum Categorie
{
    Plage,
    Camping,
    Ski
}

public class SelectCharacter : MonoBehaviour
{
    public GameObject[] Characters;

    [Tooltip("Une catégorie par option, dans le même ordre que Characters.")]
    public Categorie[] categories = new Categorie[6]
    {
        Categorie.Plage,   // option 0
        Categorie.Plage,   // option 1
        Categorie.Camping, // option 2
        Categorie.Camping, // option 3
        Categorie.Ski,     // option 4
        Categorie.Ski      // option 5
    };

    public int Number;

    [Header("Souris")]
    [Tooltip("Garde la souris libre dans cette scène (utile si un autre script la re-verrouille).")]
    public bool forcerSourisLibre = true;

    // Catégorie de l'option actuellement affichée
    public Categorie CategorieActuelle
    {
        get
        {
            if (categories == null || Number < 0 || Number >= categories.Length)
                return Categorie.Plage;
            return categories[Number];
        }
    }

    void Start()
    {
        LibererSouris();
    }

    // LateUpdate s'exécute après les Update() des autres scripts,
    // donc c'est nous qui avons le dernier mot sur l'état de la souris
    void LateUpdate()
    {
        if (forcerSourisLibre)
        {
            LibererSouris();
        }
    }

    void LibererSouris()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChangeCharacter(int Num)
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i].SetActive(false);
        }

        Number += Num;

        if (Number >= Characters.Length)
        {
            Number = 0;
        }

        if (Number < 0)
        {
            Number = Characters.Length - 1;
        }

        Characters[Number].SetActive(true);

        Debug.Log("Option " + Number + " : " + CategorieActuelle);
    }

    // Vérifie que les deux tableaux ont la même taille
    void OnValidate()
    {
        if (Characters != null && categories != null && categories.Length != Characters.Length)
        {
            System.Array.Resize(ref categories, Characters.Length);
        }
    }
}