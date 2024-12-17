using UnityEngine;

/// <summary>
/// Triggers a dissolve effect on a specified object when the player enters a defined area.
/// </summary>
public class DissolveTrigger : MonoBehaviour
{
    [Header("Dissolve Settings")]
    [Tooltip("Reference to the DissolveObject script controlling the dissolve effect.")]
    [SerializeField] private DissolveObject dissolveObject;

    [Tooltip("The distance in front of the player at which the dissolve effect may be triggered.")]
    [SerializeField] private float offsetDistance = 2f;

    /// <summary>
    /// Called when a collider enters this trigger area.
    /// If the collider belongs to the player, it starts the dissolve effect on the assigned object.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the dissolve effect when the player enters the trigger area
            dissolveObject.StartDissolve();
        }
    }
}