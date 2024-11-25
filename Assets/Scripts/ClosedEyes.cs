using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{

    [SerializeField] private Gaze gaze;
    [SerializeField] private float blinkOffSet;
    private float blinkTime;

    private bool blinking;
    private void Update()
    {
        //Debug.Log(" is brother blinking "+ gaze._blinking);
        if (Input.GetKeyDown(KeyCode.B) || gaze._blinking)
        {
            if (blinking == false)
            {
                blinking = true;

                blinkTime += Time.deltaTime;
                if (blinkTime >= blinkOffSet)
                    AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
                //   Debug.Log("BlinkingSound");

            }

            else
            {
                AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
                blinkTime = 0;
                blinking = false;
            }
        }
    }
}
