using InputSystem;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerMovementController : MonoBehaviour
    {
        public void Move(SwipeDirection swipe)
        {
            switch (swipe)
            {
                case SwipeDirection.Up:
                    Debug.Log("Move Up");
                    break;
                case SwipeDirection.Right:
                    Debug.Log("Move Right");
                    break;
                case SwipeDirection.Down:
                    Debug.Log("Move Down");
                    break;
                case SwipeDirection.Left:
                    Debug.Log("Move Left");
                    break;
            }
        }
    }
}

