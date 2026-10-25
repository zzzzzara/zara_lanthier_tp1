using UnityEngine;
using System.Collections.Generic;

public static class ChoixJoueur
{
    public static Categorie categorieChoisie;
    public static bool aFaitUnChoix = false;
    public static List<Categorie> choix = new List<Categorie>(); // les 3 morceaux choisis
    public static HashSet<string> boutiquesVisitees = new HashSet<string>(); // boutiques où un morceau a été choisi

    // Remet l'état à zéro à chaque lancement du jeu
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Reinitialiser()
    {
        categorieChoisie = Categorie.Plage;
        aFaitUnChoix = false;
        choix = new List<Categorie>();
        boutiquesVisitees = new HashSet<string>();
    }
}