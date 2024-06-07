using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackManager : MonoBehaviour
{
    [SerializeField] private EnemySpecialAttack _enemy;
    [SerializeField] private SpecialAttack specialAttack;

    private void OnEnable()
    {
        _enemy.startSpecialAttack += HandleSpecialAttack;
    }

    private void OnDisable()
    {
        _enemy.startSpecialAttack -= HandleSpecialAttack;
    }

    private void HandleSpecialAttack()
    {
        
    }

}