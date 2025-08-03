using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permite cambiar entre diferentes cámaras del juego
/// Al presionar la tecla "Barra Espaciadora" , se alterna la cámara activa.
/// </summary>


public class CameraModeSwitcher : MonoBehaviour
{
    public GameObject[] cameras;  // Lista de cámaras que se pueden alternar
    private int currentCam;   // Índice actual de la cámara activa

    void Start()
    {
        //Para asegurarnos que solo 1 camara este activa
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == 0);
        }
        currentCam = 0;
    }

    void Update()
    {
        // Presiona BARRA ESPACIADORA para cambiar de cámara
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentCam++;                            // Avanzamos al siguiente índice

            // Si el índice supera el número de cámaras, volvemos a la primera
            if (currentCam >= cameras.Length)
            {
                currentCam = 0;
            }

            // Activamos la cámara correspondiente y desactivamos las demás
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(i == currentCam);
            }
        }
    }   
}
