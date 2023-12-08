using UnityEngine;

public class River : ObstacleSpawner
{
    private static bool _isLeft;

    protected override void GetRandomValues()
    {
        base.GetRandomValues();
        _isLeft = !_isLeft;
        spawnCooldown += 2;
        spawnDirection = _isLeft ? -30 : 30;
    }

    private void Update()
    {
        ParentUpdate();
        SpawnLogs();
    }

    private void SpawnLogs()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCooldown)
        {
            SpawnObstacle();
        }
    }
}