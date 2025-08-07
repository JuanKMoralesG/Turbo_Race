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

    private float raceTime;
    private bool raceOngoing;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        raceTime = 0f;
        raceOngoing = true; // Empieza la carrera desde el principio
    }

    void Update()
    {
        if (raceOngoing)
        {
            raceTime += Time.deltaTime;
            UpdateLapTimeDisplay(raceTime);
        }
    }

    public void UpdateLapCounter(int currentLap)
    {
        lapCounterText.text = currentLap + "/" + totalLaps;

        // Si terminó la carrera, detenemos el conteo
        if (currentLap > totalLaps)
        {
            raceOngoing = false;
        }
    }

    private void UpdateLapTimeDisplay(float time)
    {
        currentLapTimeText.text = FormatTime(time);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public float GetRaceTime()
    {
        return raceTime;
    }
}
