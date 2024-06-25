using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Animator))]
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
    [SerializeField] private string _xSpeed = "xSpeed";
    [SerializeField] private string _ySpeed = "ySpeed";
    [SerializeField] private string _isMeele = "isMeele";

    [Header("Events")]
    [SerializeField] protected WeaponTypeChannel _typeEvent;
    private Animator _animator;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDFire;
    private int _animXSpeed;
    private int _animYSpeed;

    private void OnEnable()
    {
        _controller.inputMagnitudeEvent += HanldeMovementSpeed;
        _controller.onMovement += HandleMovement;
        _controller.isFiring += HandleFire;
        _controller.onGround += HandleGrounded;
        _typeEvent?.Sucription(HandleWeaponType);
    }

    private void OnDisable()
    {
        _controller.inputMagnitudeEvent -= HanldeMovementSpeed;
        _controller.onMovement -= HandleMovement;
        _controller.isFiring -= HandleFire;
        _controller.onGround -= HandleGrounded;
        _typeEvent?.Unsuscribe(HandleWeaponType);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        _animXSpeed = Animator.StringToHash(_xSpeed);
        _animYSpeed = Animator.StringToHash(_ySpeed);
    }

    private void HandleGrounded(bool grounded)
    {
        _animator.SetBool(_animIDGrounded, grounded);
    }

    private void HandleAdminSpeed(float animSpeed)
    {
        _animator.SetFloat(_animIDSpeed, animSpeed);
    }

    private void HanldeMovementSpeed(float inputMagnitude)
    {
        _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

    private void HandleMovement(float xSpeed, float ySpeed)
    {
        _animator.SetFloat(_animXSpeed, xSpeed);
        _animator.SetFloat(_animYSpeed, ySpeed);
    }

    private void HandleFire(bool fire)
    {
        _animator.SetBool(_animIDFire, fire);
    }

    private void HandleJump(bool jump)
    {
        _animator.SetBool(_animIDJump, jump);
    }

    private void HandleFalling(bool fall)
    {
        _animator.SetBool(_animIDFreeFall, fall);
    }

    private void HandleWeaponType(WeaponType type)
    {
        if(type == WeaponType.Meele)
            _animator.SetBool(_isMeele, true);
        else
            _animator.SetBool(_isMeele, false);
    }
}