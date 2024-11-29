using UnityEngine;

public class TriggerDissolve : MonoBehaviour
{
    public GameObject targetParent; // The parent object containing all children to dissolve

    // Assign the lights in the Inspector
    [SerializeField] private Light light1;
    [SerializeField] private Light light2;
    [SerializeField] private Light light3;

    // The target intensity value
    public float targetIntensity = 45f;

    // The speed of the intensity change
    public float intensityChangeSpeed = 5f;

    // Private variables to track if the trigger has been hit
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player triggers the dissolve
        {
            Debug.Log("Player entered trigger zone.");
            isTriggered = true; // Set the trigger for lights

            // Trigger dissolve for children
            if (targetParent != null)
            {
                TriggerDissolveForChildren();
            }
            else
            {
                Debug.LogWarning("Target parent is not assigned!");
            }
        }
    }

    private void TriggerDissolveForChildren()
    {
        // Get all children with the DissolveOffset script
        var dissolveScripts = targetParent.GetComponentsInChildren<DissolveOffest>();
        foreach (var dissolveScript in dissolveScripts)
        {
            dissolveScript.ActivateDissolve(); // Trigger dissolve on each child
            Debug.Log($"Dissolving {dissolveScript.gameObject.name}");
        }
    }

    private void Update()
    {
        if (isTriggered)
        {
            // Gradually increase the intensity of each light
            light1.intensity = Mathf.MoveTowards(light1.intensity, targetIntensity, intensityChangeSpeed * Time.deltaTime);
            light2.intensity = Mathf.MoveTowards(light2.intensity, targetIntensity, intensityChangeSpeed * Time.deltaTime);
            light3.intensity = Mathf.MoveTowards(light3.intensity, targetIntensity, intensityChangeSpeed * Time.deltaTime);

            // Optional: Reset trigger after lights reach the target intensity
            if (light1.intensity == targetIntensity && light2.intensity == targetIntensity && light3.intensity == targetIntensity)
            {
                isTriggered = false; // Stop updating after reaching the target intensity
                Debug.Log("Lights have reached target intensity.");
            }
        }
    }
}
