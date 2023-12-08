using UnityEngine;
using UnityEngine.Pool;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float deactivationPoint;
    [SerializeField] protected bool isGoingLeft;
    [SerializeField] protected ObstacleSpawner obstacleSpawner;
    protected IObjectPool<MovingObstacle> objectPool;
    public IObjectPool<MovingObstacle> ObjectPool { set => objectPool = value; }
    public ObstacleSpawner ObstacleSpawner { set => obstacleSpawner = value; }

    protected void ParentUpdate()
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
    
    public void SetSpeed(float obstacleSpeed)
    {
        speed = obstacleSpeed;
    }
    
    private void Deactivate()
    {
        if (isGoingLeft && transform.position.x <= deactivationPoint && gameObject.activeSelf)
        {
            obstacleSpawner.RemoveFromList(this);
            objectPool.Release(this);
        }
        else if (!isGoingLeft && transform.position.x >= deactivationPoint && gameObject.activeSelf)
        {
            obstacleSpawner.RemoveFromList(this);
            objectPool.Release(this);
        }
    }
    
    public void Release()
    {
        objectPool.Release(this);
    }

    protected virtual void GoingLeft()
    {
        deactivationPoint = -30;
        direction = Vector3.left;
        isGoingLeft = true;
    }

    protected virtual void GoingRight()
    {
        deactivationPoint = 30;
        direction = Vector3.right;
        isGoingLeft = false;
    }
}
