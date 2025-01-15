using System;
using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerModelController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private Animator _animator;

        private static readonly int JumpTrigger = Animator.StringToHash("jump");
        private static readonly int CrashTrigger = Animator.StringToHash("crash");
        private static readonly int SqueezeTrigger = Animator.StringToHash("squeeze");

        private void Awake()
        {
            InputManager.PlayerSwiped += TurnModel;
            PlayerMovementController.PlayerMoving += OnPlayerMoving;
        }

        private void OnDestroy()
        {
            InputManager.PlayerSwiped -= TurnModel;
            PlayerMovementController.PlayerMoving -= OnPlayerMoving;
        }

        private void TurnModel(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Up:
                    _playerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case SwipeDirection.Right:
                    _playerModel.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    break;
                case SwipeDirection.Down:
                    _playerModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    break;
                case SwipeDirection.Left:
                    _playerModel.transform.localRotation = Quaternion.Euler(0, -90, 0);
                    break;
            }
        }

        private void OnPlayerMoving()
        {
            TriggerJumpAnimation();
        }

        private void TriggerJumpAnimation()
        {
            _animator.SetTrigger(JumpTrigger);
        }

        public void CarFrontContact()
        {
            _animator.SetTrigger(SqueezeTrigger);
        }

        public void CarSideContact()
        {
            _animator.SetTrigger(CrashTrigger);
        }
    }
}
