using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu_Controller : MonoBehaviour
{
    public MenuMusicController menuMusicController;

    public void OnPlayPressed()
    {
        if (menuMusicController != null)
            menuMusicController.StopMusic();

        SceneManager.LoadScene("Track1-Spielberg");
    }

    public void OnOptionsPressed()
    {
        if (menuMusicController != null)
            menuMusicController.StopMusic();

        SceneManager.LoadScene("OptionMenu");
    }

    public void OnQuitPressed()
    {
        if (menuMusicController != null)
            menuMusicController.StopMusic();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
