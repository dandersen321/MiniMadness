using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<UnitController> enemies;
    private UnitController grabbedEnemy;
    // Use this for initialization
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
    }

    // Update is called once per frame
    void Update()
    {

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

