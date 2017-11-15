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

    private float zCoordinate;
    private List<Vector3> spawnPositions;

    private GameController gameController;

    //private GameObject floor;
    //private float timeUntilNextSpawn;


    public UnitManager(int lanes, List<UnitReserve> reserves, bool isEnemy, int? zCoordinate = null)
    {
        this.numberOfLanes = lanes;
        this.reserves = reserves;
        this.isEnemy = isEnemy;

        units = new List<UnitController>();

        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        float heightBuffer = 5;
        float widthBuffer = 5;
        var floorSize = floor.GetComponent<Renderer>().bounds.size;
        float height = floorSize.x - heightBuffer;
        float width = floorSize.z - widthBuffer;
        Debug.Log("Floor height: " + height.ToString());

        // yuck but oh well
        // if we don't have a z coordinate then use the default
        if (zCoordinate != null)
        {
            this.zCoordinate = (int)zCoordinate;
        }
        else
        {
            float edge = width / 2;
            if(isEnemy)
            {
                this.zCoordinate = edge;
            }
            else
            {
                this.zCoordinate = -edge;
            }
        }



            float interval = (height) / lanes;
        spawnPositions = new List<Vector3>();
        for (int i = 1; i <= lanes; ++i)
        {
            //height/2 because floor goes from -X to X
            spawnPositions.Add(new Vector3(interval * i - height/2, 0, this.zCoordinate));
            Debug.Log(spawnPositions[i-1].x);
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
        for (int i = reserves.Count-1; i >=0; i--)
        {
            var reserve = reserves[i];
            if(reserve.spawnTime <= Time.time && laneIsEmpty(reserve.lane))
            {
                spawn(reserve);
                reserves.RemoveAt(i);
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
            //float unitSizeZ = unit.GetComponent<Collider>().bounds.size.z;
            //if (unit.lane == lane && Mathf.Abs(unitSizeZ - zCoordinate) > unitSizeZ)
            // 1 instead of 2 to give some buffer space
            var colliders = Physics.OverlapBox(spawnPositions[lane], unit.GetComponent<Collider>().bounds.size);
            // 1 to ignore floor
            if(colliders.Length > 1)
                return false;
        }
        return true;
    }

    public void spawn(UnitReserve reserve)
    {

        //UnitController newUnit = new UnitController();
        //units.Add(newUnit);

        // TODO add rotation 
        Quaternion rotation = reserve.unitPrefab.transform.rotation;
        if(isEnemy)
        {
            rotation = Quaternion.LookRotation(Vector3.back);
        }
        else
        {
            rotation = Quaternion.LookRotation(Vector3.forward);
        }
        GameObject newUnitObj = GameObject.Instantiate(reserve.unitPrefab, spawnPositions[reserve.lane], rotation);
        UnitController unit = newUnitObj.GetComponent<UnitController>();
        unit.init(reserve);
        units.Add(unit);

    }
}
