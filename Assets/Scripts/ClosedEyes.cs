using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{

    [SerializeField] private float blinkOffSet;
    private float blinkTime;

    public bool _blinking;
    private void Update()
    {
        //Debug.Log(" is brother blinking "+ gaze._blinking);
        while (Input.GetKeyDown(KeyCode.B))
        {
            _blinking = true;
            Debug.Log(_blinking);

            if (_blinking == true)
            {
                blinkTime += Time.deltaTime;
                if (blinkTime >= blinkOffSet)
                {

                    AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
                }
                //   Debug.Log("BlinkingSound");
            }
        }
       
        {
            AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
            blinkTime = 0;
            _blinking = false;
        }
    }
}
