using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsigneDemmenagement : MonoBehaviour
{
    [Header("Consigne")]
    public GameObject panneauConsigne;
    public TMP_Text texteConsigne;
    public Button boutonCommencer;

    [Header("Chrono")]
    public GameObject panneauChrono;
    public TMP_Text texteChrono;
    public float dureeChrono = 100f;

    [Header("Affichage")]
    public float delaiEntreMots = 0.25f;
    public float delaiAvantBouton = 0.5f;

    private float tempsRestant;
    private bool chronoDemarre = false;
    private bool consigneEnCours = true;

    private readonly string[] combinaisons =
    {
        "Un meuble + cuisine + décoration",
        "Un meuble + couvert + cuisine",
        "Cuisine + décoration + couvert"
    };

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

        // État de départ
        SetChronoVisible(false);
        if (panneauConsigne != null) panneauConsigne.SetActive(true);
        texteConsigne.gameObject.SetActive(true);
        texteConsigne.text = "";
        boutonCommencer.gameObject.SetActive(false);

        boutonCommencer.onClick.RemoveAllListeners();
        boutonCommencer.onClick.AddListener(Commencer);

        LibererSouris();

        string choix = combinaisons[Random.Range(0, combinaisons.Length)];

        string consigne =
            "DÉMÉNAGEMENT\n\n" +
            "Achète la combinaison suivante :\n\n" +
            "• " + choix + "\n\n" +
            "Attention à ton budget !";

        StartCoroutine(AfficherConsigne(consigne));
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

    public void Commencer()
    {
        consigneEnCours = false;
        StopAllCoroutines();

        boutonCommencer.gameObject.SetActive(false);
        texteConsigne.gameObject.SetActive(false);
        if (panneauConsigne != null) panneauConsigne.SetActive(false);

        SetChronoVisible(true);
        tempsRestant = dureeChrono;
        chronoDemarre = true;
        AfficherChrono();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (consigneEnCours)
            LibererSouris();

        if (!chronoDemarre) return;

        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0)
        {
            tempsRestant = 0;
            chronoDemarre = false;
        }

        AfficherChrono();
    }

    void AfficherChrono()
    {
        if (texteChrono == null) return;

        int minutes = Mathf.FloorToInt(tempsRestant / 60);
        int secondes = Mathf.FloorToInt(tempsRestant % 60);
        texteChrono.text = minutes.ToString("00") + ":" + secondes.ToString("00");
    }
}