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
    public float maxWheelTurn = 25f;                  // Ángulo visual máximo de giro de ruedas

    //--CHECK POINT Y VUELTAS
    public int nextCheckpoint;
    public int currentLap;

    void Start()
    {
        theRB.transform.parent = null;       //Separamos el Rigidbody del padre para que rote           // Separamos el Rigidbody del padre para que rote libremente
        dragOnGround = theRB.linearDamping;          // Guardamos el valor de fricción original

        //iniciar la vuelta en 1
        currentLap = 1;
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateLapCounter(currentLap);
        }
    }

    void Update()
    {
        // Entrada de aceleración hacia adelante o hacia atrás (W/S o ↑/↓)
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0f)
            speedInput = verticalInput * forwardAccel;
        else if (verticalInput < 0f)
            speedInput = verticalInput * reverseAccel;
        else
            speedInput = 0f;

        // Entrada de giro izquierda/derecha (A/D o ←/→)
        turnInput = Input.GetAxis("Horizontal");

        //Debug.Log("Vertical: " + verticalInput); //Juan Apaga esto un momento para probar la velocidad
        //Debug.Log("Horizontal: " + turnInput);

        // Rotación visual de ruedas delanteras
        if (leftFrontWheel != null)
        {
            leftFrontWheel.localRotation = Quaternion.Euler(
                leftFrontWheel.localRotation.eulerAngles.x,
                (turnInput * maxWheelTurn) - 180f,
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

        // Primer Raycast
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normalTarget = hit.normal;
        }

        // Segundo Raycast
        if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normalTarget = (normalTarget + hit.normal) / 2f;
        }

        // Alinea el coche con la pendiente del terreno
        if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
        }

        // Movimiento del coche
        if (grounded)
        {
            theRB.linearDamping = dragOnGround;
            theRB.AddForce(transform.right * speedInput * 1000f);   //LINEA QUE ESTA ANTES
            //theRB.AddForce(transform.forward * speedInput * 1000f);    //ESTO ES PARA PROBAR PARA VER SI CORRIGO LO DE MI CAMARA
        }
        else
        {
            theRB.linearDamping = dragOnGround;
            theRB.AddForce(-Vector3.up * gravityMod * 100f);
        }

        // Limita la velocidad máxima
        if (theRB.linearVelocity.magnitude > maxSpeed)
        {
            theRB.linearVelocity = theRB.linearVelocity.normalized * maxSpeed;
        }

        // Mueve visualmente el coche con el Rigidbody
        transform.position = theRB.position;

        // Rotación en curva si se está moviendo
        if (grounded && speedInput != 0f)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles +
                new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (theRB.linearVelocity.magnitude / maxSpeed), 0f));
        }

        //Agregado por Juan para verificar funcionamiento del tacometro
        // --- VERIFICACIÓN DE VELOCIDAD REAL DEL CARRO ---
        // Obtiene la magnitud de la velocidad lineal del Rigidbody (en metros/segundo)
        float velocidadActualMS = theRB.linearVelocity.magnitude;
        // Convierte metros/segundo a kilómetros/hora
        float velocidadActualKmH = velocidadActualMS * 3.6f;
        // Muestra la velocidad en la consola, sin decimales
        Debug.Log("Velocidad del carro: " + velocidadActualKmH.ToString("F0") + " km/h");

        //Debug.Log("SpeedInput: " + speedInput);
        //Debug.Log("Velocity: " + theRB.linearVelocity.magnitude);
    }

    /// <summary>
    /// cuando el vehículo atraviesa un checkpoint.
    /// /// </summary>
    
    // --- CHECKPOINT / LAP SYSTEM ---
    //CHECK POINT -METODO PARA DETECTAR CUANDO EL AUTO PASA POR UN CHECKPOINT
    public void CheckpointHit(int cpNumber)
    {
        if (cpNumber == nextCheckpoint)
        {
            
            nextCheckpoint++;

            if (nextCheckpoint == LapManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                currentLap++;

                Debug.Log("¡Nueva vuelta! Vuelta actual: " + currentLap);

                //ui
                if (UIManager.instance != null)
                {
                    UIManager.instance.UpdateLapCounter(currentLap);
                }
            }
        }
    }
}