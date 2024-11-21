using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{

    public Gaze gaze;

    // M�ske vi skal s�tte det her script p� playeren s�ledes det altid er til stede?
    private bool isBlinking = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
           // Debug.Log("BlinkingSound");
            AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
        }

        if (gaze._blinking && !isBlinking)
        {
            // Marius g�r din ting!
            AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
            isBlinking = true;
        }
    }
}
