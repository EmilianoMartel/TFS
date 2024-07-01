using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [Tooltip("First menu in the list is the default one :)")]
    [SerializeField] private List<MenuDataSource> menusWithId;

    [SerializeField] private GameManagerDataSource gameManagerDataSource;
    [SerializeField] private StringChannel _finalScene;

    private int _currentMenuIndex = 0;

    private void OnEnable()
    {
        _finalScene?.Sucription(HandleChangeMenu);
    }

    private void OnDisable()
    {
        _finalScene?.Unsuscribe(HandleChangeMenu);
    }

    private void Start()
    {
        foreach (var menu in menusWithId)
        {
            menu.Reference.Setup();
            menu.Reference.OnChangeMenu += HandleChangeMenu;
            menu.Reference.gameObject.SetActive(false);
        }

        if (menusWithId.Count > 0)
        {
            menusWithId[_currentMenuIndex].Reference.gameObject.SetActive(true);
        }
    }

    private void HandleChangeMenu(string id)
    {
        for (var i = 0; i < menusWithId.Count; i++)
        {
            var menuWithId = menusWithId[i];
            if (menuWithId.menuId == id)
            {
                menusWithId[_currentMenuIndex].Reference.gameObject.SetActive(false);
                menuWithId.Reference.gameObject.SetActive(true);
                _currentMenuIndex = i;
                break;
            }
        }
    }

    public void PauseMoment(bool isPaused)
    {
        if (isPaused)
            HandleChangeMenu("Pause");
        else
            HandleChangeMenu("Play");
    }
}