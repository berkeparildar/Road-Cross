using UnityEngine;
using UnityEngine.Pool;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject model;
    [SerializeField] private float deactivationPoint;
    [SerializeField] private Road road;
    [SerializeField] private bool isGoingLeft;
    private IObjectPool<Car> objectPool;
    public IObjectPool<Car> ObjectPool { set => objectPool = value; }
    public Road Road {set => road = value;}
    private void Awake()
    {
        model = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        Deactivate();
    }

    public void SetDirection(int spawnPosition)
    {
        if (spawnPosition == 30)
        {
            GoingLeft();
        }
        else
        {
            GoingRight();
        }
    }

    public void SetSpeed(float carSpeed)
    {
        speed = carSpeed;
    }

    private void Deactivate()
    {
        if (isGoingLeft && transform.position.x <= deactivationPoint && gameObject.activeSelf)
        {
            objectPool.Release(this);
            road.RemoveFromList(this);
        }
        else if (!isGoingLeft && transform.position.x >= deactivationPoint && gameObject.activeSelf)
        {
            objectPool.Release(this);
            road.RemoveFromList(this);
        }
    }

    public void Release()
    {
        objectPool.Release(this);
    }

    private void GoingLeft()
    {
        model.transform.rotation = Quaternion.Euler(0, -90, 0);
        deactivationPoint = -30;
        direction = Vector3.left;
        isGoingLeft = true;
    }

    private void GoingRight()
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);
        deactivationPoint = 30;
        direction = Vector3.right;
        isGoingLeft = false;
    }
}
