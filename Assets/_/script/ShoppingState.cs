
public static class ShoppingState
{
    public static bool shoppingStarted = false;
    public static bool shopAlreadyChosen = false;

    public static int budgetRestant = 0;

    public static void StartShopping()
    {
        shoppingStarted = true;
        shopAlreadyChosen = false;
    }

    public static void InitialiserBudget(int montant)
    {
        budgetRestant = montant;
    }

    public static bool Acheter(int prix)
    {
        if (prix <= 0)
        {
            return false;
        }

        if (budgetRestant >= prix)
        {
            budgetRestant -= prix;
            return true;
        }

        return false;
    }

    public static void Reinitialiser()
    {
        shoppingStarted = false;
        shopAlreadyChosen = false;
        budgetRestant = 0;
    }
}
