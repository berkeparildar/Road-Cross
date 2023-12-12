using System;
using UnityEngine;

public class Log : MovingObstacle
{
    [SerializeField] private Material materialPrefab;
    [SerializeField] private Material materialInstance;
    [SerializeField] private MeshRenderer logRenderer;
    [SerializeField] private float scaleValue;
    [SerializeField] private float sinkSpeed;
    [SerializeField] private bool shouldSink;
    [SerializeField] private bool reachedBottom;
    public static event Action<bool> IsSinking;

    protected void OnEnable()
    {
        materialInstance = new Material(materialPrefab);
        logRenderer.material = materialInstance;
    }

    public override void SetSpeed(float obstacleSpeed)
    {
        base.SetSpeed(obstacleSpeed);
        scaleValue = speed;
        transform.localScale = new Vector3(scaleValue, 1, 1);
        materialInstance.mainTextureScale = new Vector2(speed, 1);
    }

    private void Update()
    {
        ParentUpdate();
        SinkABit();
    }

    public void Sink()
    {
        shouldSink = true;
    }

    private void SinkABit()
    {
        var currentPosition = transform.position;
        if (!shouldSink) return;
        IsSinking?.Invoke(shouldSink);
        if (!reachedBottom)
        {
            transform.position = Vector3.MoveTowards(currentPosition, new Vector3(currentPosition.x, -0.8f, 
                currentPosition.z), sinkSpeed * Time.deltaTime);
            if (!(currentPosition.y <= -0.8f)) return;
            transform.position = new Vector3(currentPosition.x, -0.8f, currentPosition.z);
            reachedBottom = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(currentPosition, new Vector3(currentPosition.x, -0.5f, 
                currentPosition.z), sinkSpeed * Time.deltaTime);
            if (!(transform.position.y >= -0.5f)) return;
            transform.position = new Vector3(currentPosition.x, -0.5f, currentPosition.z);
            shouldSink = false;
            reachedBottom = false;
            IsSinking?.Invoke(shouldSink);
        }
    }
}