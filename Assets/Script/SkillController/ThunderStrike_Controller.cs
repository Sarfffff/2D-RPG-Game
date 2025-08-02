using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats PlayerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            PlayerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
