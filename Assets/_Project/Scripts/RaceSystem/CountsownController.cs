using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CountdownController : MonoBehaviour
{
    // Asigna este texto desde el Inspector de Unity
    public TMP_Text countdownText;

    // El tiempo del contador
    public float countdownTime = 5f;

    // Referencia al coche del jugador
    // Asigna tu script de control del coche aquí
    public MonoBehaviour playerCarScript;

    // Referencias a los scripts de los otros competidores
    // Arrastra y suelta los scripts de los otros coches aquí
    public MonoBehaviour[] opponentCarScripts;

    private void Start()
    {
        // Al inicio, inicia la corrutina del contador
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        // Desactiva el control del coche del jugador
        if (playerCarScript != null)
        {
            playerCarScript.enabled = false;
        }

        // Desactiva el control de los coches de los oponentes
        foreach (var script in opponentCarScripts)
        {
            if (script != null)
            {
                script.enabled = false;
            }
        }

        // Bucle del contador
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString("0"); // Muestra el número entero
            yield return new WaitForSeconds(1f); // Espera 1 segundo
            countdownTime--; // Disminuye el tiempo
        }

        // Muestra el texto "GO!"
        countdownText.text = "¡GO!";
        yield return new WaitForSeconds(1f); // Espera 1 segundo antes de desaparecer

        // Oculta el texto del contador
        countdownText.gameObject.SetActive(false);

        // Habilita el control del coche del jugador
        if (playerCarScript != null)
        {
            playerCarScript.enabled = true;
        }

        // Habilita el control de los coches de los oponentes
        foreach (var script in opponentCarScripts)
        {
            if (script != null)
            {
                script.enabled = true;
            }
        }
    }
}