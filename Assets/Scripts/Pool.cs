using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    public static Pool SharedInstance;
    [SerializeField] private Car carPrefab;
    [SerializeField] private int carCount;
    [SerializeField] private GameObject carContainer;
    [SerializeField] private Road roadStartPrefab;
    [SerializeField] private Road roadMiddlePrefab;
    [SerializeField] private Road roadEndPrefab;
    [SerializeField] private int roadStartCount;
    [SerializeField] private int roadMiddleCount;
    [SerializeField] private int roadEndCount;
    [SerializeField] private River riverPrefab;
    [SerializeField] private int riverCount;
    [SerializeField] private GameObject levelContainer;
    public IObjectPool<Car> CarPool;
    public IObjectPool<Road> RoadStartPool;
    public IObjectPool<Road> RoadMiddlePool;
    public IObjectPool<Road> RoadEndPool;
    public IObjectPool<River> RiverPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        CarPool = new ObjectPool<Car>(CreateCar, ActivateCar, DeactivateCar, DestroyCar, true, carCount, carCount * 2);
        RoadStartPool = new ObjectPool<Road>(CreateRoadStart, ActivateRoad, DeactivateRoad, DestroyRoad, true,
            roadStartCount, roadStartCount * 2);
        RoadMiddlePool = new ObjectPool<Road>(CreateRoadMiddle, ActivateRoad, DeactivateRoad, DestroyRoad, true,
            roadMiddleCount, roadMiddleCount * 2);
        RoadEndPool = new ObjectPool<Road>(CreateRoadEnd, ActivateRoad, DeactivateRoad, DestroyRoad, true,
            roadEndCount, roadEndCount * 2);
        RiverPool = new ObjectPool<River>(CreateRiver, ActivateRiver, DeactivateRiver, DestroyRiver, true,
            riverCount, riverCount * 2);
    }

    private Car CreateCar()
    {
        Car carInstance = Instantiate(carPrefab, carContainer.transform);
        carInstance.ObjectPool = CarPool;
        return carInstance;
    }

    private void ActivateCar(Car car)
    {
        car.gameObject.SetActive(true);
    }
    
    private void DeactivateCar(Car car)
    {
        car.gameObject.SetActive(false);
    }
    
    private void DestroyCar(Car car)
    {
        Destroy(car.gameObject);
    }

    private Road CreateRoadStart()
    {
        Road roadInstance = Instantiate(roadStartPrefab, levelContainer.transform);
        roadInstance.ObjectPool = RoadStartPool;
        return roadInstance;
    }
    
    private Road CreateRoadMiddle()
    {
        Road roadInstance = Instantiate(roadMiddlePrefab, levelContainer.transform);
        roadInstance.ObjectPool = RoadMiddlePool;
        return roadInstance;
    }
    
    private Road CreateRoadEnd()
    {
        Road roadInstance = Instantiate(roadEndPrefab, levelContainer.transform);
        roadInstance.ObjectPool = RoadEndPool;
        return roadInstance;
    }

    private void ActivateRoad(Road road)
    {
        road.gameObject.SetActive(true);
    }

    private void DeactivateRoad(Road road)
    {
        road.gameObject.SetActive(false);
    }

    private void DestroyRoad(Road road)
    {
        Destroy(road.gameObject);
    }

    private River CreateRiver()
    {
        River riverInstace = Instantiate(riverPrefab, levelContainer.transform);
        riverInstace.ObjectPool = RiverPool;
        return riverInstace;
    }

    private void ActivateRiver(River river)
    {
        river.gameObject.SetActive(true);
    }

    private void DeactivateRiver(River river)
    {
        river.gameObject.SetActive(false);
    }

    private void DestroyRiver(River river)
    {
        Destroy(river.gameObject);
    }
}
