using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyViewController : MonoBehaviour
{
    [SerializeField] protected Enemy p_enemy;
    [SerializeField] protected Animator p_animetor;
    [SerializeField] protected NavMeshAgent p_agent;

    [Header("Parameters")]
    [SerializeField] private string _isAttackingParameter = "isAttacking";
    [SerializeField] private string _isMovingParameter = "isMoving";
    [SerializeField] private string _isDyingParameter = "isDying";

    private bool _isMoving => p_agent.speed > 0;

    protected virtual void OnEnable()
    {
        p_enemy.onAttack += HandleOnAttack;
        p_enemy.onDead += HandleOnDie;
    }

    protected virtual void OnDisable()
    {
        p_enemy.onAttack -= HandleOnAttack;
        p_enemy.onDead -= HandleOnDie;
    }

    protected virtual void Update()
    {
        HandleOnMove(_isMoving);
    }

    private void HandleOnMove(bool isMoving)
    {
        p_animetor.SetBool(_isMovingParameter, isMoving);
    }

    private void HandleOnAttack(bool isAttacking)
    {
        p_animetor.SetBool(_isAttackingParameter, isAttacking);
    }

    private void HandleOnDie()
    {
        p_animetor.SetBool(_isDyingParameter, true);
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        p_animetor.SetBool(_isDyingParameter, false);
    }

    private void Validate()
    {
        if (!p_agent)
        {
            Debug.LogError($"{name}: AgentMesh is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!p_enemy)
        {
            Debug.LogError($"{name}: Enemy is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!p_animetor)
        {
            Debug.LogError($"{name}: Animator is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}