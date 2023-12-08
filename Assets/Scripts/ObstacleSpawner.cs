using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] protected int spawnDirection;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] protected float spawnCooldown;
    [SerializeField] protected float obstacleSpeed;
    [SerializeField] protected float spawnTimer;
    private List<MovingObstacle> obstacles;
    private IObjectPool<ObstacleSpawner> objectPool;
    public IObjectPool<ObstacleSpawner> ObjectPool { set => objectPool = value; }
    
    private IObjectPool<MovingObstacle> obstaclePool;
    public IObjectPool<MovingObstacle> ObstaclePool { set => obstaclePool = value; }

    private void Awake()
    {
        obstacles = new List<MovingObstacle>();
    }

    private void OnEnable()
    {
        Player.OnPlayerMoved += Deactivate;
        obstacles.Clear();
        GetRandomValues();
        spawnTimer = spawnCooldown;
    }
    
    private void OnDisable()
    {
        Player.OnPlayerMoved -= Deactivate;
    }
    
    private void Deactivate(Vector3 playerPosition)
    {
        if (playerPosition.z >= transform.position.z + 20)
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Release();
            }
            objectPool.Release(this);
        }
    }
    
    protected virtual void GetRandomValues()
    {
        var seed = Guid.NewGuid().GetHashCode();
        Random.InitState(seed);
        spawnCooldown = Random.Range(1f, 5f);
        obstacleSpeed = Random.Range(4, 8);
    }
    
    protected void ParentUpdate()
    {
        var position = transform.position;
        spawnPosition = new Vector3(spawnDirection, position.y, position.z);
    }

    protected void SpawnObstacle()
    {
        spawnTimer = 0;
        var obstacle = obstaclePool.Get();
        obstacle.SetSpeed(obstacleSpeed);
        obstacles.Add(obstacle);
        obstacle.transform.position = spawnPosition;
        obstacle.SetDirection(spawnDirection);
        obstacle.ObstacleSpawner = this;
    }
    
    public void RemoveFromList(MovingObstacle movingObstacle)
    {
        obstacles.Remove(movingObstacle);
    }
}
