using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;

    [Header("Parameters")]
    [SerializeField] private string _speed = "Speed";
    [SerializeField] private string _grounded = "Grounded";
    [SerializeField] private string _jump = "Jump";
    [SerializeField] private string _freeFall = "FreeFall";
    [SerializeField] private string _motionSpeed = "MotionSpeed";
    [SerializeField] private string _fire = "Fire";

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDFire;

    private void Start()
    {
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash(_speed);
        _animIDGrounded = Animator.StringToHash(_grounded);
        _animIDJump = Animator.StringToHash(_jump);
        _animIDFreeFall = Animator.StringToHash(_freeFall);
        _animIDMotionSpeed = Animator.StringToHash(_motionSpeed);
        _animIDFire = Animator.StringToHash(_fire);
    }
}