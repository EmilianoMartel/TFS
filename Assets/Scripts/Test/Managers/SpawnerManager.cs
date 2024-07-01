using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private int _waveTime = 10;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _enemyPerWave = 10;
    [SerializeField] private Generator _generatorLife;
    [SerializeField] private Transform _generatorPosition;
    [SerializeField] private List<SpawnEnemiesController> _spawnList = new List<SpawnEnemiesController>();
    [SerializeField] private DataSource<GameManager> _gameManagerDataSource;
    [SerializeField] private float _timeBetweenSpawns = 1;

    private List<Enemy> _enemyList = new();
    private int _actualWave = 0;
    private int _enemiesDie = 0;

    private void OnEnable()
    {
        _generatorLife.dead += HandleGeneratorDie;
    }

    private void OnDisable()
    {
        _generatorLife.dead -= HandleGeneratorDie;
    }

    private void Awake()
    {
        Validate();
    }

    private void Start()
    {
        StartCoroutine(WaveStart());
    }

    private IEnumerator WaveStart()
    {
        _actualWave++;
        yield return new WaitForSeconds(_waveTime);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < _enemyPerWave; i++)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            int spawnIndex = RandomIndexSpawn();

            Enemy enemy = _spawnList[spawnIndex].SpawnEnemy(_generatorPosition);
            enemy.onDead += HandleEnemiesDie;

            _enemyList.Add(enemy);
        }
    }

    private void HandleEnemiesDie(Enemy enemy)
    {
        if (_enemyList.Contains(enemy))
        {
            enemy.onDead -= HandleEnemiesDie;
            _enemyList.Remove(enemy);
        }
        
        _enemiesDie++;

        if (_enemiesDie >= _enemyPerWave)
        {
            NextWaveCheck();
        }
    }

    private void NextWaveCheck()
    {
        _enemiesDie = 0;
        if (_actualWave >= _waveCount)
        {
            _gameManagerDataSource.Reference.HandleNextLevel();
        }
        else
        {
            StartCoroutine(WaveStart());
        }
    }

    private void HandleGeneratorDie()
    {
        _gameManagerDataSource.Reference.HandleLoose();
    }

    private int RandomIndexSpawn()
    {
        return UnityEngine.Random.Range(0, _spawnList.Count);
    }

    private void Validate()
    {
        if (!_generatorPosition)
        {
            Debug.LogError($"{name}: Generator position is null\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
        if (_spawnList.Count == 0)
        {
            Debug.LogError($"{name}: Spawn is null\nCheck and assigned one.\nDisabling component.");
            enabled = false;
            return;
        }
    }
}