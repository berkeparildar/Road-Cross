using UnityEngine;

public class Log : MovingObstacle
{
    [SerializeField] private Material materialPrefab;
    [SerializeField] private Material materialInstance;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private bool speedSet;
    private void OnEnable()
    {
        materialInstance = new Material(materialPrefab);
        renderer.material = materialInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0 && !speedSet)
        {
            speedSet = true;
            SetScale();
        }
        ParentUpdate();
    }

    private void SetScale()
    {
        transform.localScale = new Vector3(speed, 1, 1);
        materialInstance.mainTextureScale = new Vector2(speed, 1);
    }
}
