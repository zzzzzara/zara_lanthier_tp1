using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConsigneDemmenagement : MonoBehaviour
{
    public enum Categorie { Meuble, Cuisine, Decoration, Couvert }

    [Header("Consigne")]
    public GameObject panneauConsigne;
    public TMP_Text texteConsigne;
    public Button boutonCommencer;

    [Header("Chrono")]
    public GameObject panneauChrono;
    public TMP_Text texteChrono;
    public float dureeChrono = 100f;

    [Header("Vérification")]
    public Button boutonVerifier;
    public string nomSceneReussite = "SceneReussite";
    public string nomSceneEchec = "SceneEchec";
    [Tooltip("Maintenir cette touche pour afficher le curseur et cliquer sur Vérifier")]
    public KeyCode toucheCurseur = KeyCode.LeftAlt;

    [Header("Affichage")]
    public float delaiEntreMots = 0.25f;
    public float delaiAvantBouton = 0.5f;

    // ===== État conservé entre les scènes =====
    private static bool consigneValidee = false;
    private static int choixIndex = -1;
    private static bool chronoActifSauvegarde = false;
    private static float tempsRestantSauvegarde = 0f;
    private static readonly List<Categorie> achats = new List<Categorie>();

    private float tempsRestant;
    private bool chronoDemarre = false;
    private bool consigneEnCours = true;

    private static readonly Categorie[][] combinaisons =
    {
        new[] { Categorie.Meuble,   Categorie.Cuisine,    Categorie.Decoration },
        new[] { Categorie.Meuble,   Categorie.Couvert,    Categorie.Cuisine    },
        new[] { Categorie.Cuisine,  Categorie.Decoration, Categorie.Couvert    }
    };

    // ===== API à appeler depuis tes magasins =====
    // Exemple : ConsigneDemmenagement.AjouterAchat(ConsigneDemmenagement.Categorie.Meuble);
    public static bool AjouterAchat(Categorie categorie)
    {
        if (achats.Count >= 3) return false;   // déjà 3 éléments choisis
        achats.Add(categorie);
        return true;
    }

    public static int NombreAchats()
    {
        return achats.Count;
    }

    // À appeler pour relancer une nouvelle partie (menu, etc.)
    public static void ReinitialiserConsigne()
    {
        consigneValidee = false;
        choixIndex = -1;
        chronoActifSauvegarde = false;
        tempsRestantSauvegarde = 0f;
        achats.Clear();
    }

    void Start()
    {
        if (texteConsigne == null || boutonCommencer == null)
        {
            Debug.LogError("Texte Consigne ou Bouton Commencer n'est pas assigné !");
            return;
        }
        if (panneauChrono == null)
            Debug.LogWarning("Panneau Chrono n'est pas assigné (le texte du chrono sera utilisé seul).");
        if (texteChrono == null)
            Debug.LogWarning("Texte Chrono n'est pas assigné !");
        if (boutonVerifier == null)
            Debug.LogWarning("Bouton Vérifier n'est pas assigné !");

        // Bouton Vérifier caché au départ
        if (boutonVerifier != null)
        {
            boutonVerifier.gameObject.SetActive(false);
            boutonVerifier.onClick.RemoveAllListeners();
            boutonVerifier.onClick.AddListener(Verifier);
        }

        // ===== Retour dans la scène après un magasin : on restaure l'état =====
        if (consigneValidee)
        {
            consigneEnCours = false;

            boutonCommencer.gameObject.SetActive(false);
            texteConsigne.gameObject.SetActive(false);
            if (panneauConsigne != null) panneauConsigne.SetActive(false);

            SetChronoVisible(true);
            tempsRestant = tempsRestantSauvegarde;
            chronoDemarre = chronoActifSauvegarde;
            AfficherChrono();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        // ===== Première fois : affichage normal de la consigne =====
        SetChronoVisible(false);
        if (panneauConsigne != null) panneauConsigne.SetActive(true);
        texteConsigne.gameObject.SetActive(true);
        texteConsigne.text = "";
        boutonCommencer.gameObject.SetActive(false);

        boutonCommencer.onClick.RemoveAllListeners();
        boutonCommencer.onClick.AddListener(Commencer);

        LibererSouris();

        if (choixIndex < 0)
            choixIndex = Random.Range(0, combinaisons.Length);

        string consigne =
            "DÉMÉNAGEMENT\n\n" +
            "Achète la combinaison suivante :\n\n" +
            "• " + TexteCombinaison(choixIndex) + "\n\n" +
            "Attention à ton budget !";

        StartCoroutine(AfficherConsigne(consigne));
    }

    string TexteCombinaison(int index)
    {
        string[] noms = new string[3];
        for (int i = 0; i < 3; i++)
            noms[i] = NomCategorie(combinaisons[index][i]);

        string texte = string.Join(" + ", noms);
        return char.ToUpper(texte[0]) + texte.Substring(1);
    }

    string NomCategorie(Categorie c)
    {
        switch (c)
        {
            case Categorie.Meuble: return "un meuble";
            case Categorie.Cuisine: return "cuisine";
            case Categorie.Decoration: return "décoration";
            case Categorie.Couvert: return "couvert";
            default: return "";
        }
    }

    void SetChronoVisible(bool visible)
    {
        if (panneauChrono != null) panneauChrono.SetActive(visible);
        if (texteChrono != null) texteChrono.gameObject.SetActive(visible);
    }

    IEnumerator AfficherConsigne(string consigne)
    {
        string[] mots = consigne.Split(' ');
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < mots.Length; i++)
        {
            sb.Append(mots[i]);
            if (i < mots.Length - 1) sb.Append(' ');

            texteConsigne.text = sb.ToString();
            yield return new WaitForSecondsRealtime(delaiEntreMots);
        }

        yield return new WaitForSecondsRealtime(delaiAvantBouton);
        boutonCommencer.gameObject.SetActive(true);
    }

    void LibererSouris()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void VerrouillerSouris()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Commencer()
    {
        consigneEnCours = false;
        consigneValidee = true;
        StopAllCoroutines();

        boutonCommencer.gameObject.SetActive(false);
        texteConsigne.gameObject.SetActive(false);
        if (panneauConsigne != null) panneauConsigne.SetActive(false);

        SetChronoVisible(true);
        tempsRestant = dureeChrono;
        chronoDemarre = true;
        SauvegarderChrono();
        AfficherChrono();

        VerrouillerSouris();
    }

    void Update()
    {
        if (consigneEnCours)
        {
            LibererSouris();
            return;
        }

        // Le bouton Vérifier apparaît dès que 3 éléments sont choisis
        if (boutonVerifier != null)
        {
            bool visible = chronoDemarre && achats.Count >= 3;
            if (boutonVerifier.gameObject.activeSelf != visible)
                boutonVerifier.gameObject.SetActive(visible);

            // Maintenir la touche pour afficher le curseur et cliquer sur le bouton
            if (visible)
            {
                if (Input.GetKeyDown(toucheCurseur)) LibererSouris();
                if (Input.GetKeyUp(toucheCurseur)) VerrouillerSouris();
            }
        }

        if (!chronoDemarre) return;

        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0)
        {
            tempsRestant = 0;
            chronoDemarre = false;
            AfficherChrono();
            TerminerScene(false);   // temps écoulé = échec
            return;
        }

        SauvegarderChrono();
        AfficherChrono();
    }

    // ===== Vérification =====
    public void Verifier()
    {
        if (!chronoDemarre) return;
        TerminerScene(AchatsCorrespondent());
    }

    bool AchatsCorrespondent()
    {
        if (achats.Count != 3 || choixIndex < 0) return false;

        List<Categorie> cible = new List<Categorie>(combinaisons[choixIndex]);
        foreach (Categorie a in achats)
        {
            if (!cible.Remove(a)) return false;   // élément qui n'est pas dans la consigne
        }
        return cible.Count == 0;
    }

    void TerminerScene(bool reussite)
    {
        chronoDemarre = false;
        LibererSouris();
        ReinitialiserConsigne();   // la prochaine partie repartira de zéro
        SceneManager.LoadScene(reussite ? nomSceneReussite : nomSceneEchec);
    }

    void SauvegarderChrono()
    {
        tempsRestantSauvegarde = tempsRestant;
        chronoActifSauvegarde = chronoDemarre;
    }

    void AfficherChrono()
    {
        if (texteChrono == null) return;

        int minutes = Mathf.FloorToInt(tempsRestant / 60);
        int secondes = Mathf.FloorToInt(tempsRestant % 60);
        texteChrono.text = minutes.ToString("00") + ":" + secondes.ToString("00");
    }
}