using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeMagnitude = 0.1f; // Magnitud de la sacudida
    public float shakeDuration = 0.5f; // Duración de la sacudida
    private float shakeTime = 0f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            // Movimiento aleatorio en el eje X, Y
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            // Volver a la posición original
            transform.position = originalPosition;
        }
    }

    public void TriggerShake()
    {
        shakeTime = shakeDuration;
    }
}
