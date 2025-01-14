using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public static UnityAction<SwipeDirection> OnSwipe;
        private InputController _controller;
        private Vector2 _swipeDirection;
        
        [SerializeField] private float minSwipeDistance;

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
            if (_swipeDirection.magnitude < minSwipeDistance) return;

            if (_swipeDirection.x > 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Right);
            }
            else if (_swipeDirection.x < 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Left);
            }
            else if (_swipeDirection.y > 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Up);
            }
            else if (_swipeDirection.y < 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Down);
            }
        }
    }
}