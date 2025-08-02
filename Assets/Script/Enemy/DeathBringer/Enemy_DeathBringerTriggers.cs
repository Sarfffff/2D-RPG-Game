using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTriggers : EnemyAnimationTrigger
{
    private Enemy_DeathBringer enemyDeathBringer =>GetComponentInParent<Enemy_DeathBringer>();

    private void MakeInvisble() => enemyDeathBringer.fx.MakeTransprent(true);
    private void MakeVisble() => enemyDeathBringer.fx.MakeTransprent(false);
    private void Relocate() => enemyDeathBringer.FindPosition();

}
