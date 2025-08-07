using UnityEngine;

/// <summary>
/// Detecta cuándo el carro entra en un checkpoint.
/// </summary>

public class CheckpointTrigger : MonoBehaviour
{
    public CarController theCar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            RaceCheckpoint checkpoint = other.GetComponent<RaceCheckpoint>();
            if (checkpoint != null)
            {
                theCar.CheckpointHit(checkpoint.cpNumber);
            }
        }
    }
}
