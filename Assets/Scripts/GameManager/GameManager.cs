using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string playId = "Play";
    [SerializeField] private string exitId = "Exit";
    [Header("Levels")]
    [SerializeField] private List<LevelsContainer> levels;
    [Header("Data Sources")]
    [SerializeField] private DataSource<GameManager> _gameManagerDataSource;
    [SerializeField] private DataSource<SceneryManager> _sceneryManagerDataSource;
    [Header("Event Channels")]
    [SerializeField] private EmptyAction _endLevel;
    [SerializeField] private BoolChanelSo _finalGame;
    private int _currentLevel = 0;

    private void OnEnable()
    {
        if (_gameManagerDataSource != null)
            _gameManagerDataSource.Reference = this;
        if (_endLevel != null)
            _endLevel.Sucription(HandleNextLevel);
    }

    private void OnDisable()
    {
        if (_gameManagerDataSource != null && _gameManagerDataSource.Reference == this)
        {
            _gameManagerDataSource.Reference = null;
        }
        if (_endLevel != null)
            _endLevel.Unsuscribe(HandleNextLevel);
    }

    public bool HandleSpecialEvents(string id)
    {
        if (id == playId)
        {
            GameStart();
            return true;
        }
        else if (id == exitId)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
            return true;
        }
        return false;
    }

    private void GameStart()
    {
        if (_sceneryManagerDataSource != null && _sceneryManagerDataSource.Reference != null)
        {
            _sceneryManagerDataSource.Reference.ChangeLevel(levels[_currentLevel].levels);
        }
        _currentLevel++;
    }

    private void HandleNextLevel()
    {
        if (_currentLevel >= levels.Count)
        {
            _finalGame?.InvokeEvent(true);
            return;
        }

        GameStart();
    }
}
