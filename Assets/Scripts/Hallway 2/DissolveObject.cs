using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    public float _CutoffHeight = 0f;      // Controls height for dissolve
    public float _NoiseScale = 1f;       // Controls the size of the noise
    public float _NoiseStrength = 0.5f;  // Controls the intensity of the noise
    public float dissolveSpeed = 0.1f;   // Speed of the dissolve effect

    private bool isDissolving = false;

    void Update()
    {
        if (isDissolving)
        {
            // Incrementally dissolve
            _CutoffHeight += Time.deltaTime * dissolveSpeed;
            Shader.SetGlobalFloat("_CutoffHeight", _CutoffHeight);

            // Stop when fully dissolved
            if (_CutoffHeight >= 1.0f)
            {
                isDissolving = false;
            }
        }

        // Always update noise parameters
        Shader.SetGlobalFloat("_NoiseScale", _NoiseScale);
        Shader.SetGlobalFloat("_NoiseStrength", _NoiseStrength);
    }

    public void StartDissolve()
    {
        isDissolving = true;
        _CutoffHeight = 0f; // Reset dissolve effect
    }
}
