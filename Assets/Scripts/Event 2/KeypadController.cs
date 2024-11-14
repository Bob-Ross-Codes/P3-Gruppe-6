using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    public GameObject keypadUI; // Assign your keypad UI GameObject
    public TextMeshProUGUI codeDisplay; // TextMeshProUGUI to show the current code on the keypad
    public TextMeshProUGUI inputDisplay; // TextMeshProUGUI to show the player’s input
    public GameObject door; // Door GameObject to open when correct code is entered
    public GameObject player; // Player GameObject with the movement script
    public string correctCode = "12345"; // Initial correct code
    private string playerInput = "";
    private string displayedCode;
    private int maxCodeChanges = 6;
    private int changesCount = 0;

    private bool isKeypadOpen = false;
    private bool isPlayerInRange = false; // Track if the player is within range of the keypad
    private MonoBehaviour FirstPersonController; // Reference to player movement script

    void Start()
    {
        keypadUI.SetActive(true); // Always visible
        displayedCode = correctCode;
        codeDisplay.text = displayedCode;
        
        Cursor.visible = false; // Hide cursor at the start
        Cursor.lockState = CursorLockMode.Locked;

        FirstPersonController = player.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        // Only allow interaction if player is close to the keypad
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadInputMode();
        }

        // Test LookAway functionality
        if (Input.GetKeyDown(KeyCode.Alpha1) && isKeypadOpen)
        {
            LookAway();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isKeypadOpen)
        {
            LookBack();
        }
    }

    void ToggleKeypadInputMode()
    {
        isKeypadOpen = !isKeypadOpen;

        if (isKeypadOpen)
        {
            EnableCursorAndLockPlayer();
            ResetInput();
        }
        else
        {
            DisableCursorAndUnlockPlayer();
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

    public void LookAway()
    {
        if (changesCount < maxCodeChanges)
        {
            ChangeDisplayedCode();
            changesCount++;
        }
    }

    public void LookBack()
    {
        correctCode = displayedCode; // Update correctCode to the last displayedCode
        codeDisplay.text = correctCode;
    }

    void ChangeDisplayedCode()
    {
        // Copy correctCode to a character array for modification
        char[] newCodeArray = displayedCode.ToCharArray();

        // Select a random index to change
        int randomIndex = Random.Range(0, newCodeArray.Length);

        // Ensure the new digit is different from the original correct digit
        char originalDigit = displayedCode[randomIndex];
        char newDigit;
        do
        {
            newDigit = (char)Random.Range(48, 58); // ASCII for '0' to '9'
        } while (newDigit == originalDigit);

        // Replace the selected digit with the new random digit
        newCodeArray[randomIndex] = newDigit;
        displayedCode = new string(newCodeArray);

        // Update the displayed code
        codeDisplay.text = displayedCode;
    }

    void ResetCode()
    {
        displayedCode = correctCode;
        codeDisplay.text = displayedCode;
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
