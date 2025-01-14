using System;
using System.Threading.Tasks;
using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerBoundsController _boundsController;

        private void Awake()
        {
            InputManager.OnSwipe += MovePlayer;
        }

        private void OnDestroy()
        {
            InputManager.OnSwipe -= MovePlayer;
        }

        private async void MovePlayer(SwipeDirection direction)
        {
            try
            {
                var validMove = _boundsController.CheckWorldBounds(direction) &&
                                _boundsController.CheckObstacle(direction);

                if (validMove)
                {
                    await _movementController.Move(direction);
                }
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
    }
}
