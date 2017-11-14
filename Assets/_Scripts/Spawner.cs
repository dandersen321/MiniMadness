//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Spawner: MonoBehaviour
//{

//    private Vector3 position;
//    private int numberOfUnitsPerTick;
//    private float secondsBetweenTick;
//    private int numberOfUnitsToSpawn;
//    private UnitController unit;
//    private float timeUntilNextSpawn = 0;
//    private GameObject floor;
//    private List<Vector3> spawnPositions;
//    private bool enemySpawner;

//    public Spawner(Vector3 position, int numberOfUnitsPerTick, int secondsBetweenTick, int numberOfUnitsToSpawn, bool enemySpawner)
//    {
//        this.position = position;
//        this.numberOfUnitsPerTick = numberOfUnitsPerTick;
//        this.secondsBetweenTick = secondsBetweenTick;
//        this.numberOfUnitsToSpawn = numberOfUnitsToSpawn;
//        this.enemySpawner = enemySpawner;
//        floor = GameObject.FindGameObjectWithTag("floor");

//        float height = floor.GetComponent<Renderer>().bounds.size.x;
//        float buffer = 5;
//        float interval = (height-buffer) / numberOfUnitsToSpawn;
//        spawnPositions = new List<Vector3>();
//        for(int i = 0; i < numberOfUnitsToSpawn; ++i)
//        {
//            spawnPositions.Add(new Vector3(interval * i + buffer, position.y, position.z));
//        }
//    }

//    public void update()
//    {
//        if (numberOfUnitsToSpawn > 0 && timeUntilNextSpawn <= Time.time)
//        {
//            spawn();    
//        }
//    }

//    public void spawn()
//    {
//        timeUntilNextSpawn = Time.time + secondsBetweenTick;
//        for(int i = 0; i < numberOfUnitsPerTick; ++i)
//        {
//            Instantiate(UnitController, )
//        }
        
//        // TODO: spawn

//    }

//}
