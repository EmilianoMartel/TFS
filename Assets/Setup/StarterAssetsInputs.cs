using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool fire;
		public bool paused;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Channels")]
        [SerializeField] private BoolChanelSo _isTriggerEvent;
		[SerializeField] private EmptyAction _onReloadEvent;
		[SerializeField] private BoolChanelSo _aimEvent;
		[SerializeField] private EmptyAction _weapon1;
        [SerializeField] private EmptyAction _weapon2;
		[SerializeField] private EmptyAction _pauseEvent;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
            if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
            JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
            SprintInput(value.isPressed);
		}

        public void OnFire(InputValue value)
        {
            FireInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
			if(value.isPressed)
				ReloadInput();
        }

		public void OnWeapon1(InputValue value)
		{
            if (value.isPressed)
                Weapon1Input();
        }

        public void OnWeapon2(InputValue value)
        {
            if (value.isPressed)
                Weapon2Input();
        }

        public void OnPause(InputValue value)
        {
            if (value.isPressed)
                PauseInput();
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void AimInput(bool newSprintState)
        {
            aim = newSprintState;
            _aimEvent?.InvokeEvent(newSprintState);
        }

        public void FireInput(bool newSprintState)
        {
            fire = newSprintState;
			_isTriggerEvent?.InvokeEvent(newSprintState);
        }

        public void PauseInput()
        {
			paused = !paused;
            _pauseEvent?.InvokeEvent();
        }

        public void ReloadInput()
		{
            _onReloadEvent?.InvokeEvent();
        }

        public void Weapon1Input()
        {
            _weapon1?.InvokeEvent();
        }

        public void Weapon2Input()
        {
            _weapon2?.InvokeEvent();
        }

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}