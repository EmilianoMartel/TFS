using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddLife : Buff
{
    [SerializeField] private int _health = 10;

    public override void DoSomething(PlayerController player)
    {
        player.Health(_health);
        gameObject.SetActive(false);
    }
}
