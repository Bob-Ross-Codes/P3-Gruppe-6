using UnityEngine;

public class DissolveTrigger : MonoBehaviour
{
    public DissolveObject dissolveObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dissolveObject.StartDissolve();
        }
    }
}
