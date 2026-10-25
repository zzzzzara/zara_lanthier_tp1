using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopInteraction : MonoBehaviour
{
    [Header("Boutique")]
    [SerializeField] private GameObject shopButton;
    [SerializeField] private string sceneName;

    private bool playerNearby = false;

    void Start()
    {
        if (shopButton != null)
        {
            shopButton.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "Le bouton de boutique n'est pas assigné dans l'Inspector !",
                this
            );
        }
    }

    void Update()
    {
        if (shopButton == null)
            return;

        bool shouldShowButton =
            playerNearby &&
            ShoppingState.shoppingStarted &&
            !ChoixJoueur.boutiquesVisitees.Contains(sceneName);

        if (shopButton.activeSelf != shouldShowButton)
        {
            shopButton.SetActive(shouldShowButton);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    public void ClickShopButton()
    {
        if (!playerNearby)
            return;

        if (!ShoppingState.shoppingStarted)
            return;

        if (ChoixJoueur.boutiquesVisitees.Contains(sceneName))
            return;

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Le nom de la scène n'est pas renseigné !");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}