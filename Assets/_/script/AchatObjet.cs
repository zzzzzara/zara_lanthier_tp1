
using UnityEngine;
using UnityEngine.UI;

public class AchatObjet : MonoBehaviour
{
    public int prix;
    public string categorie;
    public BudgetUI budgetUI;
    public Button boutonAcheter;

    public void AcheterObjet()
    {
        if (budgetUI == null || boutonAcheter == null)
        {
            Debug.LogError("Références manquantes !");
            return;
        }

        if (ShoppingState.Acheter(prix))
        {
            ShoppingState.AjouterCategorie(categorie);

            budgetUI.AfficherBudget();
            boutonAcheter.interactable = false;

            Debug.Log("Objet acheté !");
        }
        else
        {
            Debug.Log("Pas assez d'argent !");
        }
    }
}
