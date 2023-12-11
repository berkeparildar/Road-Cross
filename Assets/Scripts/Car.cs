using UnityEngine;

public class Car : MovingObstacle
{
    [SerializeField] private GameObject model;
    [SerializeField] private int modelCount;
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private MeshRenderer bodyRender;
    [SerializeField] private BoxCollider boxCollider;
    private const int BodyMaterialIndex = 2;
    private const int CarBodyIndex = 0;
    private void Awake()
    {
        modelCount = transform.childCount;
        model = transform.GetChild(Random.Range(0, modelCount)).gameObject;
        model.SetActive(true);
        bodyRender = model.transform.GetChild(CarBodyIndex).GetComponent<MeshRenderer>();
        if (IsColored(model.name))
        {
            bodyMaterial = bodyRender.materials[BodyMaterialIndex];
            bodyMaterial.color = Random.ColorHSV();
        }
        var meshBounds = bodyRender.bounds;
        boxCollider.size = meshBounds.size;
        TurnCollider();
    }

    private void Update()
    {
        ParentUpdate();
    }
    
    protected override void GoingLeft()
    {
        model.transform.rotation = Quaternion.Euler(0, -90, 0);
        base.GoingLeft();
    }

    protected override void GoingRight()
    {
        model.transform.rotation = Quaternion.Euler(0, 90, 0);
        base.GoingRight();
    }

    private static bool IsColored(string name)
    {
        if (name is "ambulance" or "taxi" or "police")
        {
            return false;
        }

        return true;
    }

    private void TurnCollider()
    {
        var newSize = Vector3.zero;
        var currentSize = boxCollider.size;
        var xSize = currentSize.x;
        var ySize = currentSize.y;
        var zSize = currentSize.z;
        newSize = new Vector3(zSize, ySize, xSize);
        boxCollider.size = newSize;
    }
    
}
