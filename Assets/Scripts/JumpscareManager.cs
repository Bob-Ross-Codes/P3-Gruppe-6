using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareManager : MonoBehaviour
{
    public GameObject jumpscarePrefab; // Prefab to enable/disable
    public float scareDuration = 2f;  // How long the jumpscare lasts
    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private CanvasGroup deathCanvasGroup; // Reference to the CanvasGroup component

    private void Start()
    {
        jumpscarePrefab.SetActive(false);
    }

 

    public void TriggerJumpscare()
    {
        if (jumpscarePrefab != null)
        {
            AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 2);
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Enable the canvas
        deathCanvas.gameObject.SetActive(true);
        jumpscarePrefab.SetActive(true);
        float duration = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }
        yield return new WaitForSeconds(scareDuration);
        // Disable the prefab
        jumpscarePrefab.SetActive(false);
        duration = 5f;
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }


        // Wait for 5 seconds
        yield return new WaitForSeconds(2); // Redundent tror jeg.

        // Disable the canvas
        deathCanvas.gameObject.SetActive(false);
    }

}
