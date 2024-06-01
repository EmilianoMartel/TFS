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

    private int _currentLifePoints = 0;

    public event Action onDead;

    private void Move()
    {

    }

    public void TakeDamage(int damage)
    {
        _currentLifePoints -= damage;
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

    void IHealth.Dead()
    {
        throw new System.NotImplementedException();
    }

    public int ReturnDamage()
    {
        return _damage;
    }
}