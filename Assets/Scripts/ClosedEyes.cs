using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{
    [SerializeField] private float blinkOffSet;
    private float blinkTime;




    public bool _blinking;

    private bool soundPlaying;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _blinking = true;
            Debug.Log(_blinking);
            soundPlaying = true;
        }

        if (_blinking)
        {
            blinkTime += Time.deltaTime;
            if (blinkTime >= blinkOffSet && soundPlaying == true)
            {
                
                AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
                soundPlaying = false;
            }
        }
     
        if (Input.GetKeyUp(KeyCode.B))
        {
            AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
            blinkTime = 0;
            _blinking = false;
            Debug.Log(_blinking);
            soundPlaying = false;
        }
    }
}
