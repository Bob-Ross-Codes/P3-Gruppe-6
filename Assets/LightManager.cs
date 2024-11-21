using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightManager : MonoBehaviour
{
    [SerializeField] public Light[] allLights;  // Array of lights
    [SerializeField] private Light[] excludedLights; // Array of lights to exclude
    private Light[] lightsToFlicker;
    [SerializeField] private Light handlight;

    private float flickerDuration;  // Total duration of flickering effect
    private float darkInterval;
    private float lightInterval;
    public bool flickeringOn;
    public bool lightsOn = false;

    public void FixedUpdate()
    {
        // Find all lights in the scene
        var allSceneLights = FindObjectsOfType<Light>();

        // Exclude the handLight and lights in the excludedLights array
        allLights = allSceneLights
            .Where(light => light != handlight && !excludedLights.Contains(light))
            .ToArray();
    }

    public void StartFlicker(float duration, float speed, bool handLight)
    {
        if (!flickeringOn)
        {
            flickeringOn = true;
            Debug.Log("Starting Flicker");
            if (handLight)
            {
                lightsToFlicker = new List<Light>(allLights).ToArray();  // Convert to array
                List<Light> tempList = new List<Light>(lightsToFlicker); // Optionally use List later if needed
                tempList.Add(handlight); // Add handlight to temp list
                lightsToFlicker = tempList.ToArray(); // Convert back to array
            }
            else
                lightsToFlicker = new List<Light>(allLights).ToArray();  // Convert to array

            StartCoroutine(Flickering(duration, speed, handLight));
        }
    }

    public void StopFlicker()
    {
        flickeringOn = false;
    }

    public IEnumerator Flickering(float duration, float speed, bool handLight)
    {
        Debug.Log("Now Flickering");
        float elapsedTime = 0;
        if (flickeringOn)
        {
            flickerDuration = duration;

            while (elapsedTime < flickerDuration && flickeringOn)
            {
                darkInterval = Random.Range(0.1f, 0.4f);
                lightInterval = Random.Range(0.05f, 0.1f);

                // Turn off all lights (darker state)
                foreach (var light in lightsToFlicker)
                    light.enabled = false;
                WwiseStopLightsSounds();
                lightsOn = false;

                yield return new WaitForSeconds(darkInterval * speed);
                elapsedTime += darkInterval;

                // Turn on all lights (brief light state)
                foreach (var light in lightsToFlicker)
                {
                    light.enabled = true;
                    AkSoundEngine.PostEvent("Flickering_Lights", gameObject);
                }
                WwisePlayFlickeringSound();
                lightsOn = true;

                yield return new WaitForSeconds(lightInterval * speed);
                elapsedTime += lightInterval;
            }
            foreach (var light in lightsToFlicker)
            {
                light.enabled = true; lightsOn = true;
            }

            flickeringOn = false;
        }
        else { flickeringOn = false; yield break; }
    }

    // Flickering sounds
    private void WwisePlayFlickeringSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_LightState", 2, gameObject);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", gameObject);
    }

    private void WwisePlayTurnOnSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_LightState", 1, gameObject);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", gameObject);
    }

    private void WwisePlayTurnOffSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_LightState", 0, gameObject);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", gameObject);
    }

    private void WwiseStopLightsSounds()
    {
        AkSoundEngine.PostEvent("Stop_Light_OnOff_Event", gameObject);
    }
}
