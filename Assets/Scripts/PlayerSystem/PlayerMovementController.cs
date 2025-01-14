using System.Threading.Tasks;
using DG.Tweening;
using Extensions;
using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        private const int STEP = 3;
        private bool _isMoving;

        public async Task Move(SwipeDirection swipe)
        {
            if (_isMoving) return;
            _isMoving = true;

            var movementVector = InputExtensions.GetSwipeDirection(swipe);
            await MoveTween(movementVector * STEP);
            _isMoving = false;
        }

        private Task MoveTween(Vector3 targetPosition)
        {
            return transform.DOMove(targetPosition, 0.5f).SetRelative().AsyncWaitForCompletion();
        }
    }
}

