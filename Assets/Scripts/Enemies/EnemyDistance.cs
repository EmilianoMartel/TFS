using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistance : Enemy
{
    [SerializeField] private Transform _pointShoot;

    [SerializeField] private ActionChanel<Transform, Vector3> _shoot;

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
        _shoot?.InvokeEvent(_pointShoot,_pointShoot.forward);
        yield return new WaitForSeconds(p_attackCD);
        p_isAttacking = false;
        onAttack?.Invoke(p_isAttacking);
    }
}
