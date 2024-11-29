using UnityEngine;

namespace DissolveExample
{
    public class AutoAssignDissolveMaterials : MonoBehaviour
    {
        public Texture2D baseMap; // Base Map texture to apply
        public Texture2D normalMap; // Normal Map texture to apply
        public Shader dissolveShader; // Reference to the dissolve shader

        void Start()
        {
            if (dissolveShader == null)
            {
                Debug.LogError("Dissolve shader not assigned. Please assign a shader in the inspector.");
                return;
            }

            // Get all renderers in this GameObject and its children
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    Material originalMaterial = materials[i];

                    if (originalMaterial.shader.name.Contains("Dissolve"))
                    {
                        Debug.Log($"Material '{originalMaterial.name}' already uses a dissolve shader.");
                        AssignTextures(originalMaterial);
                    }
                    else
                    {
                        // Create a new material using the dissolve shader
                        Material dissolveMaterial = new Material(dissolveShader);

                        // Copy textures from the original material
                        if (originalMaterial.HasProperty("_BaseMap"))
                        {
                            dissolveMaterial.SetTexture("_BaseMap", originalMaterial.GetTexture("_BaseMap"));
                        }
                        if (originalMaterial.HasProperty("_NormalMap"))
                        {
                            dissolveMaterial.SetTexture("_NormalMap", originalMaterial.GetTexture("_NormalMap"));
                        }

                        // Assign custom textures if provided
                        AssignTextures(dissolveMaterial);

                        // Replace the original material with the dissolve material
                        materials[i] = dissolveMaterial;

                        Debug.Log($"Replaced material '{originalMaterial.name}' with dissolve material.");
                    }
                }

                // Update the renderer's materials
                renderer.materials = materials;
            }
        }

        /// <summary>
        /// Assigns Base Map and Normal Map textures to the given material if available.
        /// </summary>
        /// <param name="material">The material to modify.</param>
        private void AssignTextures(Material material)
        {
            if (baseMap != null)
            {
                material.SetTexture("_BaseMap", baseMap);
                Debug.Log($"Assigned BaseMap texture to '{material.name}'.");
            }

            if (normalMap != null)
            {
                material.SetTexture("_NormalMap", normalMap);
                Debug.Log($"Assigned NormalMap texture to '{material.name}'.");
            }
        }
    }
}
