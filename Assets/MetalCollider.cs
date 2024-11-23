using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("MetalObj"))
        {
            //MARIUS: lyd af to metal der rammer, random mellem 3 lyde
            Debug.Log("Collided with metal");
        }else
        {
            Debug.Log("Collided with wall");
            //MARIUS: Lyd af metal der rammer væggen, random mellem 2-3 lyde
        }
    }
}
