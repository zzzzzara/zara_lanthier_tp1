using UnityEngine;

public static class ChoixJoueur
{
    public static Categorie categorieChoisie;
    public static bool aFaitUnChoix = false;

    // Remet l'état à zéro à chaque lancement du jeu
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Reinitialiser()
    {
        categorieChoisie = Categorie.Plage;
        aFaitUnChoix = false;
    }
}