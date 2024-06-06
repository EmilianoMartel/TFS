using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistance : Enemy
{
    protected override void AttackLogic()
    {
        if (!p_isAttacking)
            StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()
    {
        p_isAttacking = true;
        p_agent.SetDestination(transform.position);
        onAttack?.Invoke(p_isAttacking);
        yield return new WaitForSeconds(p_attackCD);
        p_isAttacking = false;
        onAttack?.Invoke(p_isAttacking);
    }


}
