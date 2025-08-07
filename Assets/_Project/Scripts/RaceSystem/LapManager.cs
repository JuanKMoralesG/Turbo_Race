using UnityEngine;

/// <summary>
///Controla todos los checkpoints y gestiona el conteo de vueltas.
/// </summary>

public class LapManager : MonoBehaviour
{
    public static LapManager instance;

    public RaceCheckpoint[] allCheckpoints;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].cpNumber = i;
        }
    }


}
