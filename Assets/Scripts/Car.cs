using UnityEngine;

public class Car : MovingObstacle
{
    [SerializeField] private GameObject model;
    private void Awake()
    {
        model = transform.GetChild(0).gameObject;
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
    
}
