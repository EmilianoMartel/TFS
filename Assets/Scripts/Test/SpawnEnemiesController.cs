using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesController : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyList = new List<Enemy>();
    private List<EnemyType> _activeEnemies = new();
    private List<EnemyType> _desactiveEnemies = new();

    struct EnemyType
    {
        public string enemyType;
        public List<Enemy> enemyList;

        public EnemyType(string type)
        {
            enemyType = type;
            enemyList = new();
        }

        public override bool Equals(object obj)
        {
            if (obj is EnemyType other)
            {
                return enemyType == other.enemyType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return enemyType.GetHashCode();
        }

        public static bool operator ==(EnemyType lhs, EnemyType rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(EnemyType lhs, EnemyType rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    private void Start()
    {
        StartList();
    }

    private void StartList()
    {
        foreach (var enemy in _enemyList)
        {
            _activeEnemies.Add(new EnemyType(enemy.name));
            _desactiveEnemies.Add(new EnemyType(enemy.name));
        }
    }

    public Enemy SpawnEnemy(Transform generatorPosition)
    {
        Enemy enemy = SelectEnemy();
        enemy.SetTarget(generatorPosition);
        return enemy;
    }

    private Enemy SelectEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemyList.Count);
        string randomEnemyName = _enemyList[randomIndex].name;

        for (int i = 0; i < _desactiveEnemies.Count; i++)
        {
            if (_desactiveEnemies[i].enemyType == randomEnemyName && _desactiveEnemies[i].enemyList.Count > 0)
            {
                var temp = _desactiveEnemies[i].enemyList[0];
                _desactiveEnemies[i].enemyList.Remove(temp);
                _activeEnemies[i].enemyList.Add(temp);
                temp.onDead += DesactiveEnemy;
                temp.transform.position = transform.position;
                temp.transform.parent = transform;
                temp.gameObject.SetActive(true);
                return temp;
            }
        }

        Enemy enemyTemp = Instantiate(_enemyList[randomIndex]);
        enemyTemp.onDead += DesactiveEnemy;
        enemyTemp.transform.parent = transform;
        enemyTemp.transform.position = transform.position;
        enemyTemp.gameObject.SetActive(true);

        for (int i = 0; i < _activeEnemies.Count; i++)
        {
            if (_activeEnemies[i].enemyType == randomEnemyName)
            {
                _activeEnemies[i].enemyList.Add(enemyTemp);
                break;
            }
        }

        return enemyTemp;
    }

    private void DesactiveEnemy(Enemy enemy)
    {
        for (int i = 0; i < _activeEnemies.Count; i++)
        {
            if (_activeEnemies[i].enemyType == enemy.name)
            {
                if (_activeEnemies[i].enemyList.Contains(enemy))
                    _activeEnemies[i].enemyList.Remove(enemy);
                _desactiveEnemies[i].enemyList.Add(enemy);
                enemy.onDead -= DesactiveEnemy;
            }
        }
    }
}