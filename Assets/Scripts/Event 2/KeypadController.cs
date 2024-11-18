using UnityEngine;
using TMPro;
using Cinemachine;

public class KeypadController : MonoBehaviour
{
    public GameObject keypadUI; // Assign your keypad UI GameObject
    public TextMeshProUGUI inputDisplay; // TextMeshProUGUI to show the player’s input
    public GameObject door; // Door GameObject to open when correct code is entered
    public GameObject player; // Player GameObject with the movement script

    public CinemachineVirtualCamera mainCameraVCam; // Main gameplay camera
    public CinemachineVirtualCamera keypadVCam; // Keypad camera

    [System.Serializable]
    public class CodeWithPicture
    {
        public string code;              // Password for this picture
        public GameObject pictureObject; // GameObject for each picture
    }

    public CodeWithPicture[] codesWithPictures; // Array of codes and corresponding pictures

    public float interactionDistance = 2f; // Distance within which the player can interact with the keypad

    private string correctCode;         // Current correct code
    private string playerInput = "";    // Player's input
    private int currentCodeIndex = 0;   // Tracks the current picture/password index
    private bool isKeypadOpen = false;  // Tracks if keypad UI is open
    private MonoBehaviour FirstPersonController; // Reference to the player's movement script

    void Start()
    {
        keypadUI.SetActive(false); // Start with keypad UI hidden
        FirstPersonController = player.GetComponent<MonoBehaviour>();

        // Ensure the main camera is active at the start
        mainCameraVCam.Priority = 10;
        keypadVCam.Priority = 0;

        // Initialize with the first code and picture
        LoadCodeAndPicture(0);
    }

    void Update()
    {
        // Check if the player is within interaction distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Allow interaction only if the player is near the keypad
        if (distanceToPlayer <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadInputMode();
        }

        // Handle additional keypad functionality
        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha1)) // Look at password
        {
            LookAtPassword();
        }

        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha2)) // Look at keypad
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

        keypadUI.SetActive(isKeypadOpen); // Show or hide the keypad UI
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
        mainCameraVCam.Priority = 5; // Lower priority for main camera
        keypadVCam.Priority = 10;   // Higher priority for keypad camera
    }

    void SwitchToMainCamera()
    {
        mainCameraVCam.Priority = 10; // Higher priority for main camera
        keypadVCam.Priority = 0;     // Lower priority for keypad camera
    }

    void LoadCodeAndPicture(int index)
    {
        if (index >= 0 && index < codesWithPictures.Length)
        {
            currentCodeIndex = index;
            correctCode = codesWithPictures[index].code;

            // Set the correct picture active, and deactivate all others
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

    public void LookAtPassword()
    {
        // Advance to the next picture, if possible
        if (currentCodeIndex < codesWithPictures.Length - 1)
        {
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(false); // Hide current picture
            currentCodeIndex++;
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(true); // Show next picture
            Debug.Log("Looking at password: Changed to next picture");
        }
        else
        {
            Debug.Log("No more pictures to cycle through.");
        }
    }

    public void LookAtKeypad()
    {
        // Update the password to match the current picture
        correctCode = codesWithPictures[currentCodeIndex].code;
        Debug.Log("Looking at keypad: Correct code updated to " + correctCode);
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
}




/*using UnityEngine;
using TMPro;
using Cinemachine;

public class KeypadController : MonoBehaviour
{
    public GameObject keypadUI; // Assign your keypad UI GameObject
    public TextMeshProUGUI inputDisplay; // TextMeshProUGUI to show the player’s input
    public GameObject door; // Door GameObject to open when correct code is entered
    public GameObject player; // Player GameObject with the movement script

    public CinemachineVirtualCamera mainCameraVCam; // Main gameplay camera
    public CinemachineVirtualCamera keypadVCam; // Keypad camera

    [System.Serializable]
    public class CodeWithPicture
    {
        public string code;              // Password for this picture
        public GameObject pictureObject; // GameObject for each picture
    }

    public CodeWithPicture[] codesWithPictures; // Array of codes and corresponding pictures

    private string correctCode;         // Current correct code
    private string playerInput = "";    // Player's input
    private int currentCodeIndex = 0;   // Tracks the current picture/password index
    private bool isKeypadOpen = false;
    private bool isPlayerInRange = false; // Tracks if the player is near the keypad
    private MonoBehaviour FirstPersonController; // Reference to the player's movement script

    void Start()
    {
        keypadUI.SetActive(false); // Start with keypad UI hidden
        FirstPersonController = player.GetComponent<MonoBehaviour>();

        // Ensure the main camera is active at the start
        mainCameraVCam.Priority = 10;
        keypadVCam.Priority = 0;

        // Initialize with the first code and picture
        LoadCodeAndPicture(0);
    }

    void Update()
    {
        // Allow interaction only if the player is near the keypad
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadInputMode();
        }

        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha1)) // Look at password
        {
            LookAtPassword();
        }

        if (isKeypadOpen && Input.GetKeyDown(KeyCode.Alpha2)) // Look at keypad
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

        keypadUI.SetActive(isKeypadOpen); // Show or hide the keypad UI
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
        mainCameraVCam.Priority = 5; // Lower priority for main camera
        keypadVCam.Priority = 10;   // Higher priority for keypad camera
    }

    void SwitchToMainCamera()
    {
        mainCameraVCam.Priority = 10; // Higher priority for main camera
        keypadVCam.Priority = 0;     // Lower priority for keypad camera
    }

    void LoadCodeAndPicture(int index)
    {
        if (index >= 0 && index < codesWithPictures.Length)
        {
            currentCodeIndex = index;
            correctCode = codesWithPictures[index].code;

            // Set the correct picture active, and deactivate all others
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

    public void LookAtPassword()
    {
        // Advance to the next picture, if possible
        if (currentCodeIndex < codesWithPictures.Length - 1)
        {
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(false); // Hide current picture
            currentCodeIndex++;
            codesWithPictures[currentCodeIndex].pictureObject.SetActive(true); // Show next picture
            Debug.Log("Looking at password: Changed to next picture");
        }
        else
        {
            Debug.Log("No more pictures to cycle through.");
        }
    }

    public void LookAtKeypad()
    {
        // Update the password to match the current picture
        correctCode = codesWithPictures[currentCodeIndex].code;
        Debug.Log("Looking at keypad: Correct code updated to " + correctCode);
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

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

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
}*/
