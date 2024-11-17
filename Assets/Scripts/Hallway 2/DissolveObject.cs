using UnityEngine;

public class DissolveObject : MonoBehaviour
/*{
    public Material dissolveMaterial; // Assign the dissolve material in the Inspector
    public float _CutoffHeight = 0f;      // Controls height for dissolve
    public float _NoiseScale = 1f;       // Controls the size of the noise
    public float _NoiseStrength = 0.5f;  // Controls the intensity of the noise
    public float dissolveSpeed = 0.1f;   // Speed of the dissolve effect

    private Renderer[] renderers;
    private bool isDissolving = false;

    void Start()
    {
        // Get all child Renderers
        renderers = GetComponentsInChildren<Renderer>();

        // Assign the dissolve material to all child Renderers
        foreach (Renderer renderer in renderers)
        {
            renderer.material = dissolveMaterial;
        }
    }

    void Update()
    {
        if (isDissolving)
        {
            // Incrementally dissolve
            _CutoffHeight += Time.deltaTime * dissolveSpeed;

            // Update the global shader properties
            Shader.SetGlobalFloat("_CutoffHeight", _CutoffHeight);
            Shader.SetGlobalFloat("_NoiseScale", _NoiseScale);
            Shader.SetGlobalFloat("_NoiseStrength", _NoiseStrength);

            // Stop when fully dissolved
            if (_CutoffHeight >= 1.0f)
            {
                isDissolving = false;
                Debug.Log("Dissolve complete.");
            }
        }
    }

    public void StartDissolve()
    {
        Debug.Log("Dissolve started.");
        isDissolving = true;
        _CutoffHeight = 0f; // Reset dissolve effect
    }
}*/

/*{
    public Material dissolveMaterial; // Assign the dissolve material in the Inspector
    public float _CutoffHeight = 0f;  // Controls height for dissolve
    public float _NoiseScale = 1f;   // Controls the size of the noise
    public float _NoiseStrength = 0.5f;  // Controls the intensity of the noise
    public float dissolveSpeed = 0.1f;   // Speed of the dissolve effect

    private Renderer[] renderers;
    private bool isDissolving = false;

    void Start()
    {
        // Get all child Renderers and assign the material
        renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = new Material(dissolveMaterial); // Clone the material for local changes
        }
    }

    void Update()
    {
        if (isDissolving)
        {
            _CutoffHeight += Time.deltaTime * dissolveSpeed;

            // Update each material's properties locally
            foreach (Renderer renderer in renderers)
            {
                Material material = renderer.material;
                material.SetFloat("_CutoffHeight", _CutoffHeight);
                material.SetFloat("_NoiseScale", _NoiseScale);
                material.SetFloat("_NoiseStrength", _NoiseStrength);
            }

            // Stop when fully dissolved
            if (_CutoffHeight >= 1.0f)
            {
                isDissolving = false;
                Debug.Log("Dissolve complete.");
            }
        }
    }

    public void StartDissolve()
    {
        Debug.Log("Dissolve started.");
        isDissolving = true;
        _CutoffHeight = 0f; // Reset dissolve effect
    }
}*/

{
    public Material dissolveMaterial; // The dissolve shader material
    public float dissolveSpeed = 0.1f; // Speed of dissolve animation

    private Renderer[] renderers; // Array of all renderers in the object
    private Material[] dissolveMaterials; // Unique dissolve materials for each renderer
    private bool isDissolving = false;
    private float _CutoffHeight = 0f;

    void Start()
    {
        // Get all renderers in this object and its children
        renderers = GetComponentsInChildren<Renderer>();
        dissolveMaterials = new Material[renderers.Length];

        // Create unique dissolve materials for each renderer
        for (int i = 0; i < renderers.Length; i++)
        {
            // Get the original material of the renderer
            Material originalMaterial = renderers[i].material;

            // Create a new instance of the dissolve material
            dissolveMaterials[i] = new Material(dissolveMaterial);

            // Assign the original material's texture to the dissolve material's BaseTexture property
            if (originalMaterial.HasProperty("_BaseColorMap")) // For HDRP
            {
                dissolveMaterials[i].SetTexture("_BaseTexture", originalMaterial.GetTexture("_BaseColorMap"));
            }
            else if (originalMaterial.HasProperty("_BaseMap")) // Fallback for some HDRP/URP materials
            {
                dissolveMaterials[i].SetTexture("_BaseTexture", originalMaterial.GetTexture("_BaseMap"));
            }
            else if (originalMaterial.HasProperty("_MainTex")) // Standard Shader
            {
                dissolveMaterials[i].SetTexture("_BaseTexture", originalMaterial.GetTexture("_MainTex"));
            }
            else
            {
                Debug.LogWarning($"No texture found on material {originalMaterial.name} for renderer {renderers[i].name}");
            }

            // Apply the unique dissolve material to the renderer
            renderers[i].material = dissolveMaterials[i];
        }
    }

    void Update()
    {
        if (isDissolving)
        {
            _CutoffHeight += Time.deltaTime * dissolveSpeed;

            // Update the dissolve material's cutoff height
            foreach (var dissolveMaterial in dissolveMaterials)
            {
                dissolveMaterial.SetFloat("_CutoffHeight", _CutoffHeight);
            }

            // Stop the dissolve when complete
            if (_CutoffHeight >= 1.0f)
            {
                isDissolving = false;
                Debug.Log("Dissolve complete.");
            }
        }
    }

    public void StartDissolve()
    {
        Debug.Log("Dissolve started.");
        isDissolving = true;
        _CutoffHeight = 0f; // Reset dissolve
    }
}

