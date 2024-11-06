using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario01 : GazeActivation
{
    // Provide the specific activation time for this scenario
    public override float ActivationTime => 2.0f; // Example: 2 seconds

    // Public field to attach the GameObject in the Unity Inspector
    public GameObject objectToSpin;

    public override void OnLookedAt()
    {
        if (objectToSpin != null)
        {
            StartCoroutine(SpinObject());
        }
    }

    private IEnumerator SpinObject()
    {
        float duration = 5.0f; // Duration of the spin in seconds
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            objectToSpin.transform.Rotate(Vector3.up, 360 * Time.deltaTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
