using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private MenuDataSource _selfDataSource;
    [SerializeField] private ButtonController _buttonPrefab;
    [SerializeField] private List<MenuDataSource> _ids = new();
    [SerializeField] private Transform _buttonsParent;

    public event Action<string> OnChangeMenu;

    private void Awake()
    {
        _selfDataSource.Reference = this;
        ValidateParameters();
    }

    public void Setup()
    {
        foreach (var id in _ids)
        {
            var newButton = Instantiate(_buttonPrefab, _buttonsParent);
            newButton.name = $"{id.menuId}_Btn";
            newButton.Setup(id.menuId, id.menuId, HandleButtonClick);
        }
    }

    private void HandleButtonClick(string id)
    {
        OnChangeMenu?.Invoke(id);
    }

    private void ValidateParameters()
    {
        if (!_buttonPrefab)
        {
            Debug.LogError($"{name}: Button Controller Prefab is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (!_buttonsParent)
        {
            Debug.LogError($"{name}: Button Parent is null.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (_ids.Count < 0)
        {
            Debug.LogError($"{name}: Ids list count cant´t be less 0.\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}
