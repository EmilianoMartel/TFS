using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SceneLevel
{
    [field: SerializeField] public string SceneName { get; private set; }
    [field: SerializeField] public bool IsUnloadable { get; private set; }
}