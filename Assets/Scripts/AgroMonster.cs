using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMonster : GazeActivation
{
    public override float ActivationTime => 0.1f;
    public  Animator monsteranimater;
    [SerializeField] private Gaze gaze;
    [SerializeField] private float timeToScare;
    [SerializeField] private JumpscareManager scaryMonsterPrefab;
    [SerializeField] private GameObject monsterprefab;
    [SerializeField] private ClosetHide closetHide;
    public override void OnLookedAt()
    {
        timeToScare += Time.deltaTime;
        monsteranimater.SetTrigger("Agro");
        Debug.Log("scary time soon?" + timeToScare + " YES! at 1f");
        if (timeToScare >= 1f)
        {
            scaryMonsterPrefab.TriggerJumpscare();
            Destroy(gameObject);
            Destroy(monsterprefab);
            closetHide.canToggleHiding = true;
        }
        if (gaze._blinking)
            timeToScare = 0;
        
    }

}
