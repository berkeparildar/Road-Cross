using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerSystem
{
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _grassSound;
        [SerializeField] private AudioClip _roadSound;
        [SerializeField] private AudioClip _logSound;
        [SerializeField] private AudioClip _splashSound;
        [SerializeField] private AudioClip _crashSound;

        private void PlaySurfaceSound(int index)
        {
            switch (index)
            {
                case 0:
                    _audioSource.clip = _grassSound;
                    break;
                case 1:
                    _audioSource.clip = _roadSound;
                    break;
                case 2:
                    _audioSource.clip = _logSound;
                    break;
                case 3:
                    _audioSource.clip = _splashSound;
                    break;
                case 4:
                    _audioSource.clip = _crashSound;
                    break;
            }
            _audioSource.Play();
        }
    }
}
