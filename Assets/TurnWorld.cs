using System.Collections;
using UnityEngine;
using StarterAssets;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

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
    [SerializeField] private GameObject exitSign2;
    [SerializeField] private GameObject spawnAgain;
    [SerializeField] private GameObject monster;
    public FirstPersonController playerController; // Reference to the FirstPersonController scriptCloset


    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f);
    private Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);
    private Vector3 initialPosition; // Store initial position
    private Vector3 targetPosition; // Target position down and back
    private float interactionRange = 6f; // Set the interaction range
    private bool triggered;
    private bool triggeredToNormal; // Flag for TurnToNormal coroutine
    private bool handLightDead = false;
    float initialPlayerSpeed;

void Start()
{
    initialPlayerSpeed = playerController.MoveSpeed;
    triggeredToNormal = false;
    hallway.transform.localRotation = initialRotation;
    exitSign.SetActive(false);
    exitSign2.SetActive(false);
    SetLightsEnabled(RedLights, false);
    SetLightsEnabled(AllLights, true);
    monster.SetActive(false);

    // Set the class-level initialPosition and targetPosition (not local variables)
    initialPosition = hallway.transform.localPosition; // Store initial position
    targetPosition = initialPosition + new Vector3(0f, -12f, -25f); // Target position down and back
}


    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.position, trigger.position);

        if (distanceToPlayer < interactionRange && !triggered)
        {
            StartCoroutine(TurnToRed(4f)); // Start TurnToRed coroutine
            Debug.Log("TurningRed");
            triggered = true;
        }

        float distanceToPlayer2 = Vector3.Distance(player.position, trigger2.position);

        if (distanceToPlayer2 < interactionRange && !triggeredToNormal)
        {
            Debug.Log("TurningToNormal");
            StartCoroutine(TurnToNormal(2f)); // Start TurnToNormal coroutine
            triggeredToNormal = true;
        }
    }

    private IEnumerator TurnToRed(float duration)
    {
        float elapsedTime = 0f;

        // Smooth rotation and movement over time
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time (0 to 1)

            // Smoothly interpolate the rotation and position
            hallway.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            hallway.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;

            // Destroy journals and manage flickering lights during the transition
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

        // Ensure hallway finishes at the final target position and rotation
        hallway.transform.rotation = targetRotation;
        hallway.transform.localPosition = targetPosition;

        yield return new WaitForSeconds(duration/2);

        playerController.MoveSpeed = 0;
        StartCoroutine(TurnRedLights());


        while (handLightDead == false)  
        {
            float darkInterval = Random.Range(0.1f, 0.4f);
            float lightInterval = Random.Range(0.05f, 0.1f);

            handLight.SetActive(true);
            yield return new WaitForSeconds(darkInterval);
            handLight.SetActive(false);
            yield return new WaitForSeconds(lightInterval);
        }
        Destroy(handLight);
        //MARIUS: En eller anden lyd fordi lommelygten går ud. Evt. bare flickering Lights
    }

    private IEnumerator TurnRedLights (){
        
        yield return new WaitForSeconds(1f);
        handLightDead = true;

        //MARIUS: Kan vi starte en masse stemmer her. Vi Slukker dem senere

        // Assuming RedLights is a list or array of Light components
        for (int i = 0; i < RedLights.Length; i += 2) {
            // Turn on two lights at a time
            if (i < RedLights.Length) {
                RedLights[i].enabled = true;  // Turn on the first light
            }
            if (i + 1 < RedLights.Length) {
                RedLights[i + 1].enabled = true;  // Turn on the second light
            }

            // MARIUS: Spil stor lys tænder
            AkSoundEngine.PostEvent("Light_OnOff_Event", gameObject);

            // Wait for 1 second before moving to the next pair
            yield return new WaitForSeconds(1f);
        }
        playerController.MoveSpeed = initialPlayerSpeed;
        exitSign.SetActive(true);
        //MARIUS: Måske et lille 'tick'
    }


    private IEnumerator TurnToNormal(float duration)
    {
        Debug.Log("TurningToNormal");
        //MARIUS: Sluk alle stemmer
        yield return new WaitForSeconds(duration / 4);

        exitSign2.SetActive(true);

        hallway.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        hallway.transform.localPosition = initialPosition;

        yield return new WaitForSeconds(duration / 4);

        player.transform.position = spawnAgain.transform.position;

        AkSoundEngine.PostEvent("Light_OnOff_Event", gameObject);
        SetLightsEnabled(RedLights, false);
        exitSign.SetActive(false);

        player.transform.position = spawnAgain.transform.position;

        yield return new WaitForSeconds(duration / 2);
       
        player.transform.position = spawnAgain.transform.position;

        SetLightsEnabled(AllLights, true);

        yield return new WaitForSeconds(duration / 2);

        monster.SetActive(false);
    }

    private void SetLightsEnabled(Light[] lights, bool enabled)
    {
        foreach (var light in lights)
        {
            light.enabled = enabled;
        }
    }
}