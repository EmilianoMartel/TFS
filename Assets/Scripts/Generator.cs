using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IHealth
{
    [SerializeField] private int _maxLife;
    private int _actualLife;

    public event Action dead;

    private void Awake()
    {
        _actualLife = _maxLife;
    }

    public void Dead()
    {
        dead?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _actualLife -= damage;

        if(_actualLife < 0)
            dead();
    }

    [ContextMenu("Basic Damage")]
    void IHealth.BasicDamage()
    {
        _actualLife--;
    }

    [ContextMenu("Take total Damage")]
    void IHealth.TakeTotalDamage()
    {
        _actualLife = 0;
        this.dead();
    }
}