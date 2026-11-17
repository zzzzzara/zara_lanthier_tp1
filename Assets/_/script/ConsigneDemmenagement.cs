
using UnityEngine;
using TMPro;

public class ConsigneDemmenagement : MonoBehaviour
{
    public GameObject panneauConsigne;
    public TMP_Text texteConsigne;
    public GameObject panneauChrono;
    public TMP_Text texteChrono;

    public float dureeChrono = 100f;

    private float tempsRestant;
    private bool chronoDemarre = false;

    void Start()
    {
        panneauConsigne.SetActive(true);
        panneauChrono.SetActive(false);

        texteConsigne.text =
            "DÉMÉNAGEMENT\n\n" +
            "Achète une des combinaisons suivantes :\n\n" +
            "• Un meuble + cuisine + décoration\n\n" +
            "OU\n\n" +
            "• Un meuble + couvert + cuisine\n\n" +
            "OU\n\n" +
            "• Cuisine + décoration + couvert\n\n" +
            "Attention à ton budget !";
    }

    public void Commencer()
    {
        panneauConsigne.SetActive(false);
        panneauChrono.SetActive(true);

        tempsRestant = dureeChrono;
        chronoDemarre = true;
    }

    void Update()
    {
        if (!chronoDemarre)
            return;

        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0)
        {
            tempsRestant = 0;
            chronoDemarre = false;
        }

        int minutes = Mathf.FloorToInt(tempsRestant / 60);
        int secondes = Mathf.FloorToInt(tempsRestant % 60);

        texteChrono.text = minutes.ToString("00") + ":" +
                           secondes.ToString("00");
    }
}
