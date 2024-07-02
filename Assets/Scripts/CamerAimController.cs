using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Windows;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

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

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _cameraSensitivity = 10f;

    [Header("Camera aim Limits")]
    [SerializeField] private float _bottonClamp = -10;
    [SerializeField] private float _topClamp = 10;

    private const float _threshold = 0.01f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _originalYaw;
    private float _originalPitch;
    private Quaternion _lastCameraRotation;
    private float _verticalRotation = 0f;

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
        RotateHorizontally();
        RotateVertically();
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

    private void RotateHorizontally()
    {
        if (_input.look.x == 0f)
            return;

        float horizontalRotation = SmoothValue(_input.look.x);

        Vector3 targetRotation = _player.eulerAngles;
        targetRotation.y += horizontalRotation;

        _player.eulerAngles = targetRotation;
    }

    private float SmoothValue(float value)
    {
        return value * _cameraSensitivity * Time.deltaTime;
    }

    private void RotateVertically()
    {
        if (_input.look.y == 0f)
            return;

        Vector3 currentRotation = _cameraTarget.eulerAngles;
        _verticalRotation += SmoothValue(_input.look.y);

        _verticalRotation = Mathf.Clamp(_verticalRotation, _bottonClamp, _topClamp);
        currentRotation.x = -_verticalRotation;

        _cameraTarget.eulerAngles = currentRotation;
    }
}