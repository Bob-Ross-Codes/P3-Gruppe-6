using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public GameObject[] uiComponents; // Array of UI components to navigate through
    private int currentIndex = 0;

    void Start()
    {
        ShowCurrentComponent();
    }

    public void NextComponent()
    {
        currentIndex = (currentIndex + 1) % uiComponents.Length; // Cycle forward
        ShowCurrentComponent();
    }

    public void PreviousComponent()
    {
        currentIndex = (currentIndex - 1 + uiComponents.Length) % uiComponents.Length; // Cycle backward
        ShowCurrentComponent();
    }

    private void ShowCurrentComponent()
    {
        // Hide all components
        for (int i = 0; i < uiComponents.Length; i++)
        {
            uiComponents[i].SetActive(i == currentIndex); // Show only the current one
        }
    }
}
