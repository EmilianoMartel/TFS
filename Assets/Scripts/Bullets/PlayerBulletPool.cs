using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletPool : BulletPool
{
    [SerializeField] private Transform _pointShoot;
    [SerializeField] private EmptyAction _shootMomentEvent;
    [SerializeField] private ActionChanel<Transform> _pointShootEvent;

    private void OnEnable()
    {
        if (_shootMomentEvent)
            _shootMomentEvent.Sucription(HandleShoot);

        if (_pointShootEvent)
            _pointShootEvent.Sucription(HandleSetShootPoint);
    }

    private void OnDisable()
    {
        _shootMomentEvent.Unsuscribe(HandleShoot);

        if (_pointShootEvent)
            _pointShootEvent.Unsuscribe(HandleSetShootPoint);

        p_activeBullets.Clear();
        p_poolBullets.Clear();
    }

    private void HandleSetShootPoint(Transform pointShoot)
    {
        _pointShoot = pointShoot;
    }

    private void HandleShoot()
    {
        Bullet temp = SelectBullet(_pointShoot);
        temp.transform.position = _pointShoot.position;

        if (p_poolBullets.Contains(temp))
            p_poolBullets.Remove(temp);

        p_activeBullets.Add(temp);
        temp.Shoot(_pointShoot.position, _pointShoot.forward, p_speed);
    }
}