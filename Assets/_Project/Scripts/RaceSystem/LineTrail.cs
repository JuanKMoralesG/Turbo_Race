using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class LineTrail : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform carTransform;
    private List<Vector3> points = new List<Vector3>();

    // Agrega esta variable para controlar la distancia mínima entre puntos.
    public float minDistanceBetweenPoints = 2f;
    private Vector3 lastRecordedPosition;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lastRecordedPosition = carTransform.position;
    }

    void Update()
    {
        if (carTransform != null)
        {
            // Solo graba un nuevo punto si el carro se ha movido lo suficiente.
            if (Vector3.Distance(carTransform.position, lastRecordedPosition) > minDistanceBetweenPoints)
            {
                points.Add(carTransform.position);
                lastRecordedPosition = carTransform.position;
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SavePath();
        }
    }

    public void SavePath()
    {
        string filePath = Application.persistentDataPath + "/saved_path.txt";

        if (points.Count > 0)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (Vector3 point in points)
                {
                    sw.WriteLine(point.x.ToString(CultureInfo.InvariantCulture) + "," +
                                 point.y.ToString(CultureInfo.InvariantCulture) + "," +
                                 point.z.ToString(CultureInfo.InvariantCulture));
                }
            }
            Debug.Log("Camino guardado exitosamente. Puntos: " + points.Count);
        }
        else
        {
            Debug.LogWarning("La lista de puntos está vacía. No hay nada que guardar.");
        }
    }
}