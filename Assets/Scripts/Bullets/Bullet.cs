using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IHazard
{
    [Header("Parameters")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifeTime = 10f;
    [Header("Visual parameters")]
    [SerializeField] private ParticleSystem _impactSystem;
    [SerializeField] private IObjectPool<TrailRenderer> _trailPool;

    [SerializeField] private bool _enabledDebug = false;
    [SerializeField] protected Rigidbody p_rigidbody;
    public Action<Bullet> onDisable;

    public event Action<bool> onDie = delegate { };

    private void OnDisable()
    {
        onDisable?.Invoke(this);
    }

    private void Awake()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            p_rigidbody = rb;
        }
        else
        {
            Debug.LogError($"{name}: Rigidbody is null.\nCheck and assigned component.\nDisabling component.");
            enabled = false;
            return;
        }
    }

    public virtual void Shoot(Vector3 Position, Vector3 Direction, float Speed)
    {
        ActiveBullet();
        p_rigidbody.velocity = Vector3.zero;
        transform.position = Position;
        transform.forward = Direction;

        p_rigidbody.AddForce(Direction * Speed, ForceMode.VelocityChange);
        StartCoroutine(WaitForDieLogic());
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(_impactSystem  != null)
        {
            _impactSystem.transform.forward = -1 * transform.forward;
            _impactSystem.Play();
        }
        
        p_rigidbody.velocity = Vector3.zero;

        if(other.TryGetComponent<IHealth>(out var hp))
        {
            hp.TakeDamage(ReturnDamage());
            if(_enabledDebug) Debug.Log($"{name}: damage to {other.name}");
        }

        HandleDie();
    }

    private void HandleDie()
    {
        p_rigidbody.AddForce(Vector3.zero,ForceMode.Force);
        gameObject.SetActive(false);
    }

    private void ActiveBullet()
    {
        this.gameObject.SetActive(true);
    }

    private IEnumerator WaitForDieLogic()
    {
        yield return new WaitForSeconds(_lifeTime);
        HandleDie();
    }

    public int ReturnDamage()
    {
        return _damage;
    }
}