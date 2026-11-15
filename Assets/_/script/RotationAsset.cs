using UnityEngine;

public class RotationAsset : MonoBehaviour
{
    public float vitesse = 2f;           // vitesse du va-et-vient
    public float angleMax = 45f;         // angle max de chaque côté
    public Vector3 axe = Vector3.up;     // axe de rotation (Y = gauche/droite)

    private Quaternion rotationDepart;

    void Start()
    {
        rotationDepart = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * vitesse) * angleMax;
        transform.localRotation = rotationDepart * Quaternion.AngleAxis(angle, axe);
    }
}