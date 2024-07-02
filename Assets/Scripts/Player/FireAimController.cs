using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAimController : MonoBehaviour
{
    [SerializeField] private Transform _bone;
    [SerializeField] private float _angle = 0;

    [SerializeField] private ActionChanel<bool> _fireEvent;

    Quaternion _lastQuat;
    Quaternion newQuat;

    private void OnEnable()
    {
        _fireEvent?.Sucription(HandleFire);
    }

    private void OnDisable()
    {
        _fireEvent?.Unsuscribe(HandleFire);
    }

    private void Awake()
    {
        newQuat = _bone.localRotation;
    }

    private void LateUpdate()
    {
        _bone.localRotation = newQuat;
    }

    private void HandleFire(bool isFiring)
    {
        if (isFiring)
        {
            _lastQuat = _bone.localRotation;
            newQuat = _bone.localRotation;
            newQuat.y = _angle;

            _bone.localRotation = newQuat;
        }
        else
        {
            newQuat = _lastQuat;

            _bone.localRotation = newQuat;
        }
        
    }
}