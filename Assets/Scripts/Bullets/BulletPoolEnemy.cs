using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolEnemy : BulletPool
{
    [SerializeField] private ActionChanel<Transform, Vector3> _shoot;

    private void OnEnable()
    {
        if (_shoot)
            _shoot.Sucription(HandleShoot);
    }

    private void OnDisable()
    {
        _shoot.Unsuscribe(HandleShoot);

        p_activeBullets.Clear();
        p_poolBullets.Clear();
    }

    private void HandleShoot(Transform pointShoot, Vector3 direction)
    {
        Bullet temp = SelectBullet(pointShoot);
        temp.transform.position = pointShoot.position;

        if (p_poolBullets.Contains(temp))
            p_poolBullets.Remove(temp);

        p_activeBullets.Add(temp);
        temp.Shoot(pointShoot.position, direction, p_speed);
    }
}