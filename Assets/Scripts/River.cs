using UnityEngine;
using UnityEngine.Pool;

public class River : MonoBehaviour
{
    [SerializeField] private Renderer riverRenderer;
    private IObjectPool<River> objectPool;
    public IObjectPool<River> ObjectPool { set => objectPool = value; }
    void Start()
    {
        Debug.Log("I am a river");
    }

    private void OnEnable()
    {
        Player.OnPlayerMoved += Deactivate;
    }

    private void OnDisable()
    {
        Player.OnPlayerMoved -= Deactivate;
    }
    
    private void Deactivate(Vector3 playerPosition)
    {
        if (playerPosition.z >= transform.position.z + 20)
        {
            objectPool.Release(this);
        }
    }
}
