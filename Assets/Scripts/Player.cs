using System;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private int step;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool hasObstacleInDirection;
    [SerializeField] private GameObject model;
    [SerializeField] private bool atTop;
    [SerializeField] private bool jump;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private int rightBound;
    [SerializeField] private int leftBound;
    [SerializeField] private int jumpHeight;
    [SerializeField] private bool shouldFall;
    [SerializeField] private GameObject waterSplash;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grassSound;
    [SerializeField] private AudioClip roadSound;
    [SerializeField] private AudioClip logSound;
    [SerializeField] private AudioClip splashSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private GameObject riverSound;
    public static event Action<Vector3> OnPlayerMoved;
    public static event Action IsHit;
    [SerializeField] private bool gotInput;
    private static readonly int JumpTrigger = Animator.StringToHash("jump");
    private static readonly int Crash = Animator.StringToHash("crash");
    private static readonly int Squeeze = Animator.StringToHash("squeeze");
    [SerializeField] private Vector2 touchDownPosition;
    [SerializeField] private Vector2 touchUpPosition;


    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        FallDownToRiver();
        TouchControl();
        CheckObstacles();
        CheckPlayerBounds();
        MoveToPosition();
        JumpModel();
        StopCameraOutOfBounds();
    }

    private void TouchControl()
    {
        if (!isMoving && Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchDownPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                touchUpPosition = touch.position;
                CalculateDirection();
            }
        }
    }

    private void CalculateDirection()
    {
        var currentPosition = transform.position;
        OnPlayerMoved?.Invoke(currentPosition);
        if (touchUpPosition.x - touchDownPosition.x > 100)
        {
            direction = Vector3.right;
            gotInput = true;
            targetPosition = new Vector3(currentPosition.x + step, currentPosition.y, currentPosition.z);
            model.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (touchUpPosition.x - touchDownPosition.x < -100)
        {
            gotInput = true;
            direction = Vector3.left;
            targetPosition = new Vector3(currentPosition.x - step, currentPosition.y, currentPosition.z);
            model.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (touchUpPosition.y - touchDownPosition.y < -100)
        {
            direction = Vector3.back;
            gotInput = true;
            targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - step);
            model.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            direction = Vector3.forward;
            gotInput = true;
            targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + step);
            model.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void CheckObstacles()
    {
        if (!isMoving)
        {
            if (Physics.Raycast(transform.position, direction, out var hit, step))
            {
                if (hit.transform.gameObject.CompareTag("Tree"))
                {
                    hasObstacleInDirection = true;
                    gotInput = false;
                }
                else
                {
                    hasObstacleInDirection = false;
                }
            }
            else
            {
                hasObstacleInDirection = false;
            }
        }
    }

    private void MoveToPosition()
    {
        var currentPosition = transform.position;
        if (!hasObstacleInDirection && gotInput && !shouldFall)
        {
            isMoving = true;
            transform.SetParent(null);
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed
                * Time.deltaTime);
            if (transform.position == targetPosition)
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

    private void CheckPlayerBounds()
    {
        if (!isMoving)
        {
            if (transform.position.x + step >= rightBound && direction == Vector3.right)
            {
                hasObstacleInDirection = true;
            }

            if (transform.position.x - step <= leftBound && direction == Vector3.left)
            {
                hasObstacleInDirection = true;
            }

            if (transform.position.z <= step && direction == Vector3.back)
            {
                hasObstacleInDirection = true;
            }
        
            if (transform.position.x > rightBound + 1 || transform.position.x < leftBound - 1)
            {
                IsHit?.Invoke();
                moveSpeed = 0;
                enabled = false;
            }
            
            if (gotInput && !hasObstacleInDirection)
            {
                Jump();
            }
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

    private void PlaySound(int index)
    {
        switch (index)
        {
            case 0:
                audioSource.clip = grassSound;
                break;
            case 1:
                audioSource.clip = roadSound;
                break;
            case 2:
                audioSource.clip = logSound;
                break;
            case 3:
                audioSource.clip = splashSound;
                break;
            case 4:
                audioSource.clip = crashSound;
                break;
        }
        audioSource.Play();
    }
}