using UnityEngine;

public class FootstepSoundController : MonoBehaviour
{
    public LayerMask carpetLayer;
    public LayerMask tileLayer;
    public LayerMask woodLayer;

    void Update()
    {
        // Check the material the player is standing on using raycast or collision.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                if (((1 << hit.collider.gameObject.layer) & carpetLayer) != 0)
                {
                    AkSoundEngine.SetSwitch("FootstepMaterial", "Carpet", gameObject);
                }
                else if (((1 << hit.collider.gameObject.layer) & tileLayer) != 0)
                {
                    AkSoundEngine.SetSwitch("FootstepMaterial", "Tile", gameObject);
                }
                else if (((1 << hit.collider.gameObject.layer) & woodLayer) != 0)
                {
                    AkSoundEngine.SetSwitch("FootstepMaterial", "Wood", gameObject);
                }
            }
        }
    }
}
