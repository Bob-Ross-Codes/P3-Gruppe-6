using UnityEngine;

/// <summary>
/// Triggers a dissolve effect for all child objects and gradually increases the intensity of specified lights
/// when the player enters the trigger zone.
/// </summary>
public class TriggerDissolve : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The parent object containing all child objects to dissolve.")]
    public GameObject targetParent;
    public DissolveOffset dissolveOffset;

    [Header("Lights Settings")]
    [Tooltip("The first light to adjust intensity.")]
    [SerializeField] private Light light1;

    [Tooltip("The second light to adjust intensity.")]
    [SerializeField] private Light light2;

    [Tooltip("The third light to adjust intensity.")]
    [SerializeField] private Light light3;

    [Header("Intensity Settings")]
    [Tooltip("The target intensity value for the lights.")]
    public float targetIntensity = 45f;

    [Tooltip("The speed at which the light intensity changes.")]
    public float intensityChangeSpeed = 5f;

    // Private variable to track if the trigger has been hit
    private bool isTriggered = false;

    /// <summary>
    /// Detects when the player enters the trigger zone.
    /// Activates the dissolve effect for child objects.
    /// </summary>
    /// <param name="other">The collider that enters the trigger zone.</param>
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
                Debug.LogWarning("Target Parent is not assigned in the Inspector.");
            }
        }
    }

    /// <summary>
    /// Triggers the dissolve effect for all child objects with the DissolveOffset script.
    /// </summary>
    private void TriggerDissolveForChildren()
    {
        // Get all children with the DissolveOffset script
        var dissolveScripts = targetParent.GetComponentsInChildren<DissolveOffset>();
        foreach (var dissolveScript in dissolveScripts)
        {
            dissolveScript.ActivateDissolve(); // Trigger dissolve on each child
            Debug.Log($"Dissolving {dissolveScript.gameObject.name}");
        }
    }

    /// <summary>
    /// Gradually increases the intensity of lights each frame until they reach the target intensity.
    /// </summary>
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