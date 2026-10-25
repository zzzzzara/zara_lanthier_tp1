using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoutonSelectionner : MonoBehaviour
{
    [Header("Références")]
    public SelectCharacter selecteur;   //script SelectCharacter
    public Button bouton;               // bouton "Sélectionner"

    [Header("Scène")]
    public string nomSceneRetour = "ScenePrincipale"; // nom exact de la scène 

    void Start()
    {
        if (bouton != null)
        {
            bouton.onClick.RemoveListener(Selectionner);
            bouton.onClick.AddListener(Selectionner);
        }
    }

    void Update()
    {
        // Chrono fini pendant qu'on est dans la boutique : retour à la ville, qui gère la suite
        if (DialogueAleatoire.ChronoFini)
            SceneManager.LoadScene(nomSceneRetour);
    }

    public void Selectionner()
    {
        if (selecteur == null)
        {
            Debug.LogError("Le SelectCharacter n'est pas assigné !");
            return;
        }

        // Stocker la catégorie de l'option affichée
        ChoixJoueur.categorieChoisie = selecteur.CategorieActuelle;
        ChoixJoueur.aFaitUnChoix = true;

        if (ChoixJoueur.choix.Count < 3)
            ChoixJoueur.choix.Add(selecteur.CategorieActuelle);

        // Cette boutique ne pourra plus être utilisée
        ChoixJoueur.boutiquesVisitees.Add(SceneManager.GetActiveScene().name);

        Debug.Log("Choix enregistré : " + ChoixJoueur.categorieChoisie);

        // Retour à la scène principale
        SceneManager.LoadScene(nomSceneRetour);
    }
}