using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyViewController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Animator _animetor;
    [SerializeField] protected NavMeshAgent p_agent;

    [Header("Parameters")]
    [SerializeField] private string _isAttackingParameter = "isAttacking";
    [SerializeField] private string _isMovingParameter = "isMoving";
    [SerializeField] private string _isDyingParameter = "isDying";

    private bool _isMoving => p_agent.speed > 0;

    private void OnEnable()
    {
        _enemy.onAttack += HandleOnAttack;
    }

    private void OnDisable()
    {
        _enemy.onAttack -= HandleOnAttack;
    }

    private void Update()
    {
        HandleOnMove(_isMoving);
    }

    private void HandleOnMove(bool isMoving)
    {
        _animetor.SetBool(_isMovingParameter, isMoving);
    }

    private void HandleOnAttack(bool isAttacking)
    {
        _animetor.SetBool(_isAttackingParameter, isAttacking);
    }

    private void HandleOnDie()
    {
        _animetor.SetBool(_isDyingParameter, true);
    }

    private IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        _animetor.SetBool(_isDyingParameter, false);
    }

    private void Validate()
    {
        if (!p_agent)
        {
            Debug.LogError($"{name}: AgentMesh is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!_enemy)
        {
            Debug.LogError($"{name}: Enemy is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!_animetor)
        {
            Debug.LogError($"{name}: Animator is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}