using System;
using Extensions;
using InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
    public class PlayerBoundsController : MonoBehaviour
    {
        private const int MAP_RIGHT_BOUNDS = 24;
        private const int MAP_LEFT_BOUNDS = -24;
        private const int STEP = 3;

        public bool CheckWorldBounds(SwipeDirection direction)
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

        public bool CheckObstacle(SwipeDirection direction)
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
