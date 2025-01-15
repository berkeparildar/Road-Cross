using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerSystem
{
    public class PlayerContactController : MonoBehaviour
    {
        [SerializeField] private GameObject _waterSplash;
        public static UnityAction HitByCarFront;
        public static UnityAction HitByCarSide;

        private void OnTriggerEnter(Collider other)
        {

        }
    }
}
