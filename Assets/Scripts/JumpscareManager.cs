using System.Collections;
using UnityEngine;

public class JumpscareManager : MonoBehaviour
{
    public GameObject jumpscarePrefab; // Prefab to enable/disable
    public float scareDuration = 2f;  // How long the jumpscare lasts

    private void Start()
    {

        jumpscarePrefab.SetActive(false);

    }

    public void TriggerJumpscare()
    {
        if (jumpscarePrefab != null)
        {
            StartCoroutine(HandleJumpscare());
        }
    }

    private IEnumerator HandleJumpscare()
    {
        // Enable the prefab
        jumpscarePrefab.SetActive(true);

        // Wait for the scare duration
        yield return new WaitForSeconds(scareDuration);

        // Disable the prefab
        jumpscarePrefab.SetActive(false);
    }
}
