using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private TMP_Text _ammoAmmountText;
    [SerializeField] private TMP_Text _totalAmmoText;

    [Header("CrossView")]
    [SerializeField] private Color _viewEnemyColor;
    [SerializeField] private Color _dontViewEnemyColor;

    [Header("Channels")]
    [SerializeField] private BoolChanelSo _isViewEnemy;
    [SerializeField] private ActionChanel<int> _actualAmmoEvent;
    [SerializeField] private ActionChanel<int> _maxAmmoEvent;

    private void OnEnable()
    {
        if (_actualAmmoEvent)
            _actualAmmoEvent.Sucription(HandleChangeAmmo);
        if (_maxAmmoEvent)
            _maxAmmoEvent.Sucription(HandleMaxAmmo);

        _isViewEnemy?.Sucription(HandleLookEnemy);
    }

    private void OnDisable()
    {
        if (_actualAmmoEvent)
            _actualAmmoEvent.Unsuscribe(HandleChangeAmmo);
        if (_maxAmmoEvent)
            _maxAmmoEvent.Unsuscribe(HandleMaxAmmo);

        _isViewEnemy?.Unsuscribe(HandleLookEnemy);
    }

    private void Awake()
    {
        if (_crossHair == null)
        {
            Debug.LogError($"{name}: CrossHair is null.\nCheck and assigned one.\nDisabled component.");
            enabled = false;
            return;
        }
        if (!_ammoAmmountText)
        {
            Debug.LogError($"{name}: AmmoText is null.\nCheck and assigned one.\nDisabled component.");
            enabled = false;
            return;
        }
    }

    private void HandleLookEnemy(bool look)
    {
        if (look)
        {
            _crossHair.color = _viewEnemyColor;
        }
        else
        {
            _crossHair.color = _dontViewEnemyColor;
        }
    }

    private void HandleChangeAmmo(int actualAmmo)
    {
        _ammoAmmountText.text = $"{actualAmmo}";
    }

    private void HandleMaxAmmo(int maxAmmo)
    {
        _totalAmmoText.text = $"{maxAmmo}";
    }
}