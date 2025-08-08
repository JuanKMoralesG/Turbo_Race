using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PortadaController : MonoBehaviour
{
    public AudioSource audioSource;
    //public float delay = 3f; // Duración del sonido
    private bool sceneLoaded = false;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se asignó el AudioSource.");
        }

        //Invoke("CargarMenuPrincipal", delay);
    }

    private void Update()
    {
        // Si el audio terminó de reproducirse y no hemos cargado la escena todavía
        if (audioSource != null && !audioSource.isPlaying && !sceneLoaded)
        {
            sceneLoaded = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}