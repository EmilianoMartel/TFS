using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraSource _cameraSource;

    private void Awake()
    {
        _cameraSource.Reference = _camera;
    }
}
