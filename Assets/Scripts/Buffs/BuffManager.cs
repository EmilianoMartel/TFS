using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private List<Buff> _buffPrefabs;
    [SerializeField] private TransformChannelSo _spawnBuffPositionEvent;
    private List<BuffType> _activeBuffs = new();
    private List<BuffType> _desactiveBuffs = new();

    struct BuffType
    {
        public string buffType;
        public List<Buff> buffList;

        public override bool Equals(object obj)
        {
            if (obj is BuffType other)
            {
                return buffType == other.buffType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return buffType.GetHashCode();
        }

        public static bool operator ==(BuffType lhs, BuffType rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(BuffType lhs, BuffType rhs)
        {
            return !lhs.Equals(rhs);
        }
    };


    private void OnEnable()
    {
        _spawnBuffPositionEvent?.Sucription(SpawnBuff);
    }

    private void OnDisable()
    {
        _spawnBuffPositionEvent?.Unsuscribe(SpawnBuff);
    }

    private void Awake()
    {
        StartList();
    }

    private void StartList()
    {
        BuffType temp;
        for (int i = 0; i < _buffPrefabs.Count; i++)
        {
            temp.buffType = _buffPrefabs[i].name;
            temp.buffList = new();
            _activeBuffs.Add(temp);
            _desactiveBuffs.Add(temp);
        }
    }

    public void SpawnBuff(Transform spawnPoint)
    {
        Buff temp = SelectBuff();
        temp.transform.position = spawnPoint.position;
        temp.transform.parent = transform;
    }

    private Buff SelectBuff()
    {
        int randomIndex = UnityEngine.Random.Range(0, _buffPrefabs.Count);
        for (int i = 0; i < _desactiveBuffs.Count; i++)
        {
            if (_desactiveBuffs[i].buffType == _buffPrefabs[randomIndex].name && _desactiveBuffs[i].buffList.Count > 0)
            {
                var temp = _desactiveBuffs[i].buffList[0];
                _desactiveBuffs[i].buffList.Remove(temp);
                _activeBuffs[i].buffList.Add(temp);
                temp.completedAction += DesactiveBuff;
                temp.gameObject.SetActive(true);
                return temp;
            }
        }

        Buff buffTemp = Instantiate(_buffPrefabs[randomIndex]);

        for (int i = 0; i < _activeBuffs.Count; i++)
        {
            if (_activeBuffs[i].buffType == _buffPrefabs[randomIndex].name)
            {
                _activeBuffs[i].buffList.Add(buffTemp);
                buffTemp.completedAction += DesactiveBuff;
                buffTemp.gameObject.SetActive(true);
                return buffTemp;
            }
        }

        return buffTemp;
    }

    private void DesactiveBuff(Buff buff)
    {
        for (int i = 0; i < _activeBuffs.Count; i++)
        {
            if (_activeBuffs[i].buffType == buff.name)
            {
                if (_activeBuffs[i].buffList.Contains(buff))
                    _activeBuffs[i].buffList.Remove(buff);
                _desactiveBuffs[i].buffList.Add(buff);
                buff.completedAction -= DesactiveBuff;
            }
        }
    }
}
