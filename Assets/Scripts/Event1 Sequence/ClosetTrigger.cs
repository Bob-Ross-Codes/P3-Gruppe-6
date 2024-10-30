using UnityEngine;

public class ClosetTrigger : MonoBehaviour
{
    public MonsterSequenceController sequenceController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the closet
            sequenceController.OnPlayerHidden();
        }
    }
}
