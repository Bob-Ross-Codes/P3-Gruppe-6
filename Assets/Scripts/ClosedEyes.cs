using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{

    public Gaze gaze;

    // M�ske vi skal s�tte det her script p� playeren s�ledes det altid er til stede?
    void Update()
    {

        if (gaze._blinking)
        {
            // Marius g�r din ting!
            AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
            
            // Storede fede p�lsepatter
        }
        AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);


    }
}
