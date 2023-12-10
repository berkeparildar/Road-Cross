using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Grass : MonoBehaviour
{
    private List<Trees> trees;
    private IObjectPool<Grass> objectPool;
    public IObjectPool<Grass> ObjectPool { set => objectPool = value; }
    private IObjectPool<Trees> treePool;
    public IObjectPool<Trees> TreePool { set => treePool = value; }
    [SerializeField] private int treeSpawnPosition;
    [SerializeField] private int treeCount;
    [SerializeField] private List<int> spawnPositions;

    private void Awake()
    {
        trees = new List<Trees>();
    }

    private void OnEnable()
    {
        treeCount = Random.Range(0, 5);
        Player.OnPlayerMoved += Deactivate;
        trees.Clear();
    }

    public void Instantiate()
    {
        SpawnTrees();
    }

    private void OnDisable()
    {
        Player.OnPlayerMoved -= Deactivate;
    }

    private void Deactivate(Vector3 playerPosition)
    {
        if (playerPosition.z >= transform.position.z + 20)
        {
            foreach (var obstacle in trees)
            {
                spawnPositions.Add(obstacle.currentPosition);
                obstacle.Release();
            }
            objectPool.Release(this);
        }
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            treeSpawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            var tree = treePool.Get();
            tree.currentPosition = treeSpawnPosition;
            spawnPositions.Remove(treeSpawnPosition);
            tree.transform.position = new Vector3(treeSpawnPosition, 0, transform.position.z);
            trees.Add(tree);
        }
    }
}