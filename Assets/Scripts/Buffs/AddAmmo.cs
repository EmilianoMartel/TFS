using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : Buff
{
    [SerializeField] private int _addAmmo = 10;

    public override void DoSomething(PlayerController player)
    {
        player.AddAmmo(_addAmmo);
        gameObject.SetActive(false);
    }
}
