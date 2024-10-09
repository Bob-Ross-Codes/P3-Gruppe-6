using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class NoiseEmissionModulator : MonoBehaviour
{
    public Color baseEmissionColor = Color.white;  // Base color for emission
    public float minEmissionIntensity = 0.5f;      // Minimum emission intensity
    public float maxEmissionIntensity = 2.0f;      // Maximum emission intensity
    public float noiseScale = 1.0f;                // Scale of noise (affects how rapidly the emission intensity changes)
    
    private Renderer objectRenderer;
    private Material objectMaterial;
    private float emissionIntensity;

    void Start()
    {
        // Get the renderer component on the object
        objectRenderer = GetComponent<Renderer>();

        // Create a copy of the material to avoid modifying the shared material
        objectMaterial = objectRenderer.material;

        // Ensure the material supports emission by enabling the emission keyword
        objectMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Generate Perlin noise value based on the object's world position and time to modulate emission
        float noiseValue = Mathf.PerlinNoise(Time.time * noiseScale, transform.position.x * noiseScale);
        
        // Lerp between the minimum and maximum intensity using the noise value
        emissionIntensity = Mathf.Lerp(minEmissionIntensity, maxEmissionIntensity, noiseValue);

        // Set the emission color by multiplying the base color by the intensity
        Color currentEmissionColor = baseEmissionColor * emissionIntensity;
        objectMaterial.SetColor("_EmissionColor", currentEmissionColor);

        // Update the emission in the global illumination system
        DynamicGI.SetEmissive(objectRenderer, currentEmissionColor);
    }

    // Optional: Clean up the material instance when the object is destroyed
    void OnDestroy()
    {
        if (objectMaterial != null)
        {
            Destroy(objectMaterial);
        }
    }
}
