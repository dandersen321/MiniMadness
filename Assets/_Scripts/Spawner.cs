using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner {

    private Vector3 position;
    private int numberOfUnitsPerTick;
    private float secondsBetweenTick;
    private int numberOfUnitsToSpawn;
    private UnitController unit;
    private float timeUntilNextSpawn = 0;

    public Spawner(Vector3 position, int numberOfUnitsPerTick, int secondsBetweenTick, int numberOfUnitsToSpawn)
    {
        this.position = position;
        this.numberOfUnitsPerTick = numberOfUnitsPerTick;
        this.secondsBetweenTick = secondsBetweenTick;
        this.numberOfUnitsToSpawn = numberOfUnitsToSpawn;
    }

    public void update()
    {
        if (timeUntilNextSpawn <= Time.time)
        {
            spawn();    
        }
    }

    public void spawn()
    {
        timeUntilNextSpawn = Time.time + secondsBetweenTick;
        // TODO: spawn

    }

}
