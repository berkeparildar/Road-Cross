using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private int step;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool hasObstacleInDirection;
    public static event Action<Vector3> OnPlayerMoved;
    [SerializeField] private bool gotInput;
    

    private void Update()
    {
        Debug.DrawRay(transform.position, direction * step, Color.red);
        GetInput();
        if (!isMoving)
        {
            CheckObstacles();
        }
        MoveToPosition();
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
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector3.right;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x + step, currentPosition.y, currentPosition.z);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector3.forward;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + step);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector3.back;
                gotInput = true;
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - step);
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
                
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2f))
                {
                    if (hit.transform.gameObject.CompareTag("Log"))
                    {
                        transform.SetParent(hit.transform);
                    }
                }
                isMoving = false;
                gotInput = false;
            }
        }
    }
}