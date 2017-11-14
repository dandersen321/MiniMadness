using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    //public List<UnitController> defenders;
    //public List<UnitController> enemies;

    private int numberOfLanes;
    //private int numberOfReserves;
    private List<GameObject> reserves;
    //private int defenderReserves;
    //private int enemiesReserves;

    //private Spawner enemySpawner;
    //private Spawner defenderSpawner;

    public List<UnitController> units;

    private bool isEnemy;

    private int zCoordinate;
    private List<Vector3> spawnPositions;

    //private GameObject floor;
    //private float timeUntilNextSpawn;


    public UnitManager(int lanes, List<GameObject> reserves, bool isEnemy, int zCoordinate)
    {
        this.numberOfLanes = lanes;
        this.numberOfReserves = numberOfReserves;
        this.isEnemy = isEnemy;
        this.zCoordinate = zCoordinate;

        GameObject floor = GameObject.FindGameObjectWithTag("floor");

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
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (numberOfReserves > 0)
        {
            for (int i = 0; i < numberOfLanes; ++i)
            {
                if (laneIsEmpty(i))
                {
                    spawn(i);
                }
            }
        }
        
        //if (numberOfReserves > 0 && )
        //{
        //    spawn();
        //}
    }

    bool laneIsEmpty(int lane)
    {
        foreach(UnitController unit in units)
        {
            float unitSizeZ = unit.GetComponent<Renderer>().bounds.size.z;
            if (unit.lane == lane && Mathf.Abs(unitSizeZ - zCoordinate) > unitSizeZ)
                return true;
        }
        return false;
    }

    public void spawn(int laneNumber)
    {

        UnitController newUnit = new UnitController();
        units.Add(newUnit);
        Instantiate(newUnit, spawnPositions[laneNumber]);
        

        // TODO: spawn

    }
}
