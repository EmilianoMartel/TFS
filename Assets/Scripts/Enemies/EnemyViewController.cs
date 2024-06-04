using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    [Header("Parameters")]
    [SerializeField] private string _moveParameter = "onMove";
}