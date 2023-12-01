using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private int spawnDirection;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float carSpeed;
    [SerializeField] private int spawnCount;
    private void Start()
    {
        spawnCount = Random.Range(1, 6);
        carSpeed = Random.Range(5, 9);
        spawnCooldown = 5;
        var halfChance = Random.Range(0, 2);
        if (halfChance == 0)
        {
            spawnDirection = 30;
        }
        else
        {
            spawnDirection = -30;
        }

        var position = transform.position;
        spawnPosition = new Vector3(spawnDirection, position.y, position.z);
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnCar());
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator SpawnCar()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
            car.GetComponent<Car>().SetDirection(spawnDirection);
            car.GetComponent<Car>().SetSpeed(carSpeed);
            yield return new WaitForSeconds(spawnCooldown / spawnCount);
        }
    }
}