using UnityEngine;

public class TriggerDissolve : MonoBehaviour

{
    public OverallDissolveController dissolveController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone. Triggering dissolve.");
            dissolveController.TriggerDissolve();
        }
    }
}
