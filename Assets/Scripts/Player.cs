using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int step;
    [SerializeField] private bool isMoving;
    [SerializeField] private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
        MoveToPosition();
    }

    private void GetInput()
    {
        var currentPosition = transform.position;
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetPosition = new Vector3(currentPosition.x - step, currentPosition.y, currentPosition.z);
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                targetPosition = new Vector3(currentPosition.x + step, currentPosition.y, currentPosition.z);
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + step);
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - step);
                isMoving = true;
            }
        }
    }

    private void MoveToPosition()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }
}
