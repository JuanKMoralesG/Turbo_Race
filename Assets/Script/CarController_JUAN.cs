using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class CarController_JUAN : MonoBehaviour
{
    public float velocidadMovimiento = 50f; // Velocidad para avanzar/retroceder
    public float aceleracionRate = 2f;       // Qu� tan r�pido alcanza la velocidad m�xima (factor de aceleraci�n)
    public float desaceleracionRate = 1f;    // Qu� tan r�pido disminuye la velocidad (ajusta esto para una parada m�s gradual)
    public float velocidadRotacion = 100f; // Velocidad para girar (en grados por segundo)

    public Rigidbody carRb;
    public Vector3 _centroDeMasa = new Vector3(0, -1f, 0);

    // Agrega una variable p�blica para controlar la intensidad del downforce
    public float downforceFactor = 50f;

    // Esta variable almacenar� la velocidad simulada actual del carro
    private float currentSimulatedSpeed = 0f;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.useGravity = true;
        carRb.centerOfMass = _centroDeMasa; // Un centro de masa bajo
        carRb.linearDamping = 0.5f;     // Resistencia al movimiento lineal. Ajusta seg�n sea necesario.
        carRb.angularDamping = 5f; // Resistencia a la rotaci�n. Este es crucial para el giro. Ajusta este valor.
    }

    void FixedUpdate()
    {
        // Aplica un downforce (fuerza hacia abajo)
        // carRb.velocity.magnitude te da la velocidad actual del Rigidbody en m/s
        carRb.AddForce(-transform.up * carRb.linearVelocity.magnitude * downforceFactor);

        // --- Movimiento Adelante/Atr�s ---
        float inputVertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(inputVertical) > 0.1f) // Si hay una entrada significativa (adelante/atr�s)
        {
            // Acelera hacia la velocidad objetivo (inputVertical * velocidadMovimiento)
            // Mathf.Lerp mueve 'currentSimulatedSpeed' suavemente hacia el objetivo.
            currentSimulatedSpeed = Mathf.Lerp(currentSimulatedSpeed, inputVertical * velocidadMovimiento, Time.fixedDeltaTime * aceleracionRate);
        }
        else // No hay entrada, as� que desacelera
        {
            // Desacelera suavemente hacia cero
            currentSimulatedSpeed = Mathf.Lerp(currentSimulatedSpeed, 0f, Time.fixedDeltaTime * desaceleracionRate);
        }

        // --- Aplicar Movimiento Simulado ---
        // Usa la currentSimulatedSpeed para calcular el desplazamiento
        Vector3 desplazamiento = transform.forward * currentSimulatedSpeed * Time.fixedDeltaTime;
        carRb.MovePosition(carRb.position + desplazamiento);


        // --- Rotaci�n (Giro) ---
        float inputHorizontal = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * inputHorizontal * velocidadRotacion * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);

        // --- C�lculo y Mostrar la Velocidad Actual del Carro ---
        // Obtiene la magnitud de la velocidad lineal del Rigidbody (en metros/segundo)
        //float velocidadActualMS = carRb.linearVelocity.magnitude;
        float velocidadActualMS = MathF.Abs(inputVertical) * velocidadMovimiento;

        // Convierte metros/segundo a kil�metros/hora
        float velocidadActualKmH = velocidadActualMS * 3.6f;

        // Muestra la velocidad en la consola, sin decimales
        Debug.Log("Velocidad del carro: " + velocidadActualKmH.ToString("F0") + " km/h");

    }

}