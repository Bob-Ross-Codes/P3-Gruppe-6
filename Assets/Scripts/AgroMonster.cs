using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMonster : MonoBehaviour
{
    
    public  Animator monsteranimater;
    [SerializeField] private ClosedEyes gaze;
    [SerializeField] private float timeToScare;
    [SerializeField] private JumpscareManager scaryMonsterPrefab;
    [SerializeField] private GameObject monsterprefab;
    [SerializeField] private ClosetHide closetHide;
    public void FixedUpdate()
    {

        if (gaze._blinking == false)
        {


            timeToScare += Time.deltaTime;
            monsteranimater.SetTrigger("Agro");
            Debug.Log("scary time soon?" + timeToScare + " YES! at 1f");
            if (timeToScare >= 2f)
            {

                scaryMonsterPrefab.TriggerJumpscare();
                Destroy(gameObject);

                Destroy(monsterprefab);
                closetHide.canToggleHiding = true;
            }
        }
        else
            timeToScare = 0;
        
    }

}
