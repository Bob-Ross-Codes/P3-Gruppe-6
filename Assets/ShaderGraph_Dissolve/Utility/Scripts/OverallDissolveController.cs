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

    public List<DissolvableObject> dissolvableObjects = new List<DissolvableObject>(); // List of all dissolvable objects

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


