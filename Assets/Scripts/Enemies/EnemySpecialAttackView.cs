using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttackView : EnemyViewController
{
    [SerializeField] private string _isSpecialParameter = "isSpecialAttack";

    private EnemySpecialAttack _special;

    private void Awake()
    {
        _special = p_enemy.GetComponent<EnemySpecialAttack>();
        _special.startSpecialAttack += HandleSpecialAttack;
        _special.endSpecialAttack += HandleSpecialEnded;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _special.startSpecialAttack -= HandleSpecialAttack;
        _special.endSpecialAttack -= HandleSpecialEnded;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void HandleSpecialAttack()
    {
        p_animetor.SetBool(_isSpecialParameter, true);
    }

    private void HandleSpecialEnded()
    {
        p_animetor.SetBool(_isSpecialParameter, false);
    }
}
