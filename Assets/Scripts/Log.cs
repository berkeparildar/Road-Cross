using UnityEngine;

public class Log : MovingObstacle
{
    [SerializeField] private Material materialPrefab;
    [SerializeField] private Material materialInstance;
    [SerializeField] private MeshRenderer logRenderer;
    private void OnEnable()
    {
        materialInstance = new Material(materialPrefab);
        logRenderer.material = materialInstance;
    }

    private void Update()
    {
        SetScale();
        ParentUpdate();
    }

    private void SetScale()
    {
        transform.localScale = new Vector3(speed, 1, 1);
        materialInstance.mainTextureScale = new Vector2(speed, 1);
    }
}
