using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpecialAttack : Bullet
{
    [SerializeField] private ParticleSystem _effect;

    public override void Shoot(Vector3 Position, Vector3 Direction, float Speed)
    {
        base.Shoot(Position,Direction, Speed);

        _effect.Play();
    }

    private void HandleAttack()
    {

    }

    private void ValidateClass()
    {
        if (!_effect)
        {
            Debug.LogError($"{name}: Effects is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}
