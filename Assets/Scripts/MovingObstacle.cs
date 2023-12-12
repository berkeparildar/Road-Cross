using UnityEngine;
using UnityEngine.Pool;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float deactivationPoint;
    [SerializeField] protected bool isGoingLeft;
    [SerializeField] protected ObstacleSpawner obstacleSpawner;
    private IObjectPool<MovingObstacle> _objectPool;
    public IObjectPool<MovingObstacle> ObjectPool { set => _objectPool = value; }
    public ObstacleSpawner ObstacleSpawner { set => obstacleSpawner = value; }

    protected void ParentUpdate()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
        Deactivate();
    }
    
    public void SetDirection(int spawnPosition)
    {
        if (spawnPosition == 50)
        {
            GoingLeft();
        }
        else
        {
            GoingRight();
        }
    }
    
    public virtual void SetSpeed(float obstacleSpeed)
    {
        speed = obstacleSpeed;
    }
    
    private void Deactivate()
    {
        if (isGoingLeft && transform.position.x <= deactivationPoint && gameObject.activeSelf)
        {
            obstacleSpawner.RemoveFromList(this);
            _objectPool.Release(this);
        }
        else if (!isGoingLeft && transform.position.x >= deactivationPoint && gameObject.activeSelf)
        {
            obstacleSpawner.RemoveFromList(this);
            _objectPool.Release(this);
        }
    }
    
    public void Release()
    {
        _objectPool.Release(this);
    }

    protected virtual void GoingLeft()
    {
        deactivationPoint = -50;
        direction = Vector3.left;
        isGoingLeft = true;
    }

    protected virtual void GoingRight()
    {
        deactivationPoint = 50;
        direction = Vector3.right;
        isGoingLeft = false;
    }
}
