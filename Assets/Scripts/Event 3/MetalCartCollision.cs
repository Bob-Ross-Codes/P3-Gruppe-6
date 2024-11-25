using UnityEngine;
public class MetalCartCollision : MonoBehaviour
{   
    void OnCollisionEnter(Collision collision)
{
    // Get collision force
    float collisionForce = collision.relativeVelocity.magnitude;

    // Set a threshold to filter small collisions
    if (collisionForce > 1.0f) // Adjust threshold as needed
    {
        AkSoundEngine.PostEvent("Play_Cart_Impact", gameObject);
    }
    else
    {
        AkSoundEngine.PostEvent("Play_Cart_Impact", gameObject);
    }
}
}

