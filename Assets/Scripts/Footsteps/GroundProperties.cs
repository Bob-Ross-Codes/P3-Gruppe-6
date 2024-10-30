using UnityEngine;

public class GroundProperties : MonoBehaviour
{
    [Range(0f, 1f)]
    public float WetnessLevel = 0f;  // 0 is dry, 1 is fully wet
}
