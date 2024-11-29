using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public class DissolveOffest : MonoBehaviour

{
    [Header("Dissolve Settings")]
    [SerializeField] private float defaultDissolveValue; // Default dissolve state for this hallway
    [SerializeField] private float goalDissolveValue;    // Target dissolve state
    [SerializeField] private float dissolveSpeed;         // Speed of dissolve

    private Material material;
    private bool isDissolving = false; // Flag to track if dissolve is active
    private float currentDissolveValue; // Tracks the current dissolve value

    void Start()
    {
        // Get the material and initialize dissolve value
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Ensure unique instance of material
        }

        if (material == null)
        {
            Debug.LogError($"No material found on {gameObject.name}!");
            return;
        }

        // Initialize the dissolve value to the default state
        currentDissolveValue = defaultDissolveValue;
        material.SetFloat("_Dissolve", currentDissolveValue);
    }

    /// <summary>
    /// Activates the dissolve effect, moving towards the specified goal.
    /// </summary>
    public void ActivateDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(goalDissolveValue));
        }
        else
        {
            Debug.LogWarning("Dissolve is already active!");
        }
    }

    /// <summary>
    /// Activates the dissolve effect, specifying a custom goal dissolve value.
    /// </summary>
    /// <param name="targetDissolveValue">The desired end dissolve value.</param>
    public void ActivateDissolve(float targetDissolveValue)
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name} towards goal: {targetDissolveValue}");
            StartCoroutine(DissolveCoroutine(targetDissolveValue));
        }
        else
        {
            Debug.LogWarning("Dissolve is already active!");
        }
    }

    /// <summary>
    /// Coroutine to handle the dissolve effect.
    /// </summary>
    /// <param name="targetValue">The target dissolve value to reach.</param>
    private IEnumerator DissolveCoroutine(float targetValue)
    {
        isDissolving = true;

        while (!Mathf.Approximately(currentDissolveValue, targetValue))
        {
            // Lerp towards the target dissolve value
            currentDissolveValue = Mathf.MoveTowards(currentDissolveValue, targetValue, Time.deltaTime * dissolveSpeed);
            material.SetFloat("_Dissolve", currentDissolveValue);
            yield return null;
        }

        Debug.Log($"Dissolve completed for {gameObject.name}, reached: {currentDissolveValue}");
        isDissolving = false;
    }

    /// <summary>
    /// Resets the dissolve effect to the default state.
    /// </summary>
    public void ResetDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve reset for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(defaultDissolveValue));
        }
    }
}

