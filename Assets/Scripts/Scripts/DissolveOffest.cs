using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the dissolve effect on a material by gradually changing its "_Dissolve" property.
/// Allows activation, customization of target dissolve values, and resetting to a default state.
/// </summary>
public class DissolveOffset : MonoBehaviour
{
    [Header("Dissolve Settings")]
    [Tooltip("The default dissolve value used when resetting or initializing.")]
    [SerializeField] private float defaultDissolveValue;

    [Tooltip("The target dissolve value the object should reach when activated.")]
    [SerializeField] private float goalDissolveValue;

    [Tooltip("The speed at which the dissolve value moves toward the target.")]
    [SerializeField] private float dissolveSpeed;

    private Material material;           // The object's material instance
    private bool isDissolving = false;   // Tracks whether the object is currently dissolving
    private float currentDissolveValue;  // Tracks the current dissolve value of the material

    /// <summary>
    /// Initializes the material and sets the initial dissolve value to the default state.
    /// Logs an error if no suitable material is found.
    /// </summary>
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Ensures a unique material instance
        }

        if (material == null)
        {
            Debug.LogError($"No material found on {gameObject.name}!");
            return;
        }

        currentDissolveValue = defaultDissolveValue;
        material.SetFloat("_Dissolve", currentDissolveValue);
    }

    /// <summary>
    /// Activates the dissolve effect towards the predetermined goalDissolveValue.
    /// Starts a coroutine if not already dissolving.
    /// </summary>
    public void ActivateDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(goalDissolveValue));
        }
    }

    /// <summary>
    /// Activates the dissolve effect towards a specified target dissolve value.
    /// Useful if you need a custom dissolve end state.
    /// </summary>
    /// <param name="targetDissolveValue">The desired final dissolve value.</param>
    public void ActivateDissolve(float targetDissolveValue)
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name} towards goal: {targetDissolveValue}");
            StartCoroutine(DissolveCoroutine(targetDissolveValue));
        }
    }

    /// <summary>
    /// Resets the dissolve effect back to the default state.
    /// Only activates if the object is not already dissolving.
    /// </summary>
    public void ResetDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve reset for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(defaultDissolveValue));
        }
    }

    /// <summary>
    /// Coroutine that smoothly transitions the dissolve value from the current level to the target level.
    /// Updates the material's "_Dissolve" parameter each frame until the target is reached.
    /// </summary>
    /// <param name="targetValue">The target dissolve value to reach.</param>
    private IEnumerator DissolveCoroutine(float targetValue)
    {
        isDissolving = true;

        while (!Mathf.Approximately(currentDissolveValue, targetValue))
        {
            currentDissolveValue = Mathf.MoveTowards(currentDissolveValue, targetValue, Time.deltaTime * dissolveSpeed);
            material.SetFloat("_Dissolve", currentDissolveValue);
            yield return null;
        }

        Debug.Log($"Dissolve completed for {gameObject.name}, reached: {currentDissolveValue}");
        isDissolving = false;
    }
}