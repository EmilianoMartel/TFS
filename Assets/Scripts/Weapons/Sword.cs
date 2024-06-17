using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private float _attackCd = 1f;
    private bool _canAttack = true;

    private void Update()
    {
        if (p_isPressTrigger && _canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _canAttack = false;
        RaycastHit hit;

        if (Physics.Raycast(p_pointAttack.position, p_pointAttack.TransformDirection(Vector3.forward), out hit, p_attackDistance, p_enemyMask))
        {
            if (hit.collider.TryGetComponent(out IHealth hp))
                hp.TakeDamage(p_damage);
        }

        yield return new WaitForSeconds(_attackCd);

        _canAttack = true;
    }
}
