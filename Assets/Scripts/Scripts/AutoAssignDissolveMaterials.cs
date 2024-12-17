using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    /// <summary>
    /// Automatically assigns Base Map and Normal Map textures to materials using a Dissolve shader.
    /// </summary>
    public class AutoAssignDissolveMaterials : MonoBehaviour
    {
        [Header("Texture Settings")]
        [SerializeField] private Texture2D baseMap; // Base Map texture to apply
        [SerializeField] private Texture2D normalMap; // Normal Map texture to apply

        /// <summary>
        /// Assigns textures to materials using a Dissolve shader when the game starts.
        /// </summary>
        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                // Loop through all materials in the renderer
                foreach (Material material in renderer.materials)
                {
                    // Ensure the material uses the Dissolve shader
                    if (material.shader.name.Contains("Dissolve")) // Adjust shader name check as needed
                    {
                        // Assign the Base Map texture
                        if (baseMap != null)
                        {
                            material.SetTexture("_BaseMap", baseMap);
                            Debug.Log($"Base Map assigned to {material.name}");
                        }

                        // Assign the Normal Map texture
                        if (normalMap != null)
                        {
                            material.SetTexture("_NormalMap", normalMap);
                            Debug.Log($"Normal Map assigned to {material.name}");
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