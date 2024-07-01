using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool _paused = false;

    [SerializeField] private NavigationManager _navManager;
    [SerializeField] private EmptyAction _pause;

    private void OnEnable()
    {
        _pause?.Sucription(HandlePaused);
    }

    private void OnDisable()
    {
        _pause?.Unsuscribe(HandlePaused);
    }

    private void HandlePaused()
    {
        _paused = !_paused;
        _navManager.PauseMoment(_paused);
        if (_paused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
