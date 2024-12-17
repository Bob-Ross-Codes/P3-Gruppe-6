using System.Collections;
using UnityEngine;
using Cinemachine;
using StarterAssets;

/// <summary>
/// Handles hiding mechanics in a closet and triggers related game events.
/// </summary>
public class ClosetHide : MonoBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private CinemachineVirtualCamera mainCamera; // Main gameplay camera
    [SerializeField] private CinemachineVirtualCamera closetCamera; // Closet view camera

    [Header("Player and Monster Settings")]
    public FirstPersonController playerController; // Reference to the player's movement controller
    public GameObject monsterPrefab; // Monster GameObject
    [SerializeField] private Transform player; // Player's position
    [SerializeField] private Transform roomDoor; // Reference to the room door
    public float interactionRange = 2.0f; // Distance to interact with the closet

    [Header("Closet Interaction")]
    public Animator leftDoorAnimator; // Animator for the left closet door
    public Animator rightDoorAnimator; // Animator for the right closet door
    public Animator doorHingeAnimator; // Animator for the closet door hinge
    [SerializeField] private GameObject objectToDisableHallwayOne; // Object to disable when hiding
    public LightManager lightManager; // Controls the light flickering
    public GameObject RoomLight; // Light in the room
    public MonsterSequenceController sequenceController; // Handles monster sequences

    [Header("Gaze and Blink Settings")]
    [SerializeField] private Gaze gaze; // Gaze tracking for blinking
    [SerializeField] private float blinkTimer = 3f; // Time to trigger monster destruction on blinking
    private float blinkTime = 0f; // Tracks how long the player is blinking

    [Header("Hiding Logic")]
    public bool isHiding = false; // Is the player currently hiding?
    public bool canToggleHiding = true; // Can the player toggle hiding?
    private bool hasEnteredClosetOnce = false; // Tracks if the player has hidden at least once

    /// <summary>
    /// Updates player interaction with the closet and toggles hiding.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canToggleHiding && IsNearCloset())
        {
            ToggleHiding();

            if (!hasEnteredClosetOnce)
            {
                hasEnteredClosetOnce = true;
                canToggleHiding = false;
            }
        }
    }

    /// <summary>
    /// Flickers the light in the room before turning it off.
    /// </summary>
    /// <param name="duration">Duration of the flickering.</param>
    /// <param name="speed">Speed of the flickering.</param>
    /// <param name="handlight">Should the handlight flicker?</param>
    private IEnumerator FlickeringLight(float duration, float speed, bool handlight)
    {
        yield return new WaitForSeconds(1);
        lightManager.StartFlicker(duration, speed, handlight);
        yield return new WaitForSeconds(duration);
        Destroy(RoomLight);
    }

    /// <summary>
    /// Toggles the player's hiding state and updates related mechanics.
    /// </summary>
    private void ToggleHiding()
    {
        isHiding = !isHiding;

        if (isHiding)
        {
            EnterCloset();
        }
        else
        {
            ExitCloset();
        }
    }

    /// <summary>
    /// Handles logic for entering the closet.
    /// </summary>
    private void EnterCloset()
    {
        StartCoroutine(FlickeringLight(2f, 0.8f, false));

        mainCamera.Priority = 0;
        closetCamera.Priority = 10;
        playerController.MoveSpeed = 0;

        leftDoorAnimator.SetTrigger("ToggleDoor");
        rightDoorAnimator.SetTrigger("ToggleDoor");
        doorHingeAnimator.SetTrigger("broken");

        objectToDisableHallwayOne.SetActive(false);

        AkSoundEngine.PostEvent("Play_DoorSlamOpen", gameObject);
        sequenceController.OnPlayerHidden();

        AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 0, monsterPrefab);
        AkSoundEngine.PostEvent("Play_Monster_Sounds", monsterPrefab);

        StartCoroutine(WaitForMonsterToBeDestroyed());
    }

    /// <summary>
    /// Handles logic for exiting the closet.
    /// </summary>
    private void ExitCloset()
    {
        player.LookAt(roomDoor);

        mainCamera.Priority = 10;
        closetCamera.Priority = 0;
        playerController.MoveSpeed = 3.5f;

        leftDoorAnimator.SetTrigger("ToggleDoor");
        rightDoorAnimator.SetTrigger("ToggleDoor");
    }

    /// <summary>
    /// Checks if the player is close enough to interact with the closet.
    /// </summary>
    /// <returns>True if the player is within interaction range, otherwise false.</returns>
    private bool IsNearCloset()
    {
        return Vector3.Distance(player.position, transform.position) <= interactionRange;
    }

    /// <summary>
    /// Waits for the player to blink long enough to destroy the monster.
    /// </summary>
    public IEnumerator WaitForMonsterToBeDestroyed()
    {
        Debug.Log("Hiding locked. Waiting for monster to be destroyed.");

        while (true)
        {
            if (gaze._blinking)
            {
                blinkTime += Time.deltaTime;

                if (blinkTime >= blinkTimer)
                {
                    DestroyMonster();
                    yield break;
                }
            }
            else
            {
                blinkTime = 0f;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Destroys the monster and unlocks hiding mechanics.
    /// </summary>
    private void DestroyMonster()
    {
        Debug.Log("Monster destroyed. Hiding unlocked.");
        AkSoundEngine.PostEvent("Stop_Monster_Sounds", monsterPrefab);
        Destroy(monsterPrefab);
        canToggleHiding = true;
    }
}