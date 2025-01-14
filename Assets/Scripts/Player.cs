using System;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{

   /*
    [SerializeField] private GameObject model;
    [SerializeField] private bool atTop;
    [SerializeField] private bool jump;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private int jumpHeight;
    [SerializeField] private bool shouldFall;
    [SerializeField] private GameObject waterSplash;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject riverSound;
    public static event Action<Vector3> OnPlayerMoved;
    public static event Action IsHit;
    [SerializeField] private bool gotInput;
    private static readonly int JumpTrigger = Animator.StringToHash("jump");
    private static readonly int Crash = Animator.StringToHash("crash");
    private static readonly int Squeeze = Animator.StringToHash("squeeze");

    private void Update()
    {
        FallDownToRiver();
        CheckPlayerBounds();
        MoveToPosition();
        JumpModel();
        StopCameraOutOfBounds();
    }

    private void MoveToPosition()
    {
        if (!hasObstacleInDirection && gotInput && !shouldFall)
        {
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 1f))
                {
                    switch (hit.transform.tag)
                    {
                        case "Grass":
                            PlaySound(0);
                            break;
                        case "Log":
                            PlaySound(1);
                            transform.SetParent(hit.transform);
                            hit.transform.GetComponent<Log>().Sink();
                            break;
                        case "Road":
                            PlaySound(2);
                            break;
                        case "Water":
                            PlaySound(3);
                            shouldFall = true;
                            IsHit?.Invoke();
                            break;
                    }

                    if (hit.transform.CompareTag("Log") || hit.transform.CompareTag("Water"))
                    {
                        riverSound.SetActive(true);
                    }
                    else
                    {
                        riverSound.SetActive(false);
                    }
                }
                isMoving = false;
                gotInput = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            var contactPoint = transform.position;
            var colliderCenter = other.bounds.center;
            var contactDifference = contactPoint - colliderCenter;
            if (Mathf.Abs(contactDifference.x) > Mathf.Abs(contactDifference.z))
            {
                animator.SetTrigger(Crash);
            }
            else
            {
                animator.SetTrigger(Squeeze);
                transform.SetParent(other.transform);
                virtualCamera.m_LookAt = null;
                virtualCamera.m_Follow = null;
            }
            moveSpeed = 0;
            PlaySound(4);
            IsHit?.Invoke();
            boxCollider.enabled = false;
            this.enabled = false;
        }
        else if (other.CompareTag("Water"))
        {
            waterSplash.SetActive(true);
            IsHit?.Invoke();
        }
    }

    private void OnEnable()
    {
        Log.IsSinking += IsSinking;
    }

    private void OnDisable()
    {
        Log.IsSinking -= IsSinking;
    }

    private void IsSinking(bool isSinking)
    {
        isMoving = isSinking;
    }

    private void JumpModel()
    {
        var currentPosition = model.transform.position;
        if (!jump) return;
        if (!atTop)
        {
            model.transform.position = Vector3.MoveTowards(currentPosition, new Vector3(currentPosition.x,
                jumpHeight, currentPosition.z), jumpSpeed * Time.deltaTime);
            if (!(currentPosition.y >= jumpHeight)) return;
            model.transform.position = new Vector3(currentPosition.x, jumpHeight, currentPosition.z);
            atTop = true;
        }
        else
        {
            model.transform.position = Vector3.MoveTowards(currentPosition, new Vector3(currentPosition.x,
                jumpHeight - jumpHeight, currentPosition.z), jumpSpeed * Time.deltaTime);
            if (!(currentPosition.y <= (jumpHeight - jumpHeight))) return;
            model.transform.position = new Vector3(currentPosition.x, jumpHeight - jumpHeight, currentPosition.z);
            jump = false;
            atTop = false;
        }
    }

    private void Jump()
    {
        animator.SetTrigger(JumpTrigger);
        jump = true;
        if (direction == Vector3.forward)
        {
            gameManager.IncreaseScore();
        }
        else if (direction == Vector3.back)
        {
            gameManager.DecreaseScore();
        }
    }

    private void StopCameraOutOfBounds()
    {
        if (transform.position.x > rightBound + 1 || transform.position.x < leftBound - 1)
        {
         StopCameras();
        }
    }

    private void FallDownToRiver()
    {
        if (shouldFall)
        {
            StopCameras();
            isMoving = true;
            gotInput = true;
            transform.Translate(Vector3.down * (jumpSpeed * Time.deltaTime));
        }
    }

    private void StopCameras()
    {
        virtualCamera.m_LookAt = null;
        virtualCamera.m_Follow = null;
    }
    */
}
