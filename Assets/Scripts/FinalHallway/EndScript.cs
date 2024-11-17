using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
  //if player tag hits the collider load final scene
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the end of the hallway");
            // Load the final scene called "Ending"
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
}
