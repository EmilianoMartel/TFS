using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttack : Enemy
{
    [SerializeField] private float _specialAttackAnimationTime = 0.5f;
    [SerializeField] private float _specialAttackDistance = 5;
    [SerializeField] private float _specialAttackCd = 5f;

    private float _counter = 0;

    private bool _specialAttackStarted = false;

    public event Action startSpecialAttack;
    public event Action middelSpecialAttack;
    public event Action endSpecialAttack;

    protected override void Update()
    {
        if (_specialAttackStarted)
            return;
        if ((p_target.position - transform.position).magnitude > p_attackDistance)
            Move();
        else
            AttackLogic();

        if (_counter > _specialAttackCd)
        {
            _counter = 0;
            StartCoroutine(SpecialAttack());
        }
        else
        {
            _counter += Time.deltaTime;
        }
    }

    protected override void Move()
    {
        p_agent.SetDestination(p_target.position);
    }

    private IEnumerator SpecialAttack()
    {
        _specialAttackStarted = true;
        startSpecialAttack?.Invoke();
        yield return new WaitForSeconds(_specialAttackAnimationTime);
        middelSpecialAttack?.Invoke();
        endSpecialAttack?.Invoke();
        _specialAttackStarted = false;
    }

}