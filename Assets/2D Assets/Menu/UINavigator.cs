/// <summary>
/// UINavigator allows navigation through a set of UI components.
/// </summary>
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    public GameObject[] uiComponents; // List of UI components to navigate
    private int currentIndex = 0; // Index of the currently active component

    private void Start()
    {
        ShowCurrentComponent(); // Show the initial UI component
    }

    /// <summary>
    /// Switches to the next UI component.
    /// </summary>
    public void NextComponent()
    {
        currentIndex = (currentIndex + 1) % uiComponents.Length; // Cycle forward
        ShowCurrentComponent();
    }

    /// <summary>
    /// Switches to the previous UI component.
    /// </summary>
    public void PreviousComponent()
    {
        currentIndex = (currentIndex - 1 + uiComponents.Length) % uiComponents.Length; // Cycle backward
        ShowCurrentComponent();
    }

    /// <summary>
    /// Displays only the currently active UI component.
    /// </summary>
    private void ShowCurrentComponent()
    {
        for (int i = 0; i < uiComponents.Length; i++)
        {
            uiComponents[i].SetActive(i == currentIndex); // Activate only the current component
        }
    }
}