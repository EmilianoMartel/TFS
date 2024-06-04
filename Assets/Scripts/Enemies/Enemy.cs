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
    [SerializeField] protected int _maxLifePoints = 10;
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _waitForDie = 0.5f;
    [SerializeField] protected float _attackCD = 1.0f;
    [SerializeField] private float _attackDistance = 2f;

    private int _currentLifePoints = 0;
    private bool _isAttacking = false;
    
    public event Action onDead;
    public event Action onAttack;

    private void Awake()
    {
        _currentLifePoints = _maxLifePoints;
        p_agent.speed = _speed;
    }

    private void Update()
    {
        if ((p_target.position - transform.position).magnitude > _attackDistance)
            Move();
        else
            AttackLogic();
    }

    private void Move()
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
        throw new System.NotImplementedException();
    }

    protected virtual void AttackLogic()
    {
        if(!_isAttacking)
            StartCoroutine(Attack());
    }

    protected virtual IEnumerator Attack()
    {
        _isAttacking = true;
        onAttack?.Invoke();
        yield return new WaitForSeconds(_attackCD);
        _isAttacking = false;
    }

    public int ReturnDamage()
    {
        return _damage;
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