using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    public static Pool SharedInstance;
    [SerializeField] private Car carPrefab;
    [SerializeField] private int carCount;
    [SerializeField] private GameObject carContainer;
    [SerializeField] private Log logPrefab;
    [SerializeField] private int logCount;
    [SerializeField] private GameObject logContainer;
    [SerializeField] private Road roadStartPrefab;
    [SerializeField] private int roadStartCount;
    [SerializeField] private Road roadMiddlePrefab;
    [SerializeField] private int roadMiddleCount;
    [SerializeField] private Road roadEndPrefab;
    [SerializeField] private int roadEndCount;
    [SerializeField] private GameObject roadContainer;
    [SerializeField] private River riverPrefab;
    [SerializeField] private int riverCount;
    [SerializeField] private GameObject riverContainer;
    [SerializeField] private Grass grassPrefab;
    [SerializeField] private int grassCount;
    [SerializeField] private GameObject grassContainer;
    [SerializeField] private Trees treePrefab;
    [SerializeField] private int treeCount;
    [SerializeField] private GameObject treeContainer;
    public IObjectPool<MovingObstacle> CarPool;
    public IObjectPool<MovingObstacle> LogPool;
    public IObjectPool<ObstacleSpawner> RoadStartPool;
    public IObjectPool<ObstacleSpawner> RoadMiddlePool;
    public IObjectPool<ObstacleSpawner> RoadEndPool;
    public IObjectPool<ObstacleSpawner> RiverPool;
    public IObjectPool<Grass> GrassPool;
    public IObjectPool<Trees> TreePool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        CarPool = new ObjectPool<MovingObstacle>(CreateCar, ActivateMovingObstacle, DeactivateMovingObstacle, DestroyMovingObstacle, true, carCount, carCount * 2);
        LogPool = new ObjectPool<MovingObstacle>(CreateLog, ActivateMovingObstacle, DeactivateMovingObstacle,
            DestroyMovingObstacle, true, logCount, logCount * 2);
        TreePool = new ObjectPool<Trees>(CreateTree, ActivateTree, DeactivateTree, DestroyTree, true, treeCount, 
            treeCount * 2);
        RoadStartPool = new ObjectPool<ObstacleSpawner>(CreateRoadStart, ActivateObstacleSpawner, DeactivateObstacleSpawner, DestroyObstacleSpawner, true,
            roadStartCount, roadStartCount * 2);
        RoadMiddlePool = new ObjectPool<ObstacleSpawner>(CreateRoadMiddle, ActivateObstacleSpawner, DeactivateObstacleSpawner, DestroyObstacleSpawner, true,
            roadMiddleCount, roadMiddleCount * 2);
        RoadEndPool = new ObjectPool<ObstacleSpawner>(CreateRoadEnd, ActivateObstacleSpawner, DeactivateObstacleSpawner, DestroyObstacleSpawner, true,
            roadEndCount, roadEndCount * 2);
        RiverPool = new ObjectPool<ObstacleSpawner>(CreateRiver, ActivateObstacleSpawner, DeactivateObstacleSpawner, 
            DestroyObstacleSpawner, true,
            riverCount, riverCount * 2);
        GrassPool = new ObjectPool<Grass>(CreateGrass, ActivateGrass, DeactivateGrass, DestroyGrass, true,
            grassCount, grassCount * 2);
    }

    private Log CreateLog()
    {
        Log logInstance = Instantiate(logPrefab, logContainer.transform);
        logInstance.ObjectPool = LogPool;
        return logInstance;
    }

    private Car CreateCar()
    {
        Car carInstance = Instantiate(carPrefab, carContainer.transform);
        carInstance.ObjectPool = CarPool;
        return carInstance;
    }
    
    private River CreateRiver()
    {
        River riverInstance = Instantiate(riverPrefab, riverContainer.transform);
        riverInstance.ObjectPool = RiverPool;
        riverInstance.ObstaclePool = LogPool;
        return riverInstance;
    }

    private Grass CreateGrass()
    {
        Grass grassInstance = Instantiate(grassPrefab, grassContainer.transform);
        grassInstance.ObjectPool = GrassPool;
        grassInstance.TreePool = TreePool;
        return grassInstance;
    }
    
    private Trees CreateTree()
    {
        Trees treeInstance = Instantiate(treePrefab, treeContainer.transform);
        treeInstance.ObjectPool = TreePool;
        return treeInstance;
    }

    private void ActivateTree(Trees tree)
    {
        tree.gameObject.SetActive(true);
    }

    private void DeactivateTree(Trees tree)
    {
        tree.gameObject.SetActive(false);
    }

    private void DestroyTree(Trees tree)
    {
        Destroy(tree.gameObject);
    }

    private void ActivateMovingObstacle(MovingObstacle movingObstacle)
    {
        movingObstacle.gameObject.SetActive(true);
    }
    
    private void DeactivateMovingObstacle(MovingObstacle movingObstacle)
    {
        movingObstacle.gameObject.SetActive(false);
    }
    
    private void DestroyMovingObstacle(MovingObstacle movingObstacle)
    {
        Destroy(movingObstacle.gameObject);
    }

    private Road CreateRoadStart()
    {
        Road roadInstance = Instantiate(roadStartPrefab, roadContainer.transform);
        roadInstance.ObjectPool = RoadStartPool;
        roadInstance.ObstaclePool = CarPool;
        return roadInstance;
    }
    
    private Road CreateRoadMiddle()
    {
        Road roadInstance = Instantiate(roadMiddlePrefab, roadContainer.transform);
        roadInstance.ObjectPool = RoadMiddlePool;
        roadInstance.ObstaclePool = CarPool;
        return roadInstance;
    }
    
    private Road CreateRoadEnd()
    {
        Road roadInstance = Instantiate(roadEndPrefab, roadContainer.transform);
        roadInstance.ObjectPool = RoadEndPool;
        roadInstance.ObstaclePool = CarPool;
        return roadInstance;
    }

    private void ActivateObstacleSpawner(ObstacleSpawner obstacleSpawner)
    {
        obstacleSpawner.gameObject.SetActive(true);
    }

    private void DeactivateObstacleSpawner(ObstacleSpawner obstacleSpawner)
    {
        obstacleSpawner.gameObject.SetActive(false);
    }

    private void DestroyObstacleSpawner(ObstacleSpawner obstacleSpawner)
    {
        Destroy(obstacleSpawner.gameObject);
    }
    
    private void ActivateGrass(Grass grass)
    {
        grass.gameObject.SetActive(true);
    }

    private void DeactivateGrass(Grass grass)
    {
        grass.gameObject.SetActive(false);
    }

    private void DestroyGrass(Grass grass)
    {
        Destroy(grass.gameObject);
    }
}
