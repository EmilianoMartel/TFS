using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGenerator : Buff
{
    [SerializeField] private int _health = 10;
    [SerializeField] private GeneratorSource _source;

    public override void DoSomething(PlayerController player)
    {
        if(_source.Reference != null)
            _source.Reference.Health(_health);

        gameObject.SetActive(false);
    }
}
