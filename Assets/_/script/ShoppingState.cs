
public static class ShoppingState
{
    public static bool shoppingStarted = false;
    public static bool shopAlreadyChosen = false;

    public static void StartShopping()
    {
        shoppingStarted = true;
        shopAlreadyChosen = false;
    }

    public static void Reinitialiser()
    {
        shoppingStarted = false;
        shopAlreadyChosen = false;
    }
}
