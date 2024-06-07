using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth, IHazard
{
    [SerializeField] private int _speed;
    [SerializeField] protected NavMeshAgent p_agent;
    [SerializeField] protected Transform p_target;
    [SerializeField] protected Canvas p_lifeView;
    [SerializeField] private BoolChanelSo _startedGame;

    [Header("Parameters")]
    [SerializeField] protected int p_maxLifePoints = 10;
    [SerializeField] protected int p_damage = 1;
    [SerializeField] protected float p_waitForDie = 0.5f;
    [SerializeField] protected float p_attackCD = 1.0f;
    [SerializeField] protected float p_attackDistance = 2f;

    private int _currentLifePoints = 0;
    protected bool p_isAttacking = false;
    
    public event Action onDead;
    public Action<bool> onAttack;

    private void Awake()
    {
        Validate();
        _currentLifePoints = p_maxLifePoints;
        p_agent.speed = _speed;
    }

    protected virtual void Update()
    {
        if ((p_target.position - transform.position).magnitude > p_attackDistance)
            Move();
        else
            AttackLogic();
    }

    protected virtual void Move()
    {
        p_agent.SetDestination(p_target.position);
    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
        if (_currentLifePoints <= 0)
            Dead();
    }

    [ContextMenu("Basic Damage")]
    void IHealth.BasicDamage()
    {
        _currentLifePoints--;
    }

    [ContextMenu("Total Damage")]
    void IHealth.TakeTotalDamage()
    {
        _currentLifePoints = 0;
    }

    public void Dead()
    {
        onDead?.Invoke();
    }

    protected virtual void AttackLogic()
    {
        if(!p_isAttacking)
            StartCoroutine(Attack());
    }

    protected virtual IEnumerator Attack()
    {
        p_isAttacking = true;
        p_agent.SetDestination(transform.position);
        onAttack?.Invoke(p_isAttacking);
        yield return new WaitForSeconds(p_attackCD);
        p_isAttacking = false;
        onAttack?.Invoke(p_isAttacking);
    }

    public int ReturnDamage()
    {
        return p_damage;
    }

    private void Validate()
    {
        if (!p_agent)
        {
            Debug.LogError($"{name}: AgentMesh is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}