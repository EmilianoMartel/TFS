using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesController : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyList = new List<Enemy>();
    private List<EnemyType> _activeEnemies = new();
    private List<EnemyType> _desactiveEnemies = new();

    struct EnemyType {
        public string enemyType;
        public List<Enemy> enemyList;
    };

    private void StartList()
    {
        EnemyType temp;
        for (int i = 0; i < _enemyList.Count; i++)
        {
            temp.enemyType = _enemyList[i].name;
            temp.enemyList = new();
            _activeEnemies.Add(temp);
            _desactiveEnemies.Add(temp);
        }
        
    }

    public Enemy SpawnEnemy(Transform generatorPosition)
    {
        Enemy enemy = Instantiate(_enemyList[0], transform.position, Quaternion.identity);
        //enemy.SetTarget(generatorPosition);
        return enemy;
    }

    //private Enemy SelectEnemy()
    //{
    //    int randomIndex = UnityEngine.Random.Range(0, _enemyList.Count);
    //    if (_desactiveEnemies.Contains(_enemyList[randomIndex]))
    //    {
    //
    //    }
    //}
    //
    //private bool HaveDesactiveEnemy(List<Enemy> listEnemy,out Enemy enemy)
    //{
    //
    //}
}