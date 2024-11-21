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
    public GameObject targetParent; // The parent object containing all children to dissolve

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player triggers the dissolve
        {
            Debug.Log($"Player entered trigger zone. Triggering dissolve for {targetParent.name}");
            TriggerDissolveForChildren();
        }
    }

    private void TriggerDissolveForChildren()
    {
        if (targetParent == null)
        {
            Debug.LogWarning("Target parent is not assigned!");
            return;
        }

        // Get all children with the DissolveOffest script
        var dissolveScripts = targetParent.GetComponentsInChildren<DissolveOffest>();
        foreach (var dissolveScript in dissolveScripts)
        {
            dissolveScript.ActivateDissolve(); // Trigger dissolve on each child
            Debug.Log($"Dissolving {dissolveScript.gameObject.name}");
        }
    }
}

