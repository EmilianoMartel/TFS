using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CheckConstrain : MonoBehaviour
{
    [SerializeField] private float _weight = 1f;
    [SerializeField] private MultiAimConstraint _constraint;
    [SerializeField] private WeaponTypeChannel _waponType;
    [SerializeField] private BoolChanelSo _aimEvent;

    private bool _canAim = true;

    private void OnEnable()
    {
        _aimEvent.Sucription(ChangeConstrait);
        _waponType.Sucription(HandleWeaponType);
    }

    private void OnDisable()
    {
        _aimEvent.Unsuscribe(ChangeConstrait);
        _waponType.Unsuscribe(HandleWeaponType);
    }

    private void ChangeConstrait(bool isAiming)
    {
        if (!_canAim)
            return;

        if (isAiming)
            _constraint.weight = _weight;
        else
            _constraint.weight = 0;
    }

    private void HandleWeaponType(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Meele:
                _canAim = false;
                break;
            case WeaponType.Rifle:
                _canAim = true;
                break;
            case WeaponType.Pistol:
                _canAim = true;
                break;
            default:
                _canAim = true;
                break;
        }
    }
}