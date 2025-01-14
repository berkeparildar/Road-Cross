using InputSystem;
using UnityEngine;

namespace Extensions
{
    public class InputExtensions
    {
        public static Vector3 GetSwipeDirection(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Up:
                    return Vector3.forward;
                case SwipeDirection.Right:
                    return Vector3.right;
                case SwipeDirection.Down:
                    return Vector3.back;
                case SwipeDirection.Left:
                    return Vector3.left;
            }
            return Vector3.zero;
        }
    }
}
