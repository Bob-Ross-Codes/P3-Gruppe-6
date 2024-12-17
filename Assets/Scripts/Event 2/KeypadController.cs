using UnityEngine;
using TMPro;
using Cinemachine;
using StarterAssets;

/// <summary>
/// Controls the keypad functionality, including player interaction, code input, and door unlocking.
/// Manages cameras, UI, and gameplay interaction logic.
/// </summary>
public class KeypadController : MonoBehaviour
{
    [Header("UI and Gameplay References")]
    [Tooltip("The keypad UI GameObject.")]
    public GameObject keypadUI;

    [Tooltip("The TextMeshProUGUI element to display the player's input.")]
    public TextMeshProUGUI inputDisplay;

    [Tooltip("The door GameObject to open when the correct code is entered.")]
    public GameObject door;

    [Tooltip("The player GameObject.")]
    public GameObject player;

    [Tooltip("The collider to enable/disable during keypad interaction.")]
    public GameObject playCollider;

    [Header("Camera References")]
    [Tooltip("The main gameplay camera.")]
    public CinemachineVirtualCamera mainCameraVCam;

    [Tooltip("The keypad interaction camera.")]
    public CinemachineVirtualCamera keypadVCam;

    [Header("Code and Picture Settings")]
    [Tooltip("Array of codes and associated pictures.")]
    public CodeWithPicture[] codesWithPictures;

    [Tooltip("Interaction distance for the keypad.")]
    public float interactionDistance = 4f;

    [Header("Player Movement Settings")]
    [Tooltip("Reference to the player's movement controller.")]
    public FirstPersonController playerController;

    [System.Serializable]
    public class CodeWithPicture
    {
        [Tooltip("The password associated with this picture.")]
        public string code;

        [Tooltip("The GameObject for the picture.")]
        public GameObject pictureObject;
    }

    private string correctCode; // The currently active correct code
    private string playerInput = ""; // Tracks the player's input
    private int currentCodeIndex = 0; // The index of the current code and picture
    private bool isKeypadOpen = false; // Whether the keypad UI is currently open

    /// <summary>
    /// Initializes the keypad UI, cameras, and gameplay elements.
    /// </summary>
    void Start()
    {
        keypadUI.SetActive(false);
        playCollider.SetActive(true);
        mainCameraVCam.Priority = 10;
        keypadVCam.Priority = 0;

        LoadCodeAndPicture(0); // Load the first code and picture
    }

    /// <summary>
    /// Monitors player interaction with the keypad and handles input logic.
    /// </summary>
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadInputMode();
        }

        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha1))
        {
            LookAtPassword();
        }

        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha2))
        {
            LookAtKeypad();
        }
    }

    /// <summary>
    /// Toggles the keypad input mode, managing the UI, cameras, and player controls.
    /// </summary>
    void ToggleKeypadInputMode()
    {
        isKeypadOpen = !isKeypadOpen;

        if (isKeypadOpen)
        {
            EnableCursorAndLockPlayer();
            SwitchToKeypadCamera();
            ResetInput();
            playCollider.SetActive(false);
        }
        else
        {
            DisableCursorAndUnlockPlayer();
            SwitchToMainCamera();
            playCollider.SetActive(true);
        }

        keypadUI.SetActive(isKeypadOpen);
    }

    /// <summary>
    /// Enables the cursor and locks player movement.
    /// </summary>
    void EnableCursorAndLockPlayer()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerController != null)
        {
            playerController.MoveSpeed = 0;
        }
    }

    /// <summary>
    /// Disables the cursor and unlocks player movement.
    /// </summary>
    void DisableCursorAndUnlockPlayer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerController != null)
        {
            playerController.MoveSpeed = 3.5f;
        }
    }

    /// <summary>
    /// Switches to the keypad camera.
    /// </summary>
    void SwitchToKeypadCamera()
    {
        mainCameraVCam.Priority = 5;
        keypadVCam.Priority = 10;
    }

    /// <summary>
    /// Switches to the main gameplay camera.
    /// </summary>
    void SwitchToMainCamera()
    {
        mainCameraVCam.Priority = 10;
        keypadVCam.Priority = 0;
    }

    /// <summary>
    /// Loads a specific code and associated picture.
    /// </summary>
    /// <param name="index">The index of the code and picture to load.</param>
    void LoadCodeAndPicture(int index)
    {
        if (index >= 0 && index < codesWithPictures.Length)
        {
            currentCodeIndex = index;
            correctCode = codesWithPictures[index].code;

            for (int i = 0; i < codesWithPictures.Length; i++)
            {
                codesWithPictures[i].pictureObject.SetActive(i == currentCodeIndex);
            }
        }
        else
        {
            Debug.LogError("Invalid code index!");
        }
    }

    /// <summary>
    /// Cycles to the next picture in the sequence.
    /// </summary>
    public void LookAtKeypad()
    {
        if (currentCodeIndex < codesWithPictures.Length - 1)
        {
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(false);
            currentCodeIndex++;
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(true);
        }
        else
        {
            Debug.Log("No more pictures to cycle through.");
        }
    }

    /// <summary>
    /// Updates the current code to match the active picture.
    /// </summary>
    public void LookAtPassword()
    {
        correctCode = codesWithPictures[currentCodeIndex].code;
    }

    /// <summary>
    /// Adds a digit to the player's input and checks the code when input is complete.
    /// </summary>
    /// <param name="digit">The digit to add to the input.</param>
    public void AddDigit(string digit)
    {
        if (playerInput.Length < correctCode.Length)
        {
            playerInput += digit;
            inputDisplay.text = playerInput;

            if (playerInput.Length == correctCode.Length)
            {
                CheckCode();
            }
        }
    }

    /// <summary>
    /// Clears the player's input.
    /// </summary>
    public void ClearInput()
    {
        ResetInput();
    }

    /// <summary>
    /// Checks if the player's input matches the correct code.
    /// </summary>
    void CheckCode()
    {
        if (playerInput == correctCode)
        {
            OpenDoor();
            ToggleKeypadInputMode();
        }
        else
        {
            ResetInput();
        }
    }

    /// <summary>
    /// Resets the player's input to an empty string.
    /// </summary>
    void ResetInput()
    {
        playerInput = "";
        inputDisplay.text = playerInput;
    }

    /// <summary>
    /// Opens the door when the correct code is entered.
    /// </summary>
    void OpenDoor()
    {
        door.SetActive(false);
    }
}