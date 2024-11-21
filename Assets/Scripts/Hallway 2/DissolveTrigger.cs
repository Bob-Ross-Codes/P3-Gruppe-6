using UnityEngine;

public class DissolveTrigger : MonoBehaviour
/*{
    public DissolveObject dissolveObject;
    public float offsetDistance = 2f; // Distance in front of the player

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Debug.Log("Player detected. Starting dissolve...");

            // Calculate position in front of the player
            Vector3 forwardPosition = other.transform.position + other.transform.forward * offsetDistance;

            // Move the dissolve effect to that position (optional, for visual alignment)
            dissolveObject.transform.position = forwardPosition;

            // Start the dissolve
            dissolveObject.StartDissolve();
        }
            if (other.CompareTag("Player"))
   
    }
}*/


{
    public DissolveObject dissolveObject;
    public float offsetDistance = 2f; // Distance in front of the player

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
               // Start the dissolve effect
            dissolveObject.StartDissolve();
        }
    }
}

