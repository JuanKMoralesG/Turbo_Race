using UnityEngine;
using UnityEngine.UI;

public class VelocityController : MonoBehaviour
{
    public Image imgTacometro;
    public CarController carController;

    // La velocidad máxima que el tacómetro debe mostrar (ej. 300 km/h).
    public float maxTachometerSpeed = 300f;

    // Esta variable será privada y se calculará automáticamente.
    private float maxSpeedForNeedle_kmh;

    // Rango de rotación de la aguja (ajusta estos valores en el Inspector).
    public float minAngle = 180f; // Ángulo de la aguja para 0 km/h
    public float maxAngle = -180f; // Ángulo de la aguja para la velocidad máxima del tacómetro

    void Start()
    {
        // 1. Revisa que la referencia al CarController no sea nula.
        if (carController == null)
        {
            Debug.LogError("El CarController no está asignado. La aguja no funcionará.");
            return;
        }

        // 2. Convierte el valor de maxSpeed del CarController (m/s) a km/h.
        // Esto es la velocidad máxima del coche, que será también el tope de la aguja.
        maxSpeedForNeedle_kmh = carController.maxSpeed * 3.6f;

        Debug.Log("La velocidad máxima para la aguja es: " + maxSpeedForNeedle_kmh + " km/h");
    }

    void Update()
    {
        if (carController == null || imgTacometro == null)
        {
            return;
        }

        // 1. Obtiene la velocidad real del coche en m/s.
        float currentRealSpeed_ms = carController.theRB.linearVelocity.magnitude;

        // 2. Convierte la velocidad a km/h.
        float currentRealSpeed_kmh = currentRealSpeed_ms * 3.6f;

        // 3. Limita la velocidad que usaremos para la aguja usando la variable privada.
        float clampedSpeedForNeedle = Mathf.Clamp(currentRealSpeed_kmh, 0f, maxSpeedForNeedle_kmh);

        // 4. Mapea la velocidad limitada al rango de ángulos de la aguja.
        // Usamos la velocidad máxima del tacómetro para calcular el porcentaje.
        float mappedAngle = Mathf.Lerp(minAngle, maxAngle, clampedSpeedForNeedle / maxTachometerSpeed);

        // 5. Aplica la rotación a la aguja.
        imgTacometro.rectTransform.localEulerAngles = new Vector3(0, 0, mappedAngle);
    }
}