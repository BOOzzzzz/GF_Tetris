using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BOO
{
    public class PlayerInputManager:Singleton<PlayerInputManager>,PlayerInput.IGamePlayActions
    {
        private PlayerInput playerInput;

        public Action onUp;
        public Action onDown;
        public Action onStopDown;
        public Action onLeft;
        public Action onRight;
        public Action onPause;
        public Action onCancelPause;
        
        public bool isDown = false;

        public void OnInit()
        {
            playerInput = new PlayerInput();
            playerInput.GamePlay.SetCallbacks(this);
        }

        public void OnEnable()
        {
            playerInput.Enable();
        }

        public void OnDisable()
        {
            playerInput.Disable();
        }

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onUp?.Invoke();
            }
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isDown = true;
                onDown?.Invoke();
            }
            if (context.canceled)
            {
                isDown = false;
                onStopDown?.Invoke();
            }
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onLeft?.Invoke();
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onRight?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if(GameEntry.Base.IsGamePaused)
                    onCancelPause?.Invoke();
                else
                    onPause?.Invoke();
            }
        }
    }
}