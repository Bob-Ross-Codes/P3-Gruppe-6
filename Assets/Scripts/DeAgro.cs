using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAgro : GazeActivation
{

    public Animator monsteranimater;

    public override float ActivationTime => 0.1f;


    public override void OnLookedAt()
    {
        monsteranimater.SetTrigger("DeAgro");
    }

}
