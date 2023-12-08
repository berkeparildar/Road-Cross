using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Road : MonoBehaviour
{
    [SerializeField] private int spawnDirection;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnPause;
    [SerializeField] private float carSpeed;
    [SerializeField] private int currentCarCount;
    [SerializeField] private int totalCarCount;
    [SerializeField] private List<Car> cars;
    [SerializeField] private bool startPauseTimer;
    [SerializeField] private float spawnTimer;
    private IObjectPool<Road> objectPool;
    public IObjectPool<Road> ObjectPool { set => objectPool = value; }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        Player.OnPlayerMoved += Deactivate;
        cars.Clear();
        GetRandomValues();
        spawnTimer = spawnCooldown;
    }

    private void OnDisable()
    {
        Player.OnPlayerMoved -= Deactivate;
    }

    private void GetRandomValues()
    {
        var seed = Guid.NewGuid().GetHashCode();
        Random.InitState(seed);
        totalCarCount = Random.Range(1, 4);
        carSpeed = Random.Range(4, 8);
        spawnCooldown = Random.Range(1f, 5f);
        var halfChance = Random.Range(0, 2);
        if (halfChance == 0)
        {
            spawnDirection = 30;
        }
        else
        {
            spawnDirection = -30;
        }
    }

    private void Update()
    {
        var position = transform.position;
        spawnPosition = new Vector3(spawnDirection, position.y, position.z);
        SpawnPause();
    }

    private void SpawnPause()
    {
        if (startPauseTimer)
        {
            spawnPause -= Time.deltaTime;
        }
        if (spawnPause <= 0f)
        {
            startPauseTimer = false;
            spawnPause = 4.0f;
        }

        if (!startPauseTimer)
        {
            SpawnCars();
        }
    }

    private void SpawnCars()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCooldown && currentCarCount < totalCarCount)
        {
            spawnTimer = 0;
            var car = Pool.SharedInstance.CarPool.Get();
            cars.Add(car);
            car.transform.position = spawnPosition;
            car.SetDirection(spawnDirection);
            car.SetSpeed(carSpeed);
            car.Road = this;
            currentCarCount++;
        }

        if (currentCarCount == totalCarCount)
        {
            currentCarCount = 0;
            startPauseTimer = true;
        }
    }
    
    private void Deactivate(Vector3 playerPosition)
    {
        if (playerPosition.z >= transform.position.z + 20)
        {
            foreach (var car in cars)
            {
                car.Release();
            }
            objectPool.Release(this);
        }
    }

    public void RemoveFromList(Car car)
    {
        cars.Remove(car);
    }
}