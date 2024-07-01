using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,IHealth
{
    [Header("Parameters")]
    [SerializeField] private int _maxLife = 100;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private Gun _gun;
    private int _actualLife;

    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private Quaternion _characterTargetRot;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
    [SerializeField] private PlayerInput _playerInput;
#endif
    private CharacterController _controller;
    [SerializeField] private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    [SerializeField] private BoolChanelSo _aimEvent;
    [SerializeField] private EmptyAction _playerDeadEvent;
    [SerializeField] private FloatChannel _damagedEvent;

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

    public event Action<bool> onGround;
    public event Action<float> inputMagnitudeEvent;
    public event Action<float, float> onMovement;
    public event Action<bool> isFiring;

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        _actualLife = _maxLife;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        //_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        //_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        _characterTargetRot = transform.localRotation;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        HandleRotateMeshInput(_input.look);
        Move();
        isFiring?.Invoke(_input.fire);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        onGround?.Invoke(Grounded);
    }

    //TO-DO
    private void HandleRotateMeshInput(Vector2 look)
    {
        if (_input.aim)
            return;

        float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

        float yRot = look.x * deltaTimeMultiplier;
        _characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);

        transform.rotation = _characterTargetRot;
    }

    private void Move()
    {
        float targetSpeed = SetSpeed();

        _animationBlend = Mathf.Lerp(_animationBlend, _speed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        SetTargetRotation();

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        //TO-DO
        var xSpeed = _input.move.x * targetSpeed;
        var ySpeed = _input.move.y * targetSpeed;

        inputMagnitudeEvent?.Invoke(inputMagnitude);
        onMovement?.Invoke(xSpeed, ySpeed);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);


                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {

                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }

    private void SetTargetRotation()
    {
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
        }
    }

    private float SetSpeed()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        return targetSpeed;
    }

    public void TakeDamage(int damage)
    {
        _actualLife -= damage;
        _damagedEvent?.InvokeEvent((float)_actualLife / _maxLife);
        if (_actualLife < 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        StartCoroutine(WaitForDead());
    }

    void IHealth.BasicDamage()
    {
        throw new NotImplementedException();
    }

    void IHealth.TakeTotalDamage()
    {
        throw new NotImplementedException();
    }

    private IEnumerator WaitForDead()
    {
        _playerDeadEvent?.InvokeEvent();
        yield return new WaitForSeconds(0.1f);
        transform.position = _respawnPoint.position;
        _actualLife = _maxLife;
        _damagedEvent?.InvokeEvent((float)_actualLife / (float)_maxLife);
    }

    public void SuscribeAction(Action<PlayerController> action)
    {
        throw new NotImplementedException();
    }

    public void Unsuscribe(Action<PlayerController> action)
    {
        throw new NotImplementedException();
    }

    void IHealth.SuscribeAction(Action action)
    {
        throw new NotImplementedException();
    }

    void IHealth.Unsuscribe(Action action)
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Buff buff))
            buff.DoSomething(this);
    }

    public void Health(int hp)
    {
        _actualLife += hp;
        if (_actualLife > _maxLife)
            _actualLife = _maxLife;

        _damagedEvent?.InvokeEvent((float)_actualLife / (float)_maxLife);
    }

    public void AddAmmo(int cant)
    {
        if (_gun != null)
            _gun.AddActualAmmoAmount(cant);
    }
}