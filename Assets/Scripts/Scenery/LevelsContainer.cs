using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsContainer", menuName = "LevelsContainer")]
public class LevelsContainer : ScriptableObject
{
    [SerializeField] private List<ScenaryContainer> _levels = new();

    public List<ScenaryContainer> levels { get { return _levels; } }
}
