using UnityEngine;
using UnityEngine.UI;

public class VelocityController : MonoBehaviour
{
    public Image imgTacometro;
    public CarController carController;

    // La velocidad m�xima que el tac�metro debe mostrar (ej. 300 km/h).
    public float maxTachometerSpeed = 300f;

    // Esta variable ser� privada y se calcular� autom�ticamente.
    private float maxSpeedForNeedle_kmh;

    // Rango de rotaci�n de la aguja (ajusta estos valores en el Inspector).
    public float minAngle = 180f; // �ngulo de la aguja para 0 km/h
    public float maxAngle = -180f; // �ngulo de la aguja para la velocidad m�xima del tac�metro

    void Start()
    {
        // 1. Revisa que la referencia al CarController no sea nula.
        if (carController == null)
        {
            Debug.LogError("El CarController no est� asignado. La aguja no funcionar�.");
            return;
        }

        // 2. Convierte el valor de maxSpeed del CarController (m/s) a km/h.
        // Esto es la velocidad m�xima del coche, que ser� tambi�n el tope de la aguja.
        maxSpeedForNeedle_kmh = carController.maxSpeed * 3.6f;

        Debug.Log("La velocidad m�xima para la aguja es: " + maxSpeedForNeedle_kmh + " km/h");
    }

    void Update()
    {
        if (carController == null || imgTacometro == null)
        {
            return;
        }

        // 1. Obtiene la velocidad real del coche en m/s.
        //float currentRealSpeed_ms = carController.theRB.linearVelocity.magnitude;
        float currentRealSpeed_ms = carController.theRB.linearVelocity.magnitude;

        // 2. Convierte la velocidad a km/h.
        float currentRealSpeed_kmh = currentRealSpeed_ms * 3.6f;

        // 3. Limita la velocidad que usaremos para la aguja usando la variable privada.
        float clampedSpeedForNeedle = Mathf.Clamp(currentRealSpeed_kmh, 0f, maxSpeedForNeedle_kmh);

        // 4. Mapea la velocidad limitada al rango de �ngulos de la aguja.
        // Usamos la velocidad m�xima del tac�metro para calcular el porcentaje.
        float mappedAngle = Mathf.Lerp(minAngle, maxAngle, clampedSpeedForNeedle / maxTachometerSpeed);

        // 5. Aplica la rotaci�n a la aguja.
        imgTacometro.rectTransform.localEulerAngles = new Vector3(0, 0, mappedAngle);
    }
}