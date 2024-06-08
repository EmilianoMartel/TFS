using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUiController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Image _imageHp;

    private void OnEnable()
    {
        _enemy.lifeChange += HandleChangeLife;
    }

    private void OnDisable()
    {
        _enemy.lifeChange -= HandleChangeLife;
    }

    private void Update()
    {
        transform.LookAt(_camera.transform.position);
    }

    private void HandleChangeLife(float actualLifeAmount)
    {
        _imageHp.fillAmount = actualLifeAmount;
    }
}