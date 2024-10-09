using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isHitByFlashlight = false;

    private EnemySpawner enemySpawner;

    public void HandleFlashlightHit()
    {
        if (!isHitByFlashlight)
        {
            isHitByFlashlight = true;
            StartCoroutine(StopAndDestroy());
        }
    }

    private IEnumerator StopAndDestroy()
    {
        // Set the enemy speed to 0 using the speed value from the EnemySpawner script
        float originalSpeed = enemySpawner.enemySpeed;
        enemySpawner.enemySpeed = 0f;
        // Stop the enemy for 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy the enemy
        Destroy(gameObject);


    }
}

