using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitReserve
{
    public float spawnTime;
    public GameObject unitPrefab;
    public int lane;

    public UnitReserve(float spawnTime, int lane, GameObject unitPrefab)
    {
        this.spawnTime = spawnTime;
        this.lane = lane;
        this.unitPrefab = unitPrefab;
    }
}

public class UnitManager {

    //public List<UnitController> defenders;
    //public List<UnitController> enemies;

    private int numberOfLanes;
    //private int numberOfReserves;
    //private List<GameObject> reserves;
    //private int defenderReserves;
    //private int enemiesReserves;

    //private Spawner enemySpawner;
    //private Spawner defenderSpawner;

    public List<UnitReserve> reserves;
    private List<UnitController> units;

    private bool isEnemy;

    private int zCoordinate;
    private List<Vector3> spawnPositions;

    private GameController gameController;

    //private GameObject floor;
    //private float timeUntilNextSpawn;


    public UnitManager(int lanes, List<UnitReserve> reserves, bool isEnemy, int zCoordinate)
    {
        this.numberOfLanes = lanes;
        this.reserves = reserves;
        this.isEnemy = isEnemy;
        this.zCoordinate = zCoordinate;

        units = new List<UnitController>();

        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        float height = floor.GetComponent<Renderer>().bounds.size.x;
        float buffer = 5;
        float interval = (height - buffer) / lanes;
        spawnPositions = new List<Vector3>();
        for (int i = 0; i < lanes; ++i)
        {
            spawnPositions.Add(new Vector3(interval * i + buffer, 1, zCoordinate));
        }
    }

    // Use this for initialization
    //void Start ()
    //   {

    //}

    // Update is called once per frame
    //void Update()
    public void updateUnits()
    {
        foreach(var reserve in reserves)
        {
            if(reserve.spawnTime <= Time.time && laneIsEmpty(reserve.lane))
            {
                spawn(reserve);
            }
        }
        //if (numberOfReserves > 0)
        //{
        //    for (int i = 0; i < numberOfLanes; ++i)
        //    {
        //        if (laneIsEmpty(i))
        //        {
        //            spawn(i);
        //        }
        //    }
        //}
        
        //if (numberOfReserves > 0 && )
        //{
        //    spawn();
        //}
    }

    bool laneIsEmpty(int lane)
    {
        foreach(UnitController unit in units)
        {
            float unitSizeZ = unit.GetComponent<Collider>().bounds.size.z;
            if (unit.lane == lane && Mathf.Abs(unitSizeZ - zCoordinate) > unitSizeZ)
                return false;
        }
        return true;
    }

    public void spawn(UnitReserve reserve)
    {

        //UnitController newUnit = new UnitController();
        //units.Add(newUnit);

        // TODO add rotation
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject newUnit = GameObject.Instantiate(reserve.unitPrefab, spawnPositions[reserve.lane], rotation);
        units.Add(newUnit.GetComponent<UnitController>());

    }
}
