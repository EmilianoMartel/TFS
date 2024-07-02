using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] private GameObject _weapon1;
    [SerializeField] private GameObject _weapon2;

    [SerializeField] private EmptyAction _weapon1Event;
    [SerializeField] private EmptyAction _weapon2Event;

    private void OnEnable()
    {
        _weapon1Event.Sucription(HandleWeapon1);
        _weapon2Event.Sucription(HandleWeapon2);
    }

    private void OnDisable()
    {
        _weapon1Event.Unsuscribe(HandleWeapon1);
        _weapon2Event.Unsuscribe(HandleWeapon2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPickable newWeapon))
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IPickable newWeapon))
        {

        }
    }

    private void HandleWeapon1()
    {
        _weapon1.SetActive(true);
        _weapon2.SetActive(false);
    }

    private void HandleWeapon2()
    {
        _weapon1.SetActive(false);
        _weapon2.SetActive(true);
    }
}