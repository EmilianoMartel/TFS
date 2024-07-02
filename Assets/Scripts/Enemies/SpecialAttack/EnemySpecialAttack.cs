using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttack : Enemy
{
    [SerializeField] protected AudioSource _specialAttackSound;
    [SerializeField] private float _specialAttackAnimationTime = 0.5f;
    [SerializeField] private float _specialAttackCd = 5f;
    [SerializeField] private SpecialSO _data;
    [SerializeField] private Transform _pointShoot;
    [SerializeField] private float _specialSpeed = 5f;
    [SerializeField] private SpecialAttack _specialBullet;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    private float _counter = 0;

    private bool _specialAttackStarted = false;

    private bool _isJumping = false;
    private Vector3 _jumpTarget;

    public event Action startSpecialAttack;
    public event Action middelSpecialAttack;
    public event Action groundSpecialAttack;
    public event Action endSpecialAttack;

    protected override void Update()
    {
        if (_specialAttackStarted)
            return;

        if ((p_target.position - transform.position).magnitude > p_attackDistance)
            Move();
        else
            AttackLogic();

        if (_counter > _specialAttackCd)
        {
            _counter = 0;
            StartCoroutine(SpecialAttack());
        }
        else
        {
            _counter += Time.deltaTime;
        }
    }

    private IEnumerator SpecialAttack()
    {
        p_agent.enabled = false;
        _specialAttackStarted = true;
        startSpecialAttack?.Invoke();
        _specialAttackSound.Play();
        yield return new WaitForSeconds(_data.startAnimationTime);

        _specialBullet.Shoot(_pointShoot.position, _pointShoot.forward, _specialSpeed);
        
        if (_data.jumpForce > 0)
            yield return StartCoroutine(Jump());

        yield return new WaitForSeconds(_data.startAnimationTime);

        endSpecialAttack?.Invoke();
        _specialAttackStarted = false;
        p_agent.enabled = true;
    }

    private IEnumerator Jump()
    {
        _isJumping = true;

        _jumpTarget = _data.AttackDistance(transform.position, p_target.position);


        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _data.jumpHeight, startPosition.z);

        float elapsedTime = 0f;
        while (elapsedTime < _data.jumpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / _data.jumpDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Simulate the fall
        elapsedTime = 0f;
        while (!GroundCheck())
        {
            transform.position = Vector3.Lerp(endPosition, new Vector3(_jumpTarget.x, startPosition.y, _jumpTarget.z), (elapsedTime / _data.jumpDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        groundSpecialAttack?.Invoke();

        yield return new WaitForSeconds(_data.startAnimationTime);

        _isJumping = false;
    }

    private bool GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (GroundCheck()) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }
}