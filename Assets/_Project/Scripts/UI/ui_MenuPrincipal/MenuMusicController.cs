using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    public AudioSource musicSource;

    private void Start()
    {
        if (musicSource != null)
        {
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("No se asignó el AudioSource.");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
