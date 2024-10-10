using UnityEngine;
using FMODUnity;

public class ReverbZone : MonoBehaviour
{
    [SerializeField] [FMODUnity.EventRef] public string snapshotPath; // Path to the FMOD snapshot

    private FMOD.Studio.EventInstance snapshotInstance;

    void Start()
    {
        // Create the snapshot instance
        snapshotInstance = FMODUnity.RuntimeManager.CreateInstance(snapshotPath);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activate the snapshot when player enters the zone
            snapshotInstance.start();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deactivate the snapshot when player exits the zone
            snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnDestroy()
    {
        // Release the snapshot when the object is destroyed
        snapshotInstance.release();
    }
}
