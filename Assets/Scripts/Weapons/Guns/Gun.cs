using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : Weapon
{
    [Header("Gun Parameters")]
    [Tooltip("Max shoot distance.")]
    [SerializeField] private int _shootDistance;
    [Tooltip("The shoot start point.")]
    [SerializeField] private Transform _shootPoint;
    [Tooltip("Set if the gun is automatic shoot.")]
    [SerializeField] private bool _isAutomatic;
    [Tooltip("Time between shoots in RPM(round per minute).")]
    [SerializeField] private float _fireRate;
    [Tooltip("Ammunition cartridge size")]
    [SerializeField] private int _cartridgeSize = 50;
    [Tooltip("Total ammo. This count the catridge size plus the ammo left.")]
    [SerializeField] private int _maxAmmo;
    [Tooltip("The time it takes to reload the gun.")]
    [SerializeField] private float _timeReload;

    [Header("Optional parameters")]
    [SerializeField] private RecoilSO _recoilData;

    private bool _isShooting = false;
    private bool _canShoot = true;
    private bool _isReloaded;

    private float _timeBetweenShoot;
    private int _ammoLeft;
    private int _totalAmmoAmmountLeft;

    [Header("Channels")]
    [SerializeField] private EmptyAction _shootEvent;
    [SerializeField] private BoolChanelSo _viewEnemy;
    [SerializeField] private IntChannel _actualAmmoEvent;
    [SerializeField] private IntChannel _maxAmmoEvent;
    [SerializeField] private TransformChannelSo _pointShootEvent;
    [SerializeField] private IntChannel _damageValueEvent;
    [SerializeField] private EmptyAction _reloadEvent;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_maxAmmoEvent)
            _maxAmmoEvent.InvokeEvent(_totalAmmoAmmountLeft);

        if (_pointShootEvent)
            _pointShootEvent.InvokeEvent(_shootPoint);

        if (_actualAmmoEvent)
            _actualAmmoEvent.InvokeEvent(_ammoLeft);

        _reloadEvent?.Sucription(HandleReload);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _reloadEvent?.Unsuscribe(HandleReload);
    }

    private void Awake()
    {
        Validate();

        //This count is to have the time between shots.
        _timeBetweenShoot = 60 / _fireRate;
        _pointShootEvent?.InvokeEvent(_shootPoint);
        _ammoLeft = _cartridgeSize;
        _totalAmmoAmmountLeft = _maxAmmo - _cartridgeSize;
    }

    protected override void Start()
    {
        base.Start();

        if (_maxAmmoEvent)
            _maxAmmoEvent.InvokeEvent(_totalAmmoAmmountLeft);

        if (_actualAmmoEvent)
            _actualAmmoEvent.InvokeEvent(_ammoLeft);
    }

    private void Update()
    {
        if (p_isPressTrigger && !_isShooting && _canShoot && _ammoLeft > 0 && !_isReloaded)
        {
            StartCoroutine(Shoot());
        }

        RaycastHit hit;

        _viewEnemy?.InvokeEvent(Physics.Raycast(_shootPoint.position, _shootPoint.forward, out hit, _shootDistance, p_enemyMask));
    }

    protected override void HandleSetPressTrigger(bool pressTrigger)
    {
        p_isPressTrigger = pressTrigger;
        if (!_isAutomatic && !pressTrigger)
        {
            _canShoot = true;
        }
    }

    private IEnumerator Shoot()
    {
        _isShooting = true;
        _canShoot = false;

        if (_shootEvent)
            _shootEvent?.InvokeEvent();

        _ammoLeft--;

        if (_actualAmmoEvent)
            _actualAmmoEvent?.InvokeEvent(_ammoLeft);

        yield return new WaitForSeconds(_timeBetweenShoot);

        _canShoot = _isAutomatic;

        _isShooting = false;
    }

    private void HandleReload()
    {
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _isReloaded = true;

        yield return new WaitForSeconds(_timeReload);

        if(_totalAmmoAmmountLeft - _cartridgeSize <= 0)
        {
            _ammoLeft = _totalAmmoAmmountLeft;
            _totalAmmoAmmountLeft = 0;
        }
        else
        {
            _ammoLeft = _cartridgeSize;
            _totalAmmoAmmountLeft -= _cartridgeSize;
        }
        
        _actualAmmoEvent?.InvokeEvent(_ammoLeft);
        _maxAmmoEvent?.InvokeEvent(_totalAmmoAmmountLeft);

        _isReloaded = false;
    }

    public override void SendWeaponParameters()
    {
        if (_actualAmmoEvent)
            _actualAmmoEvent.InvokeEvent(_ammoLeft);
        if (_maxAmmoEvent)
            _maxAmmoEvent.InvokeEvent(_maxAmmo);
        if (_pointShootEvent)
            _pointShootEvent.InvokeEvent(_shootPoint);
        if (_damageValueEvent)
            _damageValueEvent.InvokeEvent(p_damage);
    }

    private void Validate()
    {
        if (_fireRate <= 0)
        {
            Debug.LogError($"{name}: Rate fire cannot be 0 or less.\nCheck and assigned a valid number.\nDisabled component.");
            enabled = false;
            return;
        }
        if (_maxAmmo <= 0)
        {
            Debug.LogError($"{name}: Max Ammo cannot be 0 or less.\nCheck and assigned a valid number.\nDisabled component.");
            enabled = false;
            return;
        }
        if (_timeReload <= 0)
        {
            Debug.LogError($"{name}: TimeReload cannot be 0 or less.\nCheck and assigned a valid number.\nDisabled component.");
            enabled = false;
            return;
        }
    }

    public void AddActualAmmoAmount(int cant)
    {
        _totalAmmoAmmountLeft += cant;
        _maxAmmoEvent?.InvokeEvent(_totalAmmoAmmountLeft);
    }
}