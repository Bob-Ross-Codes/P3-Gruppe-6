using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // for FirstPersonController

public class RunEnableTrigger : MonoBehaviour
{

    //when player hits the box collider, enable change the run value of the player controller script
    public float newWalkSpeed = 2.0f;
    public CinemachineVirtualCamera camera; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<FirstPersonController>();
            if (controller != null)
            {
                controller.MoveSpeed = newWalkSpeed;
                StartCoroutine(SmoothTransition(camera.m_Lens.FieldOfView, 90, 1.0f)); // 1 second transition
            }
        }
    }

    private IEnumerator SmoothTransition(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            camera.m_Lens.FieldOfView = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.m_Lens.FieldOfView = to;
    }


}
