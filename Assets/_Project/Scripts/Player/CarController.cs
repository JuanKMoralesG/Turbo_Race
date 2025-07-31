using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controlador principal para el vehículo del jugador.
/// Administra movimiento, aceleración, giro, detección de suelo y físicas.
/// </summary>

public class CarController : MonoBehaviour
{
    // --- COMPONENTES Y AJUSTES PRINCIPALES ---
    public Rigidbody theRB;                   // Rigidbody que controla la física del vehículo
    public float maxSpeed = 20f;              // Velocidad máxima permitida
    public float forwardAccel = 8f;           // Aceleración hacia adelante
    public float reverseAccel = 4f;           // Aceleración hacia atrás
    private float speedInput;                 // Entrada actual de aceleración
    public float turnStrength = 180f;         // Intensidad del giro del vehículo
    private float turnInput;                  // Entrada de dirección (izquierda/derecha)
    private bool grounded;                    // ¿Está tocando el suelo?

    // --- DETECCIÓN DE SUELO ---
    public Transform groundRayPoint, groundRayPoint2; // Puntos desde los cuales lanzamos rayos hacia el suelo
    public LayerMask whatIsGround;                    // Capa del suelo
    public float groundRayLength = 0.75f;             // Longitud de los rayos

    private float dragOnGround;            // Resistencia al moverse sobre el suelo
    public float gravityMod = 10f;         // Multiplicador de gravedad cuando está en el aire

    // --- CONTROL VISUAL DE RUEDAS DELANTERAS ---
    public Transform leftFrontWheel, rightFrontWheel; // Ruedas delanteras para rotación visual
    public float maxWheelTurn = 25f;                   // Ángulo visual máximo de giro de ruedas

    void Start()
    {
        // Separamos el Rigidbody del padre para que rote libremente
        theRB.transform.parent = null;

        // Guardamos el valor de damping original (fricción)
        dragOnGround = theRB.linearDamping;
    }

    void Update()
    {
        speedInput = 0f;

        // Entrada de aceleración hacia adelante
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }

        // Entrada de retroceso
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
        }

        // Entrada de giro
        turnInput = Input.GetAxis("Horizontal");

        // Rotación visual de ruedas delanteras
        if (leftFrontWheel != null)
        {
            leftFrontWheel.localRotation = Quaternion.Euler(
                leftFrontWheel.localRotation.eulerAngles.x,
                (turnInput * maxWheelTurn) - 180,
                leftFrontWheel.localRotation.eulerAngles.z);
        }

        if (rightFrontWheel != null)
        {
            rightFrontWheel.localRotation = Quaternion.Euler(
                rightFrontWheel.localRotation.eulerAngles.x,
                (turnInput * maxWheelTurn),
                rightFrontWheel.localRotation.eulerAngles.z);
        }
    }

    void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;
        Vector3 normalTarget = Vector3.zero;

        // Raycast 1
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normalTarget = hit.normal;
        }

        // Raycast 2
        if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normalTarget = (normalTarget + hit.normal) / 2f;
        }

        // Alinea el coche con el terreno
        if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }

        // Aplicación de fuerzas
        if (grounded)
        {
            theRB.linearDamping = dragOnGround;
            theRB.AddForce(transform.forward * speedInput * 1000f);
        }
        else
        {
            theRB.linearDamping = 0.1f;
            theRB.AddForce(-Vector3.up * gravityMod * 100f);
        }

        // Limitar velocidad máxima
        if (theRB.linearVelocity.magnitude > maxSpeed)
        {
            theRB.linearVelocity = theRB.linearVelocity.normalized * maxSpeed;
        }

        // Mueve visualmente el coche con el Rigidbody
        transform.position = theRB.position;

        // Gira el coche si está acelerando
        if (grounded && speedInput != 0)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles +
                new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.linearVelocity.magnitude / maxSpeed), 0f));
        }
    }
}
