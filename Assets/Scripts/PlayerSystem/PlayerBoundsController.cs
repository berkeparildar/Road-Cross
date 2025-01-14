using UnityEngine;

namespace PlayerSystem 
{
    public class PlayerBoundsController : MonoBehaviour
    {
        private const int MAP_RIGHT_BOUNDS = 24;
        private const int MAP_LEFT_BOUNDS = -24;
        private const int STEP = 3;

        public void CheckWorldBounds()
        {
            var currentPosition = transform.position;
            
            var isWithinRightBounds = currentPosition.x < MAP_RIGHT_BOUNDS - STEP;
            var isWithinLeftBounds = currentPosition.x > MAP_LEFT_BOUNDS + STEP;
            var isWithinBottomBounds = currentPosition.z > 
            if (transform.position.x < MAP_RIGHT_BOUNDS - STEP)
            {
                
            }
        }
    }
}