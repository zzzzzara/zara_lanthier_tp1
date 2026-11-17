
using UnityEngine;
using TMPro;

public class VerificationMission : MonoBehaviour
{
    public TMP_Text texteResultat;

    public void Verifier()
    {
        bool meuble = ShoppingState.categoriesAchetees.Contains("Meuble");
        bool cuisine = ShoppingState.categoriesAchetees.Contains("Cuisine");
        bool deco = ShoppingState.categoriesAchetees.Contains("Deco");
        bool couvert = ShoppingState.categoriesAchetees.Contains("Couvert");

        bool combinaison1 = meuble && cuisine && deco;
        bool combinaison2 = meuble && cuisine && couvert;
        bool combinaison3 = cuisine && deco && couvert;

        if (combinaison1 || combinaison2 || combinaison3)
        {
            texteResultat.text = "MISSION RÉUSSIE !";
        }
        else
        {
            texteResultat.text = "Il te manque encore des catégories !";
        }
    }
}
