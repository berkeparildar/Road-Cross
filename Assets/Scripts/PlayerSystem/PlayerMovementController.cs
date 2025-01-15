using System;
using System.Threading.Tasks;
using DG.Tweening;
using Extensions;
using InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        public static UnityAction PlayerMoved;
        public static UnityAction PlayerMoving;
        private const int MAP_RIGHT_BOUNDS = 24;
        private const int MAP_LEFT_BOUNDS = -24;
        private const int STEP = 3;
        private bool _isMoving;

        private void Awake()
        {
            InputManager.PlayerSwiped += OnPlayerSwiped;
        }

        private void OnDestroy()
        {
            InputManager.PlayerSwiped -= OnPlayerSwiped;
        }

        private void OnPlayerSwiped(SwipeDirection swipe)
        {
            Move(swipe);
        }

        private async void Move(SwipeDirection swipe)
        {
            if (_isMoving) return;
            if (!CheckWorldBounds(swipe)) return;
            if (!CheckObstacle(swipe)) return;
            if (transform.parent is not null) transform.SetParent(null);
            PlayerMoving?.Invoke();
            _isMoving = true;
            var movementVector = InputExtensions.GetSwipeDirection(swipe);
            await MoveTween(movementVector * STEP);
            _isMoving = false;
            PlayerMoved?.Invoke();
        }

        private Task MoveTween(Vector3 targetPosition)
        {
            return transform.DOMove(targetPosition, 0.25f).SetRelative().AsyncWaitForCompletion();
        }

        private bool CheckWorldBounds(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Right:
                    if (transform.position.x < MAP_RIGHT_BOUNDS - STEP)
                    {
                        return true;
                    }
                    break;
                case SwipeDirection.Left:
                    if (transform.position.x > MAP_LEFT_BOUNDS + STEP)
                    {
                        return true;
                    }
                    break;
                case SwipeDirection.Up:
                    return true;
                case SwipeDirection.Down:
                    if (transform.position.z > STEP)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private bool CheckObstacle(SwipeDirection direction)
        {
            var vector = InputExtensions.GetSwipeDirection(direction);
            if (Physics.Raycast(transform.position, vector, out var hit, STEP))
            {
                if (hit.transform.gameObject.CompareTag("Tree"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

