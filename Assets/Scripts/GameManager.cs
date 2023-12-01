using System;
using System.IO.Compression;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject riverPrefab;
    [SerializeField] private GameObject roadStartPrefab;
    [SerializeField] private GameObject roadMiddlePrefab;
    [SerializeField] private GameObject roadEndPrefab;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private float currentPosition;
    [SerializeField] private int levelSelector; // This will be used to control what is being generated
    [SerializeField] private bool currentlyBeingGenerated;

    [SerializeField] private Vector3 defaultGrassPosition;
    [SerializeField] private Vector3 defaultRiverPosition;
    [SerializeField] private Vector3 defaultRoadPosition;

    [SerializeField] private float targetSpawnPosition;

    [SerializeField] private Player player;
    

    private void Start()
    {
        currentPosition = 15;
        levelSelector = 1;
        targetSpawnPosition = -18;
    }

    private void Update()
    {
        if (player.transform.position.z >= targetSpawnPosition)
        {
            GenerateLevel();
            targetSpawnPosition += 3;
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
            defaultGrassPosition.z = currentPosition;
            Instantiate(grassPrefab, defaultGrassPosition, Quaternion.identity);
            currentPosition += 3;
        }
        levelSelector++;
    }

    private void GenerateRiver()
    {
        var riverCount = Random.Range(3, 8);
        for (int i = 0; i < riverCount; i++)
        {
            defaultRiverPosition.z = currentPosition;
            Instantiate(riverPrefab, defaultRiverPosition, Quaternion.identity);
            currentPosition += 3;
        }
        levelSelector++;
    }
    
    private void GenerateRoad()
    {
        defaultRoadPosition.z = currentPosition;
        currentPosition += 3;
        Instantiate(roadStartPrefab, defaultRoadPosition, Quaternion.identity);
        var roadCount = Random.Range(0, 5);
        for (int i = 0; i < roadCount; i++)
        {
            defaultRoadPosition.z = currentPosition;
            Instantiate(roadMiddlePrefab, defaultRoadPosition, Quaternion.identity);
            currentPosition += 3;
        }
        defaultRoadPosition.z = currentPosition;
        currentPosition += 3;
        Instantiate(roadEndPrefab, defaultRoadPosition, Quaternion.identity);
        levelSelector = 1;
    }
}
