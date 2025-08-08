using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class PathFollower : MonoBehaviour
{
    // Array de los puntos de paso (waypoints) por donde se moverá la cápsula
    // Este array se llenará automáticamente con los puntos del archivo
    private List<Vector3> waypoints;

    // Velocidad de movimiento de la cápsula
    public float speed = 5f;

    // Distancia mínima para considerar que se ha llegado al waypoint
    public float waypointDistanceThreshold = 0.5f;

    // Índice del waypoint actual al que se está moviendo la cápsula
    private int currentWaypointIndex = 0;

    // Referencia al objeto que se va a mover (la cápsula)
    public Transform objectToMove;

    // Referencia al Rigidbody para mover el objeto de forma suave
    private Rigidbody rb;

    void Awake()
    {
        // Define la ruta del archivo que guardaste
        string filePath = Application.persistentDataPath + "/saved_path1.txt";

        // Carga los puntos del archivo
        waypoints = LoadPath(filePath);

        if (objectToMove != null)
        {
            // Obtiene el componente Rigidbody del objeto
            rb = objectToMove.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Asegurarse de que hay puntos de paso y que el objeto a mover está asignado
        if (waypoints == null || waypoints.Count == 0 || objectToMove == null)
        {
            Debug.LogError("No se han cargado waypoints o el objeto a mover no está asignado.");
            return;
        }

        // Obtener la posición del waypoint de destino
        Vector3 targetWaypoint = waypoints[currentWaypointIndex];

        // Mover el objeto hacia el waypoint de destino
        MoveTowardsWaypoint(targetWaypoint);

        // Comprobar si se ha llegado lo suficientemente cerca del waypoint
        CheckIfWaypointReached(targetWaypoint);
    }

    private void MoveTowardsWaypoint(Vector3 target)
    {
        // Calcular la dirección del movimiento
        Vector3 direction = target - objectToMove.position;

        // Opcional: hacer que el objeto mire hacia el siguiente waypoint
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (rb != null)
            {
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, speed * Time.deltaTime));
            }
            else
            {
                objectToMove.rotation = Quaternion.Slerp(objectToMove.rotation, targetRotation, speed * Time.deltaTime);
            }
        }

        // Mover el objeto
        if (rb != null)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime));
        }
        else
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, target, speed * Time.deltaTime);
        }
    }

    private void CheckIfWaypointReached(Vector3 target)
    {
        // Calcular la distancia al waypoint actual
        float distanceToWaypoint = Vector3.Distance(objectToMove.position, target);

        // Si la distancia es menor que el umbral, pasamos al siguiente waypoint
        if (distanceToWaypoint < waypointDistanceThreshold)
        {
            // Incrementar el índice para el siguiente waypoint
            currentWaypointIndex++;

            // Si llegamos al final del array, volvemos al principio (crea un bucle)
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    // Función para cargar los puntos del archivo de texto
    private List<Vector3> LoadPath(string filePath)
    {
        List<Vector3> loadedPoints = new List<Vector3>();
        int lineNumber = 0;

        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    try
                    {
                        string[] coords = line.Split(',');
                        if (coords.Length == 3)
                        {
                            float x = float.Parse(coords[0], CultureInfo.InvariantCulture);
                            float y = float.Parse(coords[1], CultureInfo.InvariantCulture);
                            float z = float.Parse(coords[2], CultureInfo.InvariantCulture);
                            loadedPoints.Add(new Vector3(x, y, z));
                        }
                    }
                    catch (System.FormatException ex)
                    {
                        Debug.LogError($"Error al analizar la línea {lineNumber} del archivo. Verifique el formato de la línea: '{line}'. Error: {ex.Message}");
                    }
                }
            }
            Debug.Log("Camino cargado exitosamente. Puntos: " + loadedPoints.Count);
        }
        else
        {
            Debug.LogError("No se encontró el archivo del camino en: " + filePath);
        }

        return loadedPoints;
    }
}