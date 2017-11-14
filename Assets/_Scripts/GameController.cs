using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<UnitController> enemies;
    private UnitController grabbedEnemy;
    // Use this for initialization

    private UnitManager defenderManager;
    private UnitManager enemyManager;

    //public float gameTimer;
    private float startTime;


    public GameObject soldierPrefab;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        grabbedEnemy = null;
        enemies = new List<UnitController>();
        int i = 0;
        foreach (var obj in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            UnitController unit = obj.GetComponent<UnitController>();
            unit.name = "Unit" + i.ToString();
            i += 1;
            enemies.Add(unit);

        }
        //startTime = Time.time;
        defenderManager = new UnitManager(5, createDefenderManager(), false);
        enemyManager = new UnitManager(5, createDefenderManager(), true);
    }

    List<UnitReserve> createDefenderManager()
    {
        List<UnitReserve> defenders = new List<UnitReserve>();
        defenders.Add(new UnitReserve(0, 0, soldierPrefab));
        defenders.Add(new UnitReserve(0.2f, 1, soldierPrefab));
        defenders.Add(new UnitReserve(0.4f, 2, soldierPrefab));
        defenders.Add(new UnitReserve(0.6f, 3, soldierPrefab));
        defenders.Add(new UnitReserve(0.8f, 4, soldierPrefab));

        return defenders;
    }

    // Update is called once per frame
    void Update()
    {
        //gameTimer = Time.time - startTime;

        enemyManager.updateUnits();
        defenderManager.updateUnits();


        if (Input.GetMouseButton(0))
        {
            mouseDownAction();
        }

        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Released!");
            grabbedEnemy = null;
        }

        //if(Input.GetMouseButtonDown(0))
        //{
        //    //Debug.Log(Input.mousePosition.ToString());
        //    mouseDownAction();
        //}


    }

    void mouseDownAction()
    {
        //foreach(var unit in enemies)
        //{
        //    Debug.Log(unit.transform.position);
        //}
        //Debug.Log(grabbedEnemy.ToString());
        UnitController unit = grabbedEnemy != null ? grabbedEnemy : FindClosestEnemy(Input.mousePosition);
        grabbedEnemy = unit;
        Debug.Log("Grabbed " + unit.name);
        unit.grab();
        //Debug.Log("Mouse down");
    }

    public UnitController FindClosestEnemy(Vector3 position)
    {
        if(grabbedEnemy != null)
        {
            throw new System.Exception("why?");
        }
        float smallestDistance = Mathf.Infinity;
        UnitController closest = null;
        foreach(var unit in enemies)
        {
            if (!unit.alive)
                continue;
            var unitScreenPoint = Camera.main.WorldToScreenPoint(unit.transform.position);
            //var unitScreenPoint = Camera.main.ScreenToViewportPoint(unit.transform.position);
            //var unitScreenPoint = Camera.main.WorldToViewportPoint(unit.transform.position);
            unitScreenPoint = new Vector3(unitScreenPoint.x, unitScreenPoint.y, 0);
            Vector3 diff = unitScreenPoint - position;
            float curDistance = diff.sqrMagnitude;
            //if (unit.name.Contains("57"))
            //{
            //    Debug.Log("mouse-" + position.ToString());
            //    //Debug.Log("unit-rect" + unit.transform.)
            //    Debug.Log("unit-" + unitScreenPoint.ToString());
            //    Debug.Log(unit.name + "-" + curDistance.ToString());
            //}
            if (curDistance < smallestDistance)
            {
                closest = unit;
                smallestDistance = curDistance;
            }
        }
        return closest;
        //GameObject[] gos;
        //gos = GameObject.FindGameObjectsWithTag("Enemy");
        //GameObject closest = null;
        //float distance = Mathf.Infinity;
        //Vector3 position = transform.position;
        //foreach (GameObject go in gos)
        //{
        //    Vector3 diff = go.transform.position - position;
        //    float curDistance = diff.sqrMagnitude;
        //    if (curDistance < distance)
        //    {
        //        closest = go;
        //        distance = curDistance;
        //    }
        //}
        //return closest;
    }

}

