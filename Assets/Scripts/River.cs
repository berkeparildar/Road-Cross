using UnityEngine;

public class River : ObstacleSpawner
{
    private static bool _isLeft;

    protected override void GetRandomValues()
    {
        base.GetRandomValues();
        _isLeft = !_isLeft;
        spawnCooldown++;
        spawnDirection = _isLeft ? -50 : 50;
    }

    private void Update()
    {
        ParentUpdate();
        spawnPosition.y = -0.5f;
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