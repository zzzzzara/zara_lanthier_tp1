using UnityEngine;
using TMPro;

public class BudgetUI : MonoBehaviour
{
    public TMP_Text texteBudget;

    void Start()
    {
        AfficherBudget();
    }

    public void AfficherBudget()
    {
        texteBudget.text = "Budget : " + ShoppingState.budgetRestant + " $";
    }
}