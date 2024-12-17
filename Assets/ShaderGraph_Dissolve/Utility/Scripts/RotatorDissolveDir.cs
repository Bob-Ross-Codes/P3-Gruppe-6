using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    /// <summary>
    /// Updates the dissolve direction of materials by rotating a vector over time.
    /// </summary>
    public class RotatorDissolveDir : MonoBehaviour
    {
        [Header("Rotation Speed")]
        [Tooltip("The speed at which the dissolve direction vector rotates.")]
        public Vector3 Speed;

        // List of materials on the object
        private List<Material> materials = new List<Material>();

        /// <summary>
        /// Initializes the list of materials by fetching all materials from the Renderer component.
        /// </summary>
        void Start()
        {
            materials.AddRange(GetComponent<Renderer>().materials);
        }

        /// <summary>
        /// Updates the dissolve direction vector of each material based on the specified speed.
        /// </summary>
        void Update()
        {
            // Iterate through all materials and adjust their "_DissolveDirection" property
            for (int i = 0; i < materials.Count; i++)
            {
                // Get the current dissolve direction
                var value = materials[i].GetVector("_DissolveDirection");
                
                // Calculate the change in direction based on speed and time
                var delta = Speed * Time.deltaTime;

                // Add the change to the current value
                value += new Vector4(delta.x, delta.y, delta.z, 0f);

                // Update the dissolve direction in the material
                materials[i].SetVector("_DissolveDirection", value);
            }
        }
    }
}