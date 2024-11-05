using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // for FirstPersonController

public class CameraPriorityManager : MonoBehaviour
{
    public static CameraPriorityManager Instance { get; private set; }

    private Dictionary<GameObject, CinemachineVirtualCamera> activeObjects = new Dictionary<GameObject, CinemachineVirtualCamera>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you want it to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCameraPriority(GameObject obj, CinemachineVirtualCamera mainCamera, CinemachineVirtualCamera doorCamera, bool isHiding)
    {
        if (isHiding)
        {
            // Register the object and set the priority
            if (!activeObjects.ContainsKey(obj))
            {
                activeObjects[obj] = doorCamera;
            }

            mainCamera.Priority = 0;
            doorCamera.Priority = 10;
        }
        else
        {
            // Unregister the object and reset the priority
            if (activeObjects.ContainsKey(obj))
            {
                activeObjects.Remove(obj);
            }

            mainCamera.Priority = 10;
            doorCamera.Priority = 0;
        }
    }
}
