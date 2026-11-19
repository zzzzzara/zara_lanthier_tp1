using UnityEngine;

public class DemarrageDemmenagement : MonoBehaviour
{
    void Awake()
    {
        if (!ShoppingState.shoppingStarted)
        {
            ShoppingState.InitialiserBudget(300);
            ShoppingState.StartShopping();
        }

        Debug.Log("Budget actuel : " + ShoppingState.budgetRestant);
    }
}