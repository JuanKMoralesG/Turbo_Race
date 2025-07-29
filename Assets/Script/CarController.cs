using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class CarController : MonoBehaviour
{
    public float velocidadMovimiento = 50f; // Velocidad para avanzar/retroceder
    public float aceleracionRate = 2f;       // Qué tan rápido alcanza la velocidad máxima (factor de aceleración)
    public float desaceleracionRate = 1f;    // Qué tan rápido disminuye la velocidad (ajusta esto para una parada más gradual)
    public float velocidadRotacion = 100f; // Velocidad para girar (en grados por segundo)

    public Rigidbody carRb;
    public Vector3 _centroDeMasa = new Vector3(0, -1f, 0);

    // Agrega una variable pública para controlar la intensidad del downforce
    public float downforceFactor = 50f;

    // Esta variable almacenará la velocidad simulada actual del carro
    private float currentSimulatedSpeed = 0f;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.useGravity = true;
        carRb.centerOfMass = _centroDeMasa; // Un centro de masa bajo
        carRb.linearDamping = 0.5f;     // Resistencia al movimiento lineal. Ajusta según sea necesario.
        carRb.angularDamping = 5f; // Resistencia a la rotación. Este es crucial para el giro. Ajusta este valor.
    }

    void FixedUpdate()
    {
        // Aplica un downforce (fuerza hacia abajo)
        // carRb.velocity.magnitude te da la velocidad actual del Rigidbody en m/s
        carRb.AddForce(-transform.up * carRb.linearVelocity.magnitude * downforceFactor);

        // --- Movimiento Adelante/Atrás ---
        float inputVertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(inputVertical) > 0.1f) // Si hay una entrada significativa (adelante/atrás)
        {
            // Acelera hacia la velocidad objetivo (inputVertical * velocidadMovimiento)
            // Mathf.Lerp mueve 'currentSimulatedSpeed' suavemente hacia el objetivo.
            currentSimulatedSpeed = Mathf.Lerp(currentSimulatedSpeed, inputVertical * velocidadMovimiento, Time.fixedDeltaTime * aceleracionRate);
        }
        else // No hay entrada, así que desacelera
        {
            // Desacelera suavemente hacia cero
            currentSimulatedSpeed = Mathf.Lerp(currentSimulatedSpeed, 0f, Time.fixedDeltaTime * desaceleracionRate);
        }

        // --- Aplicar Movimiento Simulado ---
        // Usa la currentSimulatedSpeed para calcular el desplazamiento
        Vector3 desplazamiento = transform.forward * currentSimulatedSpeed * Time.fixedDeltaTime;
        carRb.MovePosition(carRb.position + desplazamiento);


        // --- Rotación (Giro) ---
        float inputHorizontal = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * inputHorizontal * velocidadRotacion * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);

        // --- Cálculo y Mostrar la Velocidad Actual del Carro ---
        // Obtiene la magnitud de la velocidad lineal del Rigidbody (en metros/segundo)
        //float velocidadActualMS = carRb.linearVelocity.magnitude;
        float velocidadActualMS = MathF.Abs(inputVertical) * velocidadMovimiento;

        // Convierte metros/segundo a kilómetros/hora
        float velocidadActualKmH = velocidadActualMS * 3.6f;

        // Muestra la velocidad en la consola, sin decimales
        Debug.Log("Velocidad del carro: " + velocidadActualKmH.ToString("F0") + " km/h");

    }

}