using System;
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

        private void Awake()
        {
            PlayerMovementController.PlayerMoved += OnPlayerMoved;
        }

        private void OnDestroy()
        {
            PlayerMovementController.PlayerMoved -= OnPlayerMoved;
        }

        private void OnPlayerMoved()
        {
            PlaySurfaceSound();
        }

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

        private void PlaySurfaceSound()
        {
            if (!Physics.Raycast(transform.position, Vector3.down, out var hit, 1f)) return;
            switch (hit.transform.tag)
            {
                case "Grass":
                    PlaySurfaceSound(0);
                    break;
                case "Log":
                    PlaySurfaceSound(1);
                    transform.SetParent(hit.transform);
                    hit.transform.GetComponent<Log>().Sink();
                    break;
                case "Road":
                    PlaySurfaceSound(2);
                    break;
                case "Water":
                    PlaySurfaceSound(3);
                    break;
            }
        }

        public void PlayCarContactSound()
        {

        }
    }
}
