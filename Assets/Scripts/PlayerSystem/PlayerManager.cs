using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _playerMovementController;
        
        private void Awake()
        {
            InputManager.OnSwipe += MovePlayer;
        }

        private void OnDestroy()
        {
            InputManager.OnSwipe -= MovePlayer;
        }

        private void MovePlayer(SwipeDirection direction)
        {
            _playerMovementController.Move(direction);
        }
    }
}