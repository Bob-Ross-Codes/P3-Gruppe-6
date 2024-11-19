using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public DoorOpenClose doorScript; // Reference to the DoorOpenClose script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            if (doorScript != null) // Ensure the door script is assigned
            {
                doorScript.enabled = false; // Disable the door script
                Debug.Log("Door interaction disabled.");
            }
        }
    }

  
}

