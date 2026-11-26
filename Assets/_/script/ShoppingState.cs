
using UnityEngine;
using System.Collections.Generic;
using System;

public static class ShoppingState
{
    public static bool shoppingStarted = false;
    public static bool shopAlreadyChosen = false;

    public static int budgetRestant = 0;

    public static List<string> categoriesAchetees = new List<string>();

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
            Console.WriteLine("Achat effectué. Budget restant : " + budgetRestant);
            return true;
        }

        return false;
    }

    public static void AjouterCategorie(string categorie)
    {
        if (!categoriesAchetees.Contains(categorie))
        {
            categoriesAchetees.Add(categorie);
        }
    }

    public static void Reinitialiser()
    {
        shoppingStarted = false;
        shopAlreadyChosen = false;
        budgetRestant = 0;
        categoriesAchetees.Clear();
    }
}
