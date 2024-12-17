/// <summary>
/// Handles flickering lights in the scene and plays sound effects using Wwise.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightManager : MonoBehaviour
{
    [SerializeField] public Light[] allLights; // All lights in the scene
    [SerializeField] private Light[] excludedLights; // Lights to exclude from flickering
    private Light[] lightsToFlicker; // Lights that will flicker
    [SerializeField] private Light handlight; // Optional handlight to include in flickering
    public GameObject Player; // Player object for sound control

    private float flickerDuration; // How long the flickering lasts
    private float darkInterval; // How long lights stay off
    private float lightInterval; // How long lights stay on

    public bool flickeringOn; // Is the flickering active?
    public bool lightsOn = false; // Are the lights currently on?

    /// <summary>
    /// Updates the list of lights to exclude handlight and excluded lights.
    /// </summary>
    public void Update()
    {
        allLights = FindObjectsOfType<Light>()
            .Where(light => light != handlight && !excludedLights.Contains(light))
            .ToArray();
    }

    /// <summary>
    /// Starts flickering lights.
    /// </summary>
    /// <param name="duration">How long to flicker.</param>
    /// <param name="speed">Flickering speed.</param>
    /// <param name="handLight">Include handlight in flickering?</param>
    public void StartFlicker(float duration, float speed, bool handLight)
    {
        if (!flickeringOn)
        {
            flickeringOn = true;
            Debug.Log("Starting Flicker");

            lightsToFlicker = new List<Light>(allLights).ToArray();
            if (handLight) lightsToFlicker = lightsToFlicker.Append(handlight).ToArray();

            StartCoroutine(Flickering(duration, speed));
        }
    }

    /// <summary>
    /// Stops flickering lights.
    /// </summary>
    public void StopFlicker()
    {
        flickeringOn = false;
    }

    /// <summary>
    /// Flickers lights on and off at random intervals.
    /// </summary>
    /// <param name="duration">How long to flicker.</param>
    /// <param name="speed">Flickering speed.</param>
    private IEnumerator Flickering(float duration, float speed)
    {
        Debug.Log("Now Flickering");
        float elapsedTime = 0;
        flickerDuration = duration;

        while (elapsedTime < flickerDuration && flickeringOn)
        {
            darkInterval = Random.Range(0.1f, 0.4f);
            lightInterval = Random.Range(0.05f, 0.1f);

            foreach (var light in lightsToFlicker) if (light != null) light.enabled = false;
            WwiseStopLightsSounds();
            lightsOn = false;

            yield return new WaitForSeconds(darkInterval * speed);
            elapsedTime += darkInterval;

            foreach (var light in lightsToFlicker) if (light != null) light.enabled = true;
            WwisePlayFlickeringSound();
            lightsOn = true;

            yield return new WaitForSeconds(lightInterval * speed);
            elapsedTime += lightInterval;
        }

        foreach (var light in lightsToFlicker) if (light != null) light.enabled = true;
        lightsOn = true;
        flickeringOn = false;
        WwiseStopLightsSounds();
    }

    /// <summary>
    /// Plays flickering sound.
    /// </summary>
    private void WwisePlayFlickeringSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_LightState", 2, Player);
        AkSoundEngine.PostEvent("Play_Light_OnOff_Event", Player);
    }

    /// <summary>
    /// Stops light sounds.
    /// </summary>
    private void WwiseStopLightsSounds()
    {
        AkSoundEngine.PostEvent("Stop_Light_OnOff_Event", Player);
    }
}