using System.Collections.Generic;
using UnityEngine;

public class OverallDissolveController : MonoBehaviour

{
    [System.Serializable]
    public class DissolvableObject
    {
        public GameObject targetObject; // The object to dissolve
        public Texture2D baseMap; // Base map for this object
        public Texture2D normalMap; // Normal map for this object
    }

    public List<DissolvableObject> dissolvableObjects = new List<DissolvableObject>();

    public Texture2D defaultBaseMap; // Assign a default Base Map in the Inspector
    public Texture2D defaultNormalMap; // Assign a default Normal Map in the Inspector

    private void Start()
    {
        // Automatically populate the list with child objects
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Create a new dissolvable object
                DissolvableObject dissolvable = new DissolvableObject
                {
                    targetObject = child.gameObject,
                    baseMap = defaultBaseMap, // Use default Base Map
                    normalMap = defaultNormalMap // Use default Normal Map
                };

                dissolvableObjects.Add(dissolvable);
            }
        }

        Debug.Log($"Added {dissolvableObjects.Count} children to the dissolve list.");
    }

    public void TriggerDissolve()
    {
        foreach (var dissolvable in dissolvableObjects)
        {
            if (dissolvable.targetObject != null)
            {
                var dissolveScript = dissolvable.targetObject.GetComponent<DissolveOffest>();
                if (dissolveScript != null)
                {
                    // Assign the textures dynamically
                    dissolveScript.AssignTextures(dissolvable.baseMap, dissolvable.normalMap);
                    // Start the dissolve effect
                    dissolveScript.ActivateDissolve();
                }
                else
                {
                    Debug.LogWarning($"No DissolveOffest script found on {dissolvable.targetObject.name}");
                }
            }
        }
    }
}



