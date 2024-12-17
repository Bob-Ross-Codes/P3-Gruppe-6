using System.Collections;
using UnityEngine;
using StarterAssets;

/// <summary>
/// Manages environmental transformations, light controls, and player interactions.
/// </summary>
public class TurnWorld : MonoBehaviour
{
    [Header("Lights Settings")]
    [SerializeField] private Light[] AllLights; // Array of all lights to turn off/on
    [SerializeField] private Light[] RedLights; // Array of red lights to activate
    public LightManager lightManager; // Reference to LightManager

    [Header("GameObject Settings")]
    [SerializeField] private GameObject[] Journals; // Journals to destroy during transformation
    [SerializeField] private GameObject hallway;    // Hallway to rotate and move
    [SerializeField] private GameObject handLight;  // Handheld light for flickering effects
    [SerializeField] private GameObject exitSign;   // Exit sign for red light phase
    [SerializeField] private GameObject exitSign2;  // Exit sign for normal phase
    [SerializeField] private GameObject spawnAgain; // Spawn point for resetting the player
    [SerializeField] private GameObject MonsterSound; // Object to control monster sounds

    [Header("Player Settings")]
    public FirstPersonController playerController; // Reference to the FirstPersonController script
    public Transform player; // Player's transform
    public GameObject Player; // Player's GameObject
    [SerializeField] private Transform trigger;  // Trigger for activating the red phase
    [SerializeField] private Transform trigger2; // Trigger for resetting the world

    // Removed: RunHallwayChanger hallwayChanger; // Reference removed to avoid CS0246 error

    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f); // Initial rotation of the hallway
    private Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f); // Target rotation of the hallway
    private Vector3 initialPosition; // Initial position of the hallway
    private Vector3 targetPosition;  // Target position of the hallway

    private bool triggered = false;       // Flag for activating the red phase
    private bool triggeredToNormal = false; // Flag for resetting the world
    private bool handLightDead = false;   // Is the handheld light flickering?
    private bool redLightsOn = false;     // Are the red lights active?
    private bool runningOnce = false;     // Ensures actions in the coroutine run only once
    private float initialPlayerSpeed;     // Stores the player's initial movement speed
    private const float interactionRange = 6f; // Range for interacting with triggers

    private void Start()
    {
        initialPlayerSpeed = playerController.MoveSpeed;
        initialPosition = hallway.transform.localPosition;
        targetPosition = initialPosition + new Vector3(0f, -12f, -25f);

        hallway.transform.localRotation = initialRotation;
        exitSign.SetActive(false);
        exitSign2.SetActive(false);
        SetLightsEnabled(RedLights, false);
    }

    private void FixedUpdate()
    {
        // Activate red phase if player is near the first trigger
        if (!triggered && Vector3.Distance(player.position, trigger.position) < interactionRange)
        {
            StartCoroutine(TurnToRed(4f));
            triggered = true;
        }

        // Reset world if player is near the second trigger
        if (!triggeredToNormal && Vector3.Distance(player.position, trigger2.position) < interactionRange)
        {
            StartCoroutine(TurnToNormal(2f));
            triggeredToNormal = true;
        }
    }

    /// <summary>
    /// Activates the red phase by rotating and moving the hallway, disabling normal lights, and starting flickering effects.
    /// </summary>
    private IEnumerator TurnToRed(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            hallway.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            hallway.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            yield return null;

            if (!runningOnce)
            {
                lightManager.StartFlicker(duration, 0.6f, false);
                foreach (var journal in Journals)
                {
                    Destroy(journal);
                }
                runningOnce = true;
            }

            elapsedTime += Time.deltaTime;
        }

        hallway.transform.rotation = targetRotation;
        hallway.transform.localPosition = targetPosition;

        yield return new WaitForSeconds(duration / 2);
        SetLightsEnabled(AllLights, false);

        playerController.MoveSpeed = 0;
        StartCoroutine(TurnRedLights());

        while (handLightDead)
        {
            float darkInterval = Random.Range(0.1f, 0.4f);
            float lightInterval = Random.Range(0.05f, 0.1f);

            handLight.SetActive(true);
            yield return new WaitForSeconds(darkInterval);
            handLight.SetActive(false);
            yield return new WaitForSeconds(lightInterval);
        }

        handLight.SetActive(false);
        SetLightsEnabled(AllLights, false);
    }

    /// <summary>
    /// Activates the red lights sequentially while playing sound effects and re-enabling player movement.
    /// </summary>
    private IEnumerator TurnRedLights()
    {
        redLightsOn = true;
        yield return new WaitForSeconds(2f);

        handLightDead = true;
        SetLightsEnabled(AllLights, false);

        AkSoundEngine.SetRTPCValue("RTPC_LightState", 0, Player);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < RedLights.Length; i += 2)
        {
            if (i < RedLights.Length) RedLights[i].enabled = true;
            if (i + 1 < RedLights.Length) RedLights[i + 1].enabled = true;

            AkSoundEngine.SetRTPCValue("RTPC_LightState", 1, Player);
            AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);

            yield return new WaitForSeconds(1f);
        }

        MonsterSound.SetActive(true);
        playerController.MoveSpeed = initialPlayerSpeed;
        exitSign.SetActive(true);
    }

    /// <summary>
    /// Resets the hallway, lights, and player position to their initial state.
    /// </summary>
    private IEnumerator TurnToNormal(float duration)
    {
        lightManager.StartFlicker(1f, 1.5f, true);
        yield return new WaitForSeconds(duration / 4);

        AkSoundEngine.PostEvent("Stop_Monster_Sounds", MonsterSound);
        MonsterSound.SetActive(false);
        exitSign2.SetActive(true);

        hallway.transform.localRotation = initialRotation;
        hallway.transform.localPosition = initialPosition;
        player.transform.position = spawnAgain.transform.position;

        yield return new WaitForSeconds(duration / 4);

        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);
        SetLightsEnabled(RedLights, false);
        exitSign.SetActive(false);

        // Removed the call to hallwayChanger.StartChase() since hallwayChanger is no longer defined.

        yield return new WaitForSeconds(duration / 2);

        SetLightsEnabled(AllLights, true);
        handLight.SetActive(true);
        exitSign.SetActive(false);
    }

    /// <summary>
    /// Enables or disables the specified array of lights.
    /// </summary>
    /// <param name="lights">The array of lights to enable or disable.</param>
    /// <param name="enabled">True to enable lights, false to disable.</param>
    private void SetLightsEnabled(Light[] lights, bool enabled)
    {
        foreach (var light in lights)
        {
            light.enabled = enabled;
        }
    }
}