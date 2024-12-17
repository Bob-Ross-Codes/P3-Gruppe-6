/// <summary>
/// Detects when the player looks at the bed and triggers the associated effects.
/// </summary>
using UnityEngine;

public class BedEyeDetector : GazeActivation
{
    public JournalEyeDetector journalEyeDetector; // Reference to the JournalEyeDetector script
    public LightManager lightManager; // Reference to the LightManager script
    public override float ActivationTime => 0.2f; // Time required to trigger the effect

    public GameObject targetGameObject; // GameObject with the BlurryText script
    private BlurryText blurryText; // Reference to the BlurryText component
    [SerializeField] private GameObject patient; // Reference to the patient GameObject

    private void Start()
    {
        // Ensure the targetGameObject is assigned and has a BlurryText component
        if (targetGameObject != null)
        {
            blurryText = targetGameObject.GetComponent<BlurryText>();
            if (blurryText == null)
                Debug.LogError($"BlurryText component not found on {targetGameObject.name}");
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned.");
        }
    }

    /// <summary>
    /// Triggers the bed's effect when the player looks at it.
    /// </summary>
    public override void OnLookedAt()
    {
        Debug.Log("Player looked at the bed");

        // Resume blurry text fade if applicable
        blurryText.PauseFadeOut(false);

        // Handle light flickering and journal jumpscare logic
        if (journalEyeDetector.lookAtCount != journalEyeDetector.jumpScareCount)
        {
            lightManager.StopFlicker();
        }
        else
        {
            journalEyeDetector.jumpScare = true;
        }

        // Start blurry text fade effect
        blurryText.StartFadeOut();
    }
}