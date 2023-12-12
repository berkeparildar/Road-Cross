using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] protected int spawnDirection;
    [SerializeField] protected Vector3 spawnPosition;
    [SerializeField] protected float spawnCooldown;
    [SerializeField] protected float obstacleSpeed;
    [SerializeField] protected float spawnTimer;
    private List<MovingObstacle> _obstacles;
    private IObjectPool<ObstacleSpawner> _objectPool;

    public IObjectPool<ObstacleSpawner> ObjectPool
    {
        set => _objectPool = value;
    }

    private IObjectPool<MovingObstacle> _obstaclePool;

    public IObjectPool<MovingObstacle> ObstaclePool
    {
        set => _obstaclePool = value;
    }

    private void Awake()
    {
        _obstacles = new List<MovingObstacle>();
    }

    private void OnEnable()
    {
        Player.OnPlayerMoved += Deactivate;
        Player.IsHit += GameOver;
        _obstacles.Clear();
        GetRandomValues();
        spawnTimer = spawnCooldown;
    }

    private void OnDisable()
    {
        Player.OnPlayerMoved -= Deactivate;
        Player.IsHit -= GameOver;
    }

    private void Deactivate(Vector3 playerPosition)
    {
        if (playerPosition.z >= transform.position.z + 20)
        {
            foreach (var obstacle in _obstacles)
            {
                obstacle.Release();
            }

            _objectPool.Release(this);
        }
    }

    protected virtual void GetRandomValues()
    {
        var seed = Guid.NewGuid().GetHashCode();
        Random.InitState(seed);
        spawnCooldown = Random.Range(1f, 5f);
        obstacleSpeed = Random.Range(3, 9);
    }

    protected void ParentUpdate()
    {
        var position = transform.position;
        spawnPosition = new Vector3(spawnDirection, position.y, position.z);
    }

    protected void SpawnObstacle()
    {
        spawnTimer = 0;
        var obstacle = _obstaclePool.Get();
        obstacle.SetSpeed(obstacleSpeed);
        _obstacles.Add(obstacle);
        obstacle.transform.position = spawnPosition;
        obstacle.SetDirection(spawnDirection);
        obstacle.ObstacleSpawner = this;
    }

    private void GameOver()
    {
        this.enabled = false;
    }

    public void RemoveFromList(MovingObstacle movingObstacle)
    {
        _obstacles.Remove(movingObstacle);
    }
}