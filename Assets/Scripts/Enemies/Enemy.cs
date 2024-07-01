using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth<Enemy>, IHazard
{
    [SerializeField] protected int p_speed = 1;
    [SerializeField] protected NavMeshAgent p_agent;
    [SerializeField] protected Transform p_target;
    [SerializeField] protected Canvas p_lifeView;
    [SerializeField] private BoolChanelSo _startedGame;
    [SerializeField] private TransformChannelSo _spawnBuffPositionEvent;

    [Header("Parameters")]
    [SerializeField] protected int p_maxLifePoints = 10;
    [SerializeField] protected int p_damage = 1;
    [SerializeField] protected float p_waitForDie = 0.5f;
    [SerializeField] protected float p_attackCD = 1.0f;
    [SerializeField] protected float p_attackDistance = 2f;
    [SerializeField] protected float p_dropChance = 50.0f;

    private int _currentLifePoints = 0;
    protected bool p_isAttacking = false;

    public event Action<float> lifeChange;
    public event Action<Enemy> onDead = delegate { };
    public Action<bool> onAttack;

    private void Awake()
    {
        Validate();
        _currentLifePoints = p_maxLifePoints;
        p_agent.speed = p_speed;
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

    public void SetTarget(Transform target)
    {
        p_target = target;
    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
        if (_currentLifePoints <= 0)
        {
            Dead();
            _currentLifePoints = 0;
        }

        lifeChange?.Invoke((float)_currentLifePoints / p_maxLifePoints);
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
        p_agent.speed = 0;
        onDead?.Invoke(this);
        DropCheck();
        StartCoroutine(WaitForDeath());
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        p_agent.speed = p_speed;
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

    public void SuscribeAction(Action<Enemy> action)
    {
        onDead += action;
    }

    public void Unsuscribe(Action<Enemy> action)
    {
        onDead -= action;
    }

    void IHealth.SuscribeAction(Action action)
    {
        throw new NotImplementedException();
    }

    void IHealth.Unsuscribe(Action action)
    {
        throw new NotImplementedException();
    }

    private void DropCheck()
    {
        int chance = UnityEngine.Random.Range(0, 100);
        if (chance <= p_dropChance)
            _spawnBuffPositionEvent?.InvokeEvent(transform);
    }
}