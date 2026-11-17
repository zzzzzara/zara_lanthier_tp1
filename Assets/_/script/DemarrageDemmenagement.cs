
using UnityEngine;

public class DemarrageDemmenagement : MonoBehaviour
{
    void Start()
    {
        ShoppingState.InitialiserBudget(300);
        ShoppingState.StartShopping();
    }
}
