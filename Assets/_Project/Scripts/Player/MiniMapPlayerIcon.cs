using UnityEngine;
using UnityEngine.UI;

public class MinimapPlayerIcon : MonoBehaviour
{
    // Referencias a los objetos de la escena.
    public Transform player;
    public RectTransform minimapRect;
    public Transform boundingBoxTransform;

    // Variables para invertir el mapeo de los ejes.
    // Marcar estas casillas en el Inspector para corregir el movimiento invertido.
    public bool invertXAxis = false;
    public bool invertYAxis = false;

    // Los l�mites del mundo, calculados autom�ticamente.
    private Vector2 minWorldPosition;
    private Vector2 maxWorldPosition;

    void Start()
    {
        // Verifica que todas las referencias est�n asignadas.
        if (player == null || minimapRect == null || boundingBoxTransform == null)
        {
            Debug.LogError("Las referencias del minimapa no est�n asignadas. Revisa el Inspector.");
            return;
        }

        // Obtiene los l�mites del objeto delimitador (plano, cubo, etc.).
        Bounds bounds = GetBoundsFromTransform(boundingBoxTransform);

        // Asigna los valores del Bounding Box a las variables de l�mites.
        minWorldPosition = new Vector2(bounds.min.x, bounds.min.z);
        maxWorldPosition = new Vector2(bounds.max.x, bounds.max.z);

        Debug.Log("L�mites del mundo detectados: Min=" + minWorldPosition + ", Max=" + maxWorldPosition);
    }

    void Update()
    {
        // Salir si las referencias no est�n asignadas.
        if (player == null || minimapRect == null || boundingBoxTransform == null)
        {
            return;
        }

        Vector3 playerPos = player.position;

        // Calcula el porcentaje normalizado (0 a 1) de la posici�n del jugador.
        // Se invierte si la casilla correspondiente est� marcada en el Inspector.
        float normalizedX = invertXAxis
            ? Mathf.InverseLerp(maxWorldPosition.x, minWorldPosition.x, playerPos.x)
            : Mathf.InverseLerp(minWorldPosition.x, maxWorldPosition.x, playerPos.x);

        float normalizedY = invertYAxis
            ? Mathf.InverseLerp(maxWorldPosition.y, minWorldPosition.y, playerPos.z)
            : Mathf.InverseLerp(minWorldPosition.y, maxWorldPosition.y, playerPos.z);

        // Mapea la posici�n normalizada al tama�o del RectTransform del minimapa.
        // Se resta 0.5 para que el centro sea la posici�n (0,0).
        float minimapX = minimapRect.rect.width * (normalizedX - 0.5f);
        float minimapY = minimapRect.rect.height * (normalizedY - 0.5f);

        // Aplica la posici�n local, relativa al centro del minimapa.
        transform.localPosition = new Vector3(minimapX, minimapY, 0);

        // Rota el �cono para que apunte en la direcci�n del carro.
        // Se niega el �ngulo del eje Y del jugador para que coincida con el minimapa.
        transform.localRotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
    }

    // M�todo auxiliar para obtener los l�mites de un objeto (Renderer o Collider).
    private Bounds GetBoundsFromTransform(Transform target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null) return renderer.bounds;

        Collider collider = target.GetComponent<Collider>();
        if (collider != null) return collider.bounds;

        return new Bounds(Vector3.zero, Vector3.zero);
    }
}