using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuSource", menuName = "DataSource/MenuSource")]
public class MenuDataSource : DataSource<Menu>
{
    [SerializeField] private string _menuId;
    public string menuId { get { return _menuId; } }
}