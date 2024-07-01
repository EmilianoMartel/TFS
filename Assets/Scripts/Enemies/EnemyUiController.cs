using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUiController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Image _imageHp;
    [SerializeField] private CameraSource _cameraSource;

    private void OnEnable()
    {
        _enemy.lifeChange += HandleChangeLife;
    }

    private void OnDisable()
    {
        _enemy.lifeChange -= HandleChangeLife;
    }

    private void Awake()
    {
        ValidateReference();
    }

    private void Update()
    {
        transform.LookAt(_cameraSource.Reference.transform.position);
    }

    private void HandleChangeLife(float actualLifeAmount)
    {
        _imageHp.fillAmount = actualLifeAmount;
    }

    private void ValidateReference()
    {
        if (!_cameraSource)
        {
            Debug.LogError($"{name}: Camera is null\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!_enemy)
        {
            Debug.LogError($"{name}: Enemy is null\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!_imageHp)
        {
            Debug.LogError($"{name}: Image is null\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}