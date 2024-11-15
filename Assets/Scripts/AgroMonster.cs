using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroMonster : GazeActivation
{
    public override float ActivationTime => 0.1f;
    public  Animator monsteranimater;

    public override void OnLookedAt()
    {
        monsteranimater.SetTrigger("Agro");
    }
}
