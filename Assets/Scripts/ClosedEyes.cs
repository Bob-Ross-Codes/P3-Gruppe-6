using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{

    public Gaze gaze;
    [SerializeField] private float blinkOffSet;
    private float blinkTime;

   private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B) || gaze._blinking)
        {
            blinkTime += Time.deltaTime;
            if (blinkTime >= blinkOffSet)
                AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
            // Debug.Log("BlinkingSound");

        }
        else AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject); blinkTime = 0;


    }
}
