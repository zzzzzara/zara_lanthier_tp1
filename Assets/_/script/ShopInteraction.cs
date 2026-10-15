
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopInteraction : MonoBehaviour
{
    [SerializeField] private GameObject shopButton;
    [SerializeField] private string sceneName;

    private bool playerNearby = false;

    void Start()
    {
        shopButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (ShoppingState.shoppingStarted &&
                !ShoppingState.shopAlreadyChosen)
            {
                shopButton.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            shopButton.SetActive(false);
        }
    }

    public void ClickShopButton()
    {
        if (!playerNearby)
            return;

        if (!ShoppingState.shoppingStarted)
            return;

        if (ShoppingState.shopAlreadyChosen)
            return;

        ShoppingState.shopAlreadyChosen = true;

        SceneManager.LoadScene(sceneName);
    }
}

