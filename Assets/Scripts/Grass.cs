using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Grass : MonoBehaviour
{
    private List<Trees> _trees;
    private IObjectPool<Grass> _objectPool;
    public IObjectPool<Grass> ObjectPool { set => _objectPool = value; }
    private IObjectPool<Trees> _treePool;
    public IObjectPool<Trees> TreePool { set => _treePool = value; }
    [SerializeField] private int treeSpawnPosition;
    [SerializeField] private int treeCount;
    [SerializeField] private List<int> spawnPositions;

    private void Awake()
    {
        _trees = new List<Trees>();
    }

    private void OnEnable()
    {
        treeCount = Random.Range(0, 8);
        Player.OnPlayerMoved += Deactivate;
        _trees.Clear();
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
            foreach (var obstacle in _trees)
            {
                spawnPositions.Add(obstacle.currentPosition);
                obstacle.Release();
            }
            _objectPool.Release(this);
        }
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            treeSpawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            var tree = _treePool.Get();
            tree.currentPosition = treeSpawnPosition;
            spawnPositions.Remove(treeSpawnPosition);
            tree.transform.position = new Vector3(treeSpawnPosition, 0, transform.position.z);
            _trees.Add(tree);
        }
    }
}