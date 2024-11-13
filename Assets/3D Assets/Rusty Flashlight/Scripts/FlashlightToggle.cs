using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightToggle : MonoBehaviour
{
    public GameObject lightGO; //light gameObject to work with
    private bool isOn = false; //is flashlight on or off?

    // Use this for initialization
    void Start()
    {
        //set default off
        lightGO.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        //toggle flashlight on key down
        if (Input.GetKeyDown(KeyCode.X))
        {
            //toggle light
            isOn = !isOn;
            //turn light on
            if (isOn)
            {
                AkSoundEngine.SetSwitch("FlashlightSwitch", "On", gameObject);
                AkSoundEngine.PostEvent("Flashlight_OnOff_Event", gameObject);
                lightGO.SetActive(true);
            }
            //turn light off
            else
            {
                AkSoundEngine.SetSwitch("FlashlightSwitch", "Off", gameObject);
                AkSoundEngine.PostEvent("Flashlight_OnOff_Event", gameObject);
                lightGO.SetActive(false);

            }
        }
    }
}
