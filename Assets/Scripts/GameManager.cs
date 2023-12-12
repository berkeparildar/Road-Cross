using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private float currentPosition;
    [SerializeField] private int levelSelector; // This will be used to control what is being generated
    [SerializeField] private bool currentlyBeingGenerated;
    [SerializeField] private Vector3 defaultGrassPosition;
    [SerializeField] private Vector3 defaultRiverPosition;
    [SerializeField] private Vector3 defaultRoadPosition;
    [SerializeField] private float targetSpawnPosition;
    [SerializeField] private Player player;
    [SerializeField] private GameObject endUI;
    
    private void Start()
    {
        currentPosition = 15;
        levelSelector = 1;
        targetSpawnPosition = -18;
    }

    private void Update()
    {
        if (player.transform.position.z >= targetSpawnPosition - 18)
        {
            GenerateLevel();
        }
    }

    private void GenerateLevel()
    {
        if (!currentlyBeingGenerated)
        {
            currentlyBeingGenerated = true;
            switch (levelSelector)
            {
                case 1:
                    GenerateGrass();
                    break;
                case 2:
                    GenerateRiver();
                    break;
                case 3:
                    GenerateGrass();
                    break;
                case 4:
                    GenerateRoad();
                    break;
            }
        }
        currentlyBeingGenerated = false;
    }
    
    private void GenerateGrass()
    {
        var grassCount = Random.Range(1, 3);
        for (int i = 0; i < grassCount; i++)
        {
            targetSpawnPosition += 3;
            defaultGrassPosition.z = currentPosition;
            var grass = Pool.SharedInstance.GrassPool.Get();
            grass.transform.position = defaultGrassPosition;
            grass.Instantiate();
            currentPosition += 3;
        }
        levelSelector++;
    }

    private void GenerateRiver()
    {
        var riverCount = Random.Range(3, 8);
        for (int i = 0; i < riverCount; i++)
        {
            targetSpawnPosition += 3;
            defaultRiverPosition.z = currentPosition;
            var river = Pool.SharedInstance.RiverPool.Get();
            river.transform.position = defaultRiverPosition;
            currentPosition += 3;
        }
        levelSelector++;
    }
    
    private void GenerateRoad()
    {
        defaultRoadPosition.z = currentPosition;
        currentPosition += 3;
        targetSpawnPosition += 3;
        var roadStart = Pool.SharedInstance.RoadStartPool.Get();
        roadStart.transform.position = defaultRoadPosition;
        var roadCount = Random.Range(0, 5);
        for (int i = 0; i < roadCount; i++)
        {
            targetSpawnPosition += 3;
            defaultRoadPosition.z = currentPosition;
            var roadMiddle = Pool.SharedInstance.RoadMiddlePool.Get();
            roadMiddle.transform.position = defaultRoadPosition;
            currentPosition += 3;
        }
        defaultRoadPosition.z = currentPosition;
        currentPosition += 3;
        targetSpawnPosition += 3;
        var roadEnd = Pool.SharedInstance.RoadEndPool.Get();
        roadEnd.transform.position = defaultRoadPosition;
        levelSelector = 1;
    }

    private void OnEnable()
    {
        Player.IsHit += GameOver;
    }

    private void OnDisable()
    {
        Player.IsHit -= GameOver;
    }
    
    private void GameOver()
    {
        endUI.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}