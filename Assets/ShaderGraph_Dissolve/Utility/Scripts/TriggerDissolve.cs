using UnityEngine;

public class TriggerDissolve : MonoBehaviour

/*{
    public DissolveOffest dissolveScript; // Reference to the dissolve script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player entering the zone
        {
            Debug.Log("Player entered the trigger zone.");
            dissolveScript.playerInTriggerZone = true; // Inform the dissolve script
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player exiting the zone
        {
            Debug.Log("Player exited the trigger zone.");
            dissolveScript.playerInTriggerZone = false; // Reset the dissolve trigger
        }
    }
}*/


{
    public OverallDissolveController dissolveController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone. Triggering dissolve.");
            dissolveController.TriggerDissolve();
        }
    }
}
