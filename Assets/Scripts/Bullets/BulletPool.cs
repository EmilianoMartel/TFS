using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public abstract class BulletPool : MonoBehaviour
{
    [SerializeField] protected Bullet p_bulletPrefab;
    [SerializeField] protected float p_speed = 5f;
    [SerializeField] protected bool p_debugEnable = false;

    protected List<Bullet> p_activeBullets = new();
    protected List<Bullet> p_poolBullets = new();

    protected void OnGUI()
    {
        if (p_debugEnable)
        {
            GUI.Label(new Rect(10, 10, 200, 30), $"Total Pool Size: {p_activeBullets.Count + p_poolBullets.Count}");
            GUI.Label(new Rect(10, 30, 200, 30), $"Active Objects: {p_activeBullets.Count}");
        }
    }

    protected Bullet SelectBullet(Transform pointShoot)
    {
        if (p_poolBullets.Count == 0)
        {
            Bullet temp = Instantiate(p_bulletPrefab, pointShoot.position, Quaternion.identity);
            temp.onDisable += HandleDesactiveBullet;
            temp.transform.parent = transform;
            return temp;
        }
        p_poolBullets[0].onDisable += HandleDesactiveBullet;
        return p_poolBullets[0];
    }

    protected void HandleDesactiveBullet(Bullet bullet)
    {
        bullet.onDisable -= HandleDesactiveBullet;
        if (p_activeBullets.Contains(bullet))
        {
            p_activeBullets.Remove(bullet);
            p_poolBullets.Add(bullet);
        }
    }
}