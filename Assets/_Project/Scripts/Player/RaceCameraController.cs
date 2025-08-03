using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ajusta la posición de la cámara detrás del auto,
/// alejándose cuando el auto va rápido y acercándose cuando va lento.
/// </summary>

public class RaceCameraController : MonoBehaviour
{
    // --- COMPONENTES Y AJUSTES PRINCIPALES ---

    //Referencia al scrite del carro (CarController), para saber la velocidad
    public CarController target;

    private Vector3 offsetDir;  // Dirección desde la posición inicial de la cámara hasta el carro

    public float minDistance;  // Distancias mínimas y máximas de la cámara al carro
    public float maxDistance;

    private float activeDistance;  //variable que va cambiando según la velocidad del carro

    public Transform startTargetOffset;   // Punto de referencia para calcular la dirección inicial de la cámara

    void Start()
    {
        offsetDir = transform.position - startTargetOffset.position; //dirección inicial desde el carro hasta la cámara 

        activeDistance = minDistance;  //Comenando la camara estara en la distancia minima

        offsetDir.Normalize();  // Normalizamos la dirección magnitud 1
    }

    void Update()
    {
        // Calculamos la distancia en función de la velocidad actual del carro
        activeDistance = minDistance + ((maxDistance - minDistance) * (target.theRB.linearVelocity.magnitude / target.maxSpeed));

        // Colocamos la cámara detrás del carro, ajustando la distancia
        transform.position = target.transform.position + (offsetDir * activeDistance);
    }
}
