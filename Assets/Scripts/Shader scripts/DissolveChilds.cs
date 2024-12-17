using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the dissolve effect for all child objects with materials supporting the dissolve shader.
/// </summary>
///  Copilot was used to refine this script
public class DissolveChilds : MonoBehaviour
{
    [Header("Dissolve Settings")]
    private List<Material> materials = new List<Material>(); // List of materials for dissolve effect

    /// <summary>
    /// Initializes the list of materials from all child objects.
    /// </summary>
    private void Start()
    {
        // Get all renderers from child objects
        var renderers = GetComponentsInChildren<Renderer>();

        // Collect materials from each renderer
        foreach (var renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }
    }

    /// <summary>
    /// Updates the dissolve value dynamically over time.
    /// </summary>
    private void Update()
    {
        // Calculate a dissolve value oscillating between 0 and 1 based on time
        float dissolveValue = Mathf.PingPong(Time.time * 0.5f, 1f);
        SetValue(dissolveValue);
    }

    /// <summary>
    /// Sets the dissolve value for all materials with a "_Dissolve" property.
    /// </summary>
    /// <param name="value">The dissolve value to set (0 to 1).</param>
    public void SetValue(float value)
    {
        foreach (var material in materials)
        {
            // Check if the material supports the "_Dissolve" property
            if (material.HasProperty("_Dissolve"))
            {
                material.SetFloat("_Dissolve", value); // Update dissolve value
            }
        }
    }
}