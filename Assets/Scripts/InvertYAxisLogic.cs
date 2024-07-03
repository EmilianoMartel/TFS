using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertYAxisLogic : MonoBehaviour
{
    [SerializeField] private string _boolStringPrefab = "Yaxis";
    [SerializeField] private Toggle _toogle;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(_boolStringPrefab))
            LoadBool();
        else
            SetBool();
    }

    public void SetBool()
    {
        bool isOn = _toogle.isOn;
        if (isOn)
            PlayerPrefs.SetInt(_boolStringPrefab, 1);
        else
            PlayerPrefs.SetInt(_boolStringPrefab, -1);
    }

    private void LoadBool()
    {
        int isOn = PlayerPrefs.GetInt(_boolStringPrefab);

        if (isOn == -1)
            _toogle.isOn = false;
        else 
            _toogle.isOn = true;

        SetBool();
    }
}