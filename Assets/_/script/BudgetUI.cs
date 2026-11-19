using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BudgetUI : MonoBehaviour
{
    public TMP_Text texteBudget;
    public Slider barreBudget;

    [Header("Animation")]
    public int budgetMax = 300;
    public float vitesseAnimation = 3f;

    private float valeurAffichee = 1f;

    void Update()
    {
        AfficherBudget();
    }

    public void AfficherBudget()
    {
        if (texteBudget != null)
            texteBudget.text = "Budget : " + ShoppingState.budgetRestant + " $";

        if (barreBudget != null && budgetMax > 0)
        {
            float cible = (float)ShoppingState.budgetRestant / budgetMax;

            // Descend en douceur vers la valeur cible
            valeurAffichee = Mathf.MoveTowards(valeurAffichee, cible, vitesseAnimation * Time.unscaledDeltaTime * 0.3f);
            barreBudget.value = valeurAffichee;
        }
    }
}