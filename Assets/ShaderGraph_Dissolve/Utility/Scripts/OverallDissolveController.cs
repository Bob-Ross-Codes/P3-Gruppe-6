using System.Collections.Generic;
using UnityEngine;

public class OverallDissolveController : MonoBehaviour

{
    [System.Serializable]
    public class DissolvableObject
    {
        public GameObject targetObject; // The object to dissolve
        public Texture2D baseMap;       // Base map for this object
        public Texture2D normalMap;     // Normal map for this object
    }

    public List<DissolvableObject> dissolvableObjects = new List<DissolvableObject>();
    public Texture2D defaultBaseMap; // Default Base Map (assigned in Inspector)
    public Texture2D defaultNormalMap; // Default Normal Map (assigned in Inspector)

    private void Start()
    {
        // Automatically populate dissolvable objects from children
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Create a new dissolvable object and assign defaults
                DissolvableObject dissolvable = new DissolvableObject
                {
                    targetObject = child.gameObject,
                    baseMap = defaultBaseMap,
                    normalMap = defaultNormalMap
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
                // Get the dissolve script on the object
                var dissolveScript = dissolvable.targetObject.GetComponent<DissolveOffest>();
                if (dissolveScript != null)
                {
                    // Assign the textures (defaults if not overridden)
                    dissolveScript.AssignTextures(dissolvable.baseMap, dissolvable.normalMap);

                    // Trigger dissolve
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
