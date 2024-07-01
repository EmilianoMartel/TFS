using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IHealth<Generator>
{
    [SerializeField] private int _maxLife;
    [SerializeField] private GeneratorSource _source;
    [SerializeField] private FloatChannel _generatorDamageEvent;

    private int _actualLife;

    public event Action dead;

    private void OnDisable()
    {
        _source.Reference = null;
    }

    private void Awake()
    {
        _actualLife = _maxLife;
        _source.Reference = this;
    }

    public void Dead()
    {
        dead?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _actualLife -= damage;
        if (_actualLife < 0)
            dead();

        _generatorDamageEvent?.InvokeEvent((float)_actualLife / (float)_maxLife);
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

    void IHealth<Generator>.SuscribeAction(Action<Generator> action)
    {
        throw new NotImplementedException();
    }

    void IHealth<Generator>.Unsuscribe(Action<Generator> action)
    {
        throw new NotImplementedException();
    }

    void IHealth.SuscribeAction(Action action)
    {
        throw new NotImplementedException();
    }

    void IHealth.Unsuscribe(Action action)
    {
        throw new NotImplementedException();
    }

    public void Health(int hp)
    {
        if (_actualLife + hp >= _maxLife)
            _actualLife = _maxLife;

        _actualLife += hp;
        _generatorDamageEvent?.InvokeEvent((float)_actualLife / (float)_maxLife);
    }
}