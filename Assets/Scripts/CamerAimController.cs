using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamerAimController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _aimVirtualCam;
    [SerializeField] private BoolChanelSo _aimEvent;

    private void OnEnable()
    {
        _aimEvent?.Sucription(HandleAim);
    }

    private void OnDisable()
    {
        _aimEvent?.Unsuscribe(HandleAim);
    }

    private void HandleAim(bool isAiming)
    {
        _aimVirtualCam.gameObject.SetActive(isAiming);
    }
}
