using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public Action<Buff> completedAction = delegate { };

    public virtual void DoSomething(PlayerController player)
    {
        completedAction?.Invoke(this);
    }

    public void ActiveBuff()
    {
        gameObject.SetActive(true);
    }
}