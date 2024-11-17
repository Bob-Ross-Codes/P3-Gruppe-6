using UnityEngine;
using TMPro;
using Cinemachine;

public class KeypadController : MonoBehaviour
{
    public GameObject keypadUI; // Assign your keypad UI GameObject
    public TextMeshProUGUI[] digitDisplays; // Array of TextMeshProUGUI for each digit
    public TextMeshProUGUI inputDisplay; // TextMeshProUGUI to show the player’s input
    public GameObject door; // Door GameObject to open when correct code is entered
    public GameObject player; // Player GameObject with the movement script

    public CinemachineVirtualCamera mainCameraVCam; // Main gameplay camera
    public CinemachineVirtualCamera keypadVCam; // Keypad camera

    public string correctCode = "12345"; // Initial correct code
    private string playerInput = "";
    private char[] displayedCode;
    private int maxCodeChanges = 6;
    private int changesCount = 0;

    private bool isKeypadOpen = false;
    private bool isPlayerInRange = false; // Track if the player is within range of the keypad
    private MonoBehaviour FirstPersonController; // Reference to player movement script

    private KeypadCameraController keypadCameraController; // Reference to the keypad camera controller

    void Start()
    {
        keypadUI.SetActive(true); // Always visible
        displayedCode = correctCode.ToCharArray();
        UpdateDigitDisplays();

        Cursor.visible = false; // Hide cursor at the start
        Cursor.lockState = CursorLockMode.Locked;

        FirstPersonController = player.GetComponent<MonoBehaviour>();

        // Ensure the main camera is active at start
        mainCameraVCam.Priority = 10;
        keypadVCam.Priority = 0;

        // Get the keypad camera controller
        keypadCameraController = keypadVCam.GetComponent<KeypadCameraController>();
    }

    void Update()
    {
        // Only allow interaction if player is close to the keypad
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadInputMode();
        }

        // Test LookAtPassword and LookAtKeypad functionality
        if (Input.GetKeyDown(KeyCode.Alpha1) && isKeypadOpen)
        {
            LookAtPassword();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isKeypadOpen)
        {
            LookAtKeypad();
        }
    }

    void ToggleKeypadInputMode()
    {
        isKeypadOpen = !isKeypadOpen;

        if (isKeypadOpen)
        {
            EnableCursorAndLockPlayer();
            SwitchToKeypadCamera();
            ResetInput();
        }
        else
        {
            DisableCursorAndUnlockPlayer();
            SwitchToMainCamera();
        }
    }

    void EnableCursorAndLockPlayer()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (FirstPersonController != null)
        {
            FirstPersonController.enabled = false; // Lock player movement
        }
    }

    void DisableCursorAndUnlockPlayer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (FirstPersonController != null)
        {
            FirstPersonController.enabled = true; // Unlock player movement
        }
    }

    void SwitchToKeypadCamera()
    {
        mainCameraVCam.Priority = 5; // Lower priority for the main camera
        keypadVCam.Priority = 10; // Higher priority for the keypad camera
    }

    void SwitchToMainCamera()
    {
        mainCameraVCam.Priority = 10; // Higher priority for the main camera
        keypadVCam.Priority = 0; // Lower priority for the keypad camera
    }

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

    void CheckCode()
    {
        if (playerInput == correctCode)
        {
            OpenDoor();
            ToggleKeypadInputMode(); // Exit input mode after entering the correct code
        }
        else
        {
            ResetInput(); // Clear input if code is incorrect
        }
    }

    void ResetInput()
    {
        playerInput = "";
        inputDisplay.text = playerInput;
    }

    void OpenDoor()
    {
        Debug.Log("Door is opened!");
        door.SetActive(false); // Disable or open the door
    }

    public void LookAtPassword()
    {
        if (changesCount < maxCodeChanges)
        {
            ChangeDisplayedCode();
            changesCount++;
        }
    }

    public void LookAtKeypad()
    {
        correctCode = new string(displayedCode); // Update correctCode to the last displayedCode
        UpdateDigitDisplays();
    }

    void ChangeDisplayedCode()
    {
        // Select a random index to change
        int randomIndex = Random.Range(0, displayedCode.Length);

        // Ensure the new digit is different from the original correct digit
        char originalDigit = displayedCode[randomIndex];
        char newDigit;
        do
        {
            newDigit = (char)Random.Range(48, 58); // ASCII for '0' to '9'
        } while (newDigit == originalDigit);

        // Replace the selected digit with the new random digit
        displayedCode[randomIndex] = newDigit;

        // Update the displayed code
        UpdateDigitDisplays();
    }

    void UpdateDigitDisplays()
    {
        for (int i = 0; i < digitDisplays.Length; i++)
        {
            if (i < displayedCode.Length)
            {
                digitDisplays[i].text = displayedCode[i].ToString();
                digitDisplays[i].gameObject.SetActive(true); // Ensure digit is visible
            }
            else
            {
                digitDisplays[i].gameObject.SetActive(false); // Hide extra digits
            }
        }
    }

    public void SetHoveringOverKeypad(bool isHovering)
    {
        if (keypadCameraController != null)
        {
            keypadCameraController.SetHoveringOverKeypad(isHovering);
        }
    }

    // Detect when player enters keypad range
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

    // Detect when player exits keypad range
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            if (isKeypadOpen)
            {
                ToggleKeypadInputMode(); // Automatically exit input mode when player leaves range
            }
        }
    }
}
