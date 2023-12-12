using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : ObstacleSpawner
{
    [SerializeField] private float spawnPause;
    [SerializeField] private int currentCarCount;
    [SerializeField] private int totalCarCount;
    [SerializeField] private bool startPauseTimer;
   
    protected override void GetRandomValues()
    {
        base.GetRandomValues();
        var seed = Guid.NewGuid().GetHashCode();
        Random.InitState(seed);
        totalCarCount = Random.Range(1, 4);
        var halfChance = Random.Range(0, 2);
        if (halfChance == 0)
        {
            spawnDirection = 50;
        }
        else
        {
            spawnDirection = -50;
        }
    }

    private void Update()
    {
        ParentUpdate();
        SpawnPause();
    }

    private void SpawnPause()
    {
        if (startPauseTimer)
        {
            spawnPause -= Time.deltaTime;
        }
        if (spawnPause <= 0f)
        {
            startPauseTimer = false;
            spawnPause = 4.0f;
        }

        if (!startPauseTimer)
        {
            SpawnCars();
        }
    }

    private void SpawnCars()
    {
        spawnTimer += Time.deltaTime;
        if (currentCarCount < totalCarCount)
        {
            if (spawnTimer >= spawnCooldown)
            {
                SpawnObstacle();
                currentCarCount++;
            }
        }
        if (currentCarCount == totalCarCount)
        {
            currentCarCount = 0;
            startPauseTimer = true;
        }
    }
}