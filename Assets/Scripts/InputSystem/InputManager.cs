using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public static UnityAction<SwipeDirection> PlayerSwiped;
        private InputController _controller;
        private Vector2 _swipeDirection;

        [SerializeField] private float _minSwipeDistance;

        private void Awake()
        {
            _controller = new InputController();
            _controller.Player.Enable();
            _controller.Player.Touch.canceled += ProcessTouchComplete;
            _controller.Player.Swipe.performed += ProcessSwipeDelta;
        }

        private void ProcessSwipeDelta(InputAction.CallbackContext context)
        {
            _swipeDirection = context.ReadValue<Vector2>();
        }

        private void ProcessTouchComplete(InputAction.CallbackContext context)
        {
            if (_swipeDirection.magnitude < _minSwipeDistance) return;

            if (Mathf.Abs(_swipeDirection.x) > Mathf.Abs(_swipeDirection.y))
            {
                if (_swipeDirection.x > 0)
                {
                    PlayerSwiped?.Invoke(SwipeDirection.Right);
                }
                else
                {
                    PlayerSwiped?.Invoke(SwipeDirection.Left);
                }
            }
            else
            {
                if (_swipeDirection.y > 0)
                {
                    PlayerSwiped?.Invoke(SwipeDirection.Up);
                }
                else
                {
                    PlayerSwiped?.Invoke(SwipeDirection.Down);
                }
            }
        }

        private void OnDestroy()
        {
            _controller.Player.Touch.canceled -= ProcessTouchComplete;
            _controller.Player.Swipe.performed -= ProcessSwipeDelta;
            _controller.Player.Disable();
        }
    }
}
