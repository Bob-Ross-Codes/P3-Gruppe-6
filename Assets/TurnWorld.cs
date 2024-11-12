using System.Collections;
using UnityEngine;

public class TurnWorld : MonoBehaviour
{
    public Light[] AllLights;       // Array of lights to turn off/on
    public Light[] RedLights;       // Array of lights to turn on
    public GameObject[] Journals;   // Array of journals to destroy

    [SerializeField] private Transform player;
    [SerializeField] private Transform trigger;
    [SerializeField] private Transform trigger2;
    [SerializeField] private GameObject hallway;
    [SerializeField] private GameObject handLight;
    [SerializeField] private GameObject exitSign;

    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f);
    private Quaternion newRotation = Quaternion.Euler(90f, 0f, 0f);
    private float interactionRange = 6f; // Set the interaction range
    private bool triggered;
    private bool triggeredToNormal; // Flag for TurnToNormal coroutine

    void Start()
    {
        triggered = false;
        triggeredToNormal = false;
        hallway.transform.localRotation = initialRotation;
        exitSign.SetActive(false);
        SetLightsEnabled(RedLights, false);
        SetLightsEnabled(AllLights, true);
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.position, trigger.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // Start TurnToRed coroutine if within range and not triggered
        if (distanceToPlayer < interactionRange && !triggered)
        {
            StartCoroutine(TurnToRed(4f)); // Start TurnToRed coroutine
            triggered = true;
        }

        float distanceToPlayer2 = Vector3.Distance(player.position, trigger2.position);
        // Start TurnToNormal coroutine if within range and not triggered to normal
        if (distanceToPlayer2 < interactionRange && !triggeredToNormal)
        {
            StartCoroutine(TurnToNormal(4f)); // Start TurnToNormal coroutine
            triggeredToNormal = true;
        }
    }

    private IEnumerator TurnToRed(float duration)
    {
        Destroy(handLight);

        float elapsedTime = 0f;
        Vector3 initialPosition = hallway.transform.localPosition; // Store initial position
        Vector3 targetPosition = initialPosition + new Vector3(0f, -12f, -25f); // Target position down and back

        // Smooth rotation and movement
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time (0 to 1)
            hallway.transform.localRotation = Quaternion.Lerp(initialRotation, newRotation, t);
            hallway.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;

            // Destroy journals and manage flickering lights
            if (elapsedTime >= 0.3f && elapsedTime <= 0.5f)
            {
                foreach (var journal in Journals)
                {
                    Destroy(journal);
                }
            }
            if (elapsedTime > 0.2f && elapsedTime <= 0.6f) { SetLightsEnabled(AllLights, false); }
            else if (elapsedTime > 0.6f && elapsedTime <= 0.8f) { SetLightsEnabled(AllLights, true); AkSoundEngine.PostEvent("Flickering_Lights", gameObject); }
            else if (elapsedTime > 0.8f && elapsedTime <= 1.2f) { SetLightsEnabled(AllLights, false); }
            else if (elapsedTime > 1.2f && elapsedTime <= 1.3f) { SetLightsEnabled(AllLights, true); AkSoundEngine.PostEvent("Flickering_Lights", gameObject); }
            else if (elapsedTime > 1.3f && elapsedTime <= 1.6f) { SetLightsEnabled(AllLights, false); }
            else if (elapsedTime > 1.6f && elapsedTime <= 1.8f) { SetLightsEnabled(AllLights, true); AkSoundEngine.PostEvent("Flickering_Lights", gameObject); }
            else if (elapsedTime > 1.8f && elapsedTime <= 3.2f) { SetLightsEnabled(AllLights, false); }
            else if (elapsedTime > 3.2f && elapsedTime <= 3.4f) { SetLightsEnabled(AllLights, true); AkSoundEngine.PostEvent("Flickering_Lights", gameObject); }
            else if (elapsedTime > 3.4f && elapsedTime <= 3.9f) { SetLightsEnabled(AllLights, false); }
        }
    }

    private IEnumerator TurnToNormal(float duration)
    {
        Vector3 initialPosition = hallway.transform.localPosition; // Store initial position
        Vector3 targetPosition = initialPosition + new Vector3(0f, 12f, 25f); // Target position up and forward

        yield return new WaitForSeconds(duration / 2);

        hallway.transform.localRotation = Quaternion.Lerp(newRotation, initialRotation, 0.1f);
        hallway.transform.localPosition = Vector3.Lerp(hallway.transform.localPosition, targetPosition, 0.1f);

        AkSoundEngine.PostEvent("Light_OnOff_Event", gameObject);
        SetLightsEnabled(RedLights, false);
        exitSign.SetActive(false);

        yield return new WaitForSeconds(duration / 2);

        SetLightsEnabled(AllLights, true);
    }

    // Helper method to enable/disable lights in an array
    private void SetLightsEnabled(Light[] lights, bool enabled)
    {
        foreach (var light in lights)
        {
            light.enabled = enabled;
        }
    }
}
