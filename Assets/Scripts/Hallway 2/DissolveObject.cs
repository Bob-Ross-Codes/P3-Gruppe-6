using UnityEngine;

/// <summary>
/// Handles the dissolve effect for an object and its children using a dissolve shader material.
/// </summary>
public class DissolveObject : MonoBehaviour
{
    [Header("Dissolve Settings")]
    [SerializeField] private Material dissolveMaterial; // The dissolve shader material
    [SerializeField] private float dissolveSpeed = 0.1f; // Speed of the dissolve animation

    private Renderer[] renderers; // Array of all renderers in the object
    private Material[] dissolveMaterials; // Unique dissolve materials for each renderer
    private bool isDissolving = false; // Tracks whether the object is dissolving
    private float _CutoffHeight = 10f; // The cutoff height for the dissolve effect

    /// <summary>
    /// Initializes dissolve materials for all renderers in the object and its children.
    /// </summary>
    private void Start()
    {
        // Get all renderers in this object and its children
        renderers = GetComponentsInChildren<Renderer>();
        dissolveMaterials = new Material[renderers.Length];

        // Create unique dissolve materials for each renderer
        for (int i = 0; i < renderers.Length; i++)
        {
            // Clone the dissolve material
            dissolveMaterials[i] = new Material(dissolveMaterial);

            // Transfer textures from the original material
            Material originalMaterial = renderers[i].material;
            if (originalMaterial.HasProperty("_MainTex"))
            {
                dissolveMaterials[i].SetTexture("_MainTex", originalMaterial.GetTexture("_MainTex"));
            }

            // Assign the new dissolve material to the renderer
            renderers[i].material = dissolveMaterials[i];
        }
    }

    /// <summary>
    /// Updates the dissolve effect over time if it is active.
    /// </summary>
    private void Update()
    {
        if (isDissolving)
        {
            // Increment the cutoff height for the dissolve effect
            _CutoffHeight += Time.deltaTime * dissolveSpeed;

            // Update the dissolve material property
            foreach (var mat in dissolveMaterials)
            {
                mat.SetFloat("_CutoffHeight", _CutoffHeight);
            }

            // Stop dissolving when the effect is complete
            if (_CutoffHeight >= 1.0f)
            {
                isDissolving = false;
                Debug.Log("Dissolve complete.");
            }
        }
    }

    /// <summary>
    /// Starts the dissolve effect, resetting the dissolve state and cutoff height.
    /// </summary>
    public void StartDissolve()
    {
        Debug.Log("Dissolve started.");
        isDissolving = true;
        _CutoffHeight = 0f; // Reset dissolve
    }
}