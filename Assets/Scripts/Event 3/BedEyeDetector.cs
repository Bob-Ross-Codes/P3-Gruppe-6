using System.Collections;
using UnityEngine;

public class BedEyeDetector : GazeActivation
{
    public JournalEyeDetector journalEyeDetector;
    public LightManager lightManager;  // Reference to the LightManager script
    public override float ActivationTime => 0.1f;

    public GameObject targetGameObject;  // Reference to the GameObject with SpriteFade attached
    private BlurryText blurryText;       // Reference to the SpriteFade script
    [SerializeField] private GameObject patient;

    private void Awake()
    {
        
    }

    void Start()
    {
        if (targetGameObject != null)
        {
            blurryText = targetGameObject.GetComponent<BlurryText>();

            if (blurryText == null)
            {
                Debug.LogError("SpriteFade component not found on " + targetGameObject.name);
            }
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned.");
        }
    }

    public override void OnLookedAt()
    {
        patient.SetActive(false);

        blurryText.PauseFadeOut(false);

        if (journalEyeDetector.flickerCount == journalEyeDetector.countForJumpscare && journalEyeDetector.jumpScare == false)
        {
            Debug.Log("flickerCount = countForJumpscare, triggering JumpScare");
            journalEyeDetector.StartJumpScare();
        }
        else if (lightManager.flickeringOn && journalEyeDetector.jumpScare == false)
        {
            //journalEyeDetector.StartPatientGone();
            lightManager.flickeringOn = false;
        }

        // Start blurring the journal
        blurryText.StartFadeOut();
    }
}
