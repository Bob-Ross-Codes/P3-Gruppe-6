using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class AutoAssignDissolveMaterials : MonoBehaviour

    /*
    {
        public Shader dissolveShader; // Reference to the dissolve shader

        void Start()
        {
            // Get all renderers in this GameObject and its children
            var renderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                // Loop through each material in the renderer
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    Material originalMaterial = renderer.materials[i];

                    // Create a new material using the dissolve shader
                    Material dissolveMaterial = new Material(dissolveShader);

                    // Copy BaseMap and NormalMap from the original material
                    if (originalMaterial.HasProperty("_BaseMap"))
                        dissolveMaterial.SetTexture("_BaseMap", originalMaterial.GetTexture("_BaseMap"));

                    if (originalMaterial.HasProperty("_NormalMap"))
                        dissolveMaterial.SetTexture("_NormalMap", originalMaterial.GetTexture("_NormalMap"));

                    // Replace the material in the renderer
                    renderer.materials[i] = dissolveMaterial;
                }
            }
        }
    }

    */

/*{
    public Texture2D baseMap; // Assign this in the Inspector
    public Texture2D normalMap; // Assign this in the Inspector
    public Shader dissolveShader; // Reference to the dissolve shader

    void Start()
    {
        // Get all renderers in this object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a new material with the dissolve shader
            Material dissolveMaterial = new Material(dissolveShader);

            // Assign the base map and normal map
            if (baseMap != null)
                dissolveMaterial.SetTexture("_BaseMap", baseMap);

            if (normalMap != null)
                dissolveMaterial.SetTexture("_NormalMap", normalMap);

            // Assign the material to the renderer
            renderer.material = dissolveMaterial;
        }
    }
}*/



{
    public Texture2D baseMap; // Base Map texture to apply
    public Texture2D normalMap; // Normal Map texture to apply

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Loop through all materials of this renderer
            foreach (Material material in renderer.materials)
            {
                // Ensure the material uses the dissolve shader
                if (material.shader.name.Contains("Dissolve")) // Adjust if needed
                {
                    // Update the Base Map and Normal Map in the material
                    if (baseMap != null)
                    {
                        material.SetTexture("_BaseMap", baseMap);
                        Debug.Log($"BaseMap assigned to {material.name}");
                    }

                    if (normalMap != null)
                    {
                        material.SetTexture("_NormalMap", normalMap);
                        Debug.Log($"NormalMap assigned to {material.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"{material.name} does not use the Dissolve Shader.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No Renderer found on {gameObject.name}");
        }
    }
}


}
