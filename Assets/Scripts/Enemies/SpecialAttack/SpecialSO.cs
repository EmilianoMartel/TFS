using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialSO", fileName = "SpecialSO")]
public class SpecialSO : ScriptableObject
{
    [SerializeField] private float _attackMoveSpeed = 1f;
    [SerializeField] private float _jumpForce = 15;
    [SerializeField] private float _startAnimationTime = 1f;
    [SerializeField] private float _attackDistance = 5f;
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _jumpDuration = 1.0f;
    public float attackMoveSpeed { get { return _attackMoveSpeed; } }
    public float jumpForce { get { return _jumpForce; } }
    public float startAnimationTime { get { return _startAnimationTime; } }
    public float attackDistance { get { return _attackDistance; } }
    public float jumpHeight {get{ return _jumpHeight; }}
    public float jumpDuration { get { return _jumpDuration; } }

    public Vector3 AttackDistance(Vector3 star, Vector3 end)
    {
        if ((end - star).magnitude < _attackDistance)
            return end;
        else
            return (end - star).normalized * _attackDistance;
    }
}