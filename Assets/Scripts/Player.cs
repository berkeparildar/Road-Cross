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
    [SerializeField] private int rightBound;
    [SerializeField] private int leftBound;
    [SerializeField] private int jumpHeight;
    public static event Action<Vector3> OnPlayerMoved;
    public static event Action IsHit;
    [SerializeField] private bool gotInput;
    private static readonly int Jump = Animator.StringToHash("jump");


    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        GetInput();
        if (!isMoving)
        {
            CheckObstacles();
            CheckPlayerBounds();
        }
        MoveToPosition();
        JumpModel();
        StopCameraOutOfBounds();
    }

    private void GetInput()
    {
        var currentPosition = transform.position;
        OnPlayerMoved?.Invoke(currentPosition);
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                gotInput = true;
                direction = Vector3.left;
                targetPosition = new Vector3(currentPosition.x - step, currentPosition.y, currentPosition.z);
                jump = true;
                model.transform.rotation = Quaternion.Euler(0, -90, 0);
                animator.SetTrigger(Jump);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector3.right;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x + step, currentPosition.y, currentPosition.z);
                jump = true;
                model.transform.rotation = Quaternion.Euler(0, 90, 0);
                animator.SetTrigger(Jump);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector3.forward;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + step);
                jump = true;
                model.transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetTrigger(Jump);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector3.back;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - step);
                jump = true;
                model.transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetTrigger(Jump);
            }
        }
    }

    private void CheckObstacles()
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

    private void MoveToPosition()
    {
        if (!hasObstacleInDirection && gotInput)
        {
            isMoving = true;
            transform.SetParent(null);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed
                * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 0.8f))
                {
                    if (hit.transform.CompareTag("Log"))
                    {
                        transform.SetParent(hit.transform);
                        hit.transform.GetComponent<Log>().Sink();
                    }
                    else if (hit.transform.CompareTag("Water"))
                    {
                        IsHit?.Invoke();
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
            IsHit?.Invoke();
            moveSpeed = 0;
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
        if (transform.position.x + step >= rightBound && direction == Vector3.right)
        {
            hasObstacleInDirection = true;
        }

        if (transform.position.x - step <= leftBound && direction == Vector3.left)
        {
            hasObstacleInDirection = true;
        }
        
        if (transform.position.x > rightBound + 1 || transform.position.x < leftBound - 1)
        {
            IsHit?.Invoke();
            moveSpeed = 0;
            enabled = false;
        }
    }

    private void StopCameraOutOfBounds()
    {
        if (transform.position.x > rightBound + 1 || transform.position.x < leftBound - 1)
        {
            virtualCamera.m_LookAt = null;
            virtualCamera.m_Follow = null;
        }
    }
}