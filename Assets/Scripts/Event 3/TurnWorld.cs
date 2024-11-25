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

    public GameObject Player;

    [SerializeField] private Transform player;
    [SerializeField] private Transform trigger;
    [SerializeField] private Transform trigger2;
    [SerializeField] private GameObject hallway;
    [SerializeField] private GameObject handLight;
    [SerializeField] private GameObject exitSign;
    [SerializeField] private GameObject exitSign2;
    [SerializeField] private GameObject spawnAgain;
    [SerializeField] private RunHallwayChanger hallwayChanger;
    public FirstPersonController playerController; // Reference to the FirstPersonController scriptCloset
    public LightManager lightManager;
    bool playingMonsterSound;


    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f);
    private Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);
    private Vector3 initialPosition; // Store initial position
    private Vector3 targetPosition; // Target position down and back
    private float interactionRange = 6f; // Set the interaction range
    private bool triggered;
    private bool triggeredToNormal; // Flag for TurnToNormal coroutine
    private bool handLightDead = false;
    private bool redLightsOn;
    private bool runningOnce = false;
    float initialPlayerSpeed;
    public GameObject MonsterSound;

    void Start()
    {
        initialPlayerSpeed = playerController.MoveSpeed;
        triggeredToNormal = false;
        hallway.transform.localRotation = initialRotation;
        exitSign.SetActive(false);
        exitSign2.SetActive(false);
        SetLightsEnabled(RedLights, false);
        playingMonsterSound = false;


        // Set the class-level initialPosition and targetPosition (not local variables)
        initialPosition = hallway.transform.localPosition; // Store initial position
        targetPosition = initialPosition + new Vector3(0f, -12f, -25f); // Target position down and back
    }

    private void FixedUpdate()
    {
        if (triggered == false)
            SetLightsEnabled(RedLights, false);

        float distanceToPlayer = Vector3.Distance(player.position, trigger.position);

        if (distanceToPlayer < interactionRange && !triggered)
        {
            StartCoroutine(TurnToRed(4f)); // Start TurnToRed coroutine
            Debug.Log("TurningRed");
            triggered = true;
        }

        if (redLightsOn && !playingMonsterSound)
        {
            if (distanceToPlayer > 5f && distanceToPlayer < 10f)
            {
                AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 0, MonsterSound);
                AkSoundEngine.PostEvent("Play_Monster_Sounds", MonsterSound);
                playingMonsterSound = true;
            }
            else if (distanceToPlayer > 10f)
            {
                AkSoundEngine.PostEvent("Stop_Monster_Sounds", MonsterSound);
                playingMonsterSound = false;
            }
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

            yield return null;

            if (runningOnce)
                lightManager.StartFlicker(duration, 0.6f, false);
            foreach (var journal in Journals)
            {
                Destroy(journal);
            }
            elapsedTime += Time.deltaTime;
            runningOnce = true;
        }
        SetLightsEnabled(AllLights, false);
        AkSoundEngine.SetRTPCValue("RTPC_LightState", 0, Player);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);

        // Ensure hallway finishes at the final target position and rotation
        hallway.transform.rotation = targetRotation;
        hallway.transform.localPosition = targetPosition;

        yield return new WaitForSeconds(duration / 2);
        SetLightsEnabled(AllLights, false);

        playerController.MoveSpeed = 0;
        StartCoroutine(TurnRedLights());


        while (handLightDead == true)
        {
            float darkInterval = Random.Range(0.1f, 0.4f);
            float lightInterval = Random.Range(0.05f, 0.1f);

            handLight.SetActive(true);
            yield return new WaitForSeconds(darkInterval);
            handLight.SetActive(false);
            yield return new WaitForSeconds(lightInterval);
        }
        handLight.SetActive(false);

        redLightsOn = true;

        SetLightsEnabled(AllLights, false);
    }

    private IEnumerator TurnRedLights()
    {

        yield return new WaitForSeconds(1f);
        handLightDead = true;

        // Assuming RedLights is a list or array of Light components
        for (int i = 0; i < RedLights.Length; i += 2)
        {
            // Turn on two lights at a time
            if (i < RedLights.Length)
            {
                RedLights[i].enabled = true;  // Turn on the first light
            }
            if (i + 1 < RedLights.Length)
            {
                RedLights[i + 1].enabled = true;  // Turn on the second light
            }

            // MARIUS: Spil stor lys tænder
            AkSoundEngine.SetRTPCValue("RTPC_LightState", 1, Player);
            AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);

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
        AkSoundEngine.PostEvent("Stop_Monster_Sounds", MonsterSound);

        exitSign2.SetActive(true);

        hallway.transform.localRotation = initialRotation;
        hallway.transform.localPosition = initialPosition;
        player.transform.position = spawnAgain.transform.position;

        yield return new WaitForSeconds(duration / 4);

        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);
        SetLightsEnabled(RedLights, false);
        exitSign.SetActive(false);
        hallwayChanger.StartChase();                                    //STARTERCHASEEE!!!! JAAAA! LIGE HER BÆTCH

        yield return new WaitForSeconds(duration / 2);

        SetLightsEnabled(AllLights, true);
        handLight.SetActive(true);
        exitSign.SetActive(false);

        yield return new WaitForSeconds(duration / 2);
    }

    private void SetLightsEnabled(Light[] lights, bool enabled)
    {
        foreach (var light in lights)
        {
            light.enabled = enabled;
        }
    }
}