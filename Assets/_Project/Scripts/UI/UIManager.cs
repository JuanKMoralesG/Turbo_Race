using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Referencias UI")]
    public TMP_Text lapCounterText;
    public TMP_Text currentLapTimeText;

    [Header("Parámetros de la carrera")]
    public int totalLaps = 3;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateLapTimeDisplay();
    }

    public void UpdateLapCounter(int currentLap)
    {
        lapCounterText.text = currentLap + "/" + totalLaps;
    }

    private void UpdateLapTimeDisplay()
    {
        float currentLapTime = Time.timeSinceLevelLoad;  // Esto lo reemplazaremos después con un tiempo real de vuelta
        currentLapTimeText.text = FormatTime(currentLapTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
