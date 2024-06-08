using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttackView : EnemyViewController
{
    [SerializeField] private string _isSpecialParameter = "isSpecialAttack";
    [SerializeField] private string _isSpecialMomentParameter = "SpecialMoment";

    private EnemySpecialAttack _special;

    private void Awake()
    {
        _special = p_enemy.GetComponent<EnemySpecialAttack>();
        _special.startSpecialAttack += HandleSpecialAttack;
        _special.endSpecialAttack += HandleSpecialEnded;
        _special.middelSpecialAttack += HandleSpecialMid;
        _special.groundSpecialAttack += HandleSpecialEndWait;
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
        _special.middelSpecialAttack -= HandleSpecialMid;
        _special.groundSpecialAttack -= HandleSpecialEndWait;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void HandleSpecialAttack()
    {
        p_animetor.SetBool(_isSpecialParameter, true);
        p_animetor.SetFloat(_isSpecialMomentParameter, 0.0f);
    }

    private void HandleSpecialMid()
    {
        p_animetor.SetFloat(_isSpecialMomentParameter,0.5f);
    }

    private void HandleSpecialEndWait()
    {
        p_animetor.SetFloat(_isSpecialMomentParameter, 0.9f);
    }

    private void HandleSpecialEnded()
    {
        p_animetor.SetBool(_isSpecialParameter, false);
    }
}
