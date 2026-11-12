using UnityEngine;

public class ImageProximiteMagasin : MonoBehaviour
{
    [Header("Réglages")]
    public Transform joueur;
    public GameObject imageAAfficher;
    public float distance = 5f;

    void Start()
    {
        if (joueur == null)
        {
            GameObject j = GameObject.FindGameObjectWithTag("Player");
            if (j != null)
                joueur = j.transform;
            else
                Debug.LogWarning("Aucun joueur trouvé ! Assigne-le ou mets le tag Player.");
        }

        if (imageAAfficher != null)
            imageAAfficher.SetActive(false);
    }

    void Update()
    {
        if (joueur == null || imageAAfficher == null)
            return;

        float d = Vector3.Distance(transform.position, joueur.position);
        bool proche = d <= distance;

        if (imageAAfficher.activeSelf != proche)
            imageAAfficher.SetActive(proche);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}