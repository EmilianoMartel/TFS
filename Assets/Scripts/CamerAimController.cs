using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Windows;
using StarterAssets;
using UnityEngine.InputSystem;

public class CamerAimController : MonoBehaviour
{
    public GameObject CinemachineCameraTarget;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    [SerializeField] private CinemachineVirtualCamera _aimVirtualCam;
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private BoolChanelSo _aimEvent;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

#if ENABLE_INPUT_SYSTEM
    [SerializeField] private PlayerInput _playerInput;
#endif

    [Header("Camera aim Limits")]
    [SerializeField] private float _minLateralLimit = -45f;
    [SerializeField] private float _maxLateralLimit = 45f;
    [SerializeField] private float _bottonClamp = -10;
    [SerializeField] private float _topClamp = 10;

    private const float _threshold = 0.01f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _originalYaw;
    private float _originalPitch;
    private Quaternion _lastCameraRotation;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    private void OnEnable()
    {
        _aimEvent?.Sucription(HandleAim);
    }

    private void OnDisable()
    {
        _aimEvent?.Unsuscribe(HandleAim);
    }

    private void Update()
    {
        CameraRotation();
    }

    private void HandleAim(bool isAiming)
    {
        if (isAiming)
        {
            _aimVirtualCam.gameObject.SetActive(true);
            _originalYaw = _cinemachineTargetYaw;
            _originalPitch = _cinemachineTargetPitch;
        }
        else
        {
            _aimVirtualCam.gameObject.SetActive(false);
            _cinemachineTargetYaw = _originalYaw;
            _cinemachineTargetPitch = _originalPitch;
        }
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        if (_input.aim)
        {
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, _originalYaw + _minLateralLimit, _originalYaw + _maxLateralLimit);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _originalPitch + _bottonClamp, _originalPitch + _topClamp);
        }
        else
        {
            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        }

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}