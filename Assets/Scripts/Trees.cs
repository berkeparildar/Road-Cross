using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Trees : MonoBehaviour
{
    private IObjectPool<Trees> objectPool;
    public IObjectPool<Trees> ObjectPool { set => objectPool = value; }
    public int currentPosition;
    [SerializeField] private int treeLength;
    [SerializeField] private GameObject[] treeParts;

    private void OnEnable()
    {
        treeLength = Random.Range(0, 3);
        for (int i = 0; i < treeLength; i++)
        {
            treeParts[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (var tree in treeParts)
        {
            if (tree.activeSelf)
            {
                tree.SetActive(false);
            }
        }
    }
    
    public void Release()
    {
        objectPool.Release(this);
    }
}
