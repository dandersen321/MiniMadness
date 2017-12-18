using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private List<UnitController> enemies;
    private UnitController grabbedBaddie;
    // Use this for initialization

    private UnitManager defenderManager;
    private UnitManager baddieManager;

    //public float gameTimer;
    private float startTime;


    public GameObject soldierPrefab;

    public PlayerManager playerManager;

    public GameObject levelFactoryObject;
    private LevelFactory levelFactory;
    private Level currentLevel;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        grabbedBaddie = null;
        enemies = new List<UnitController>();

        levelFactory = levelFactoryObject.GetComponent<LevelFactory>();


        //ArcherAbility archerAbility = new ArcherAbility()
        //playerManager = new PlayerManager();
        playerManager = levelFactory.createLevel(1);

        int i = 0;
        //foreach (var obj in GameObject.FindGameObjectsWithTag("BaddieUnit"))
        //{
        //    UnitController unit = obj.GetComponent<UnitController>();
        //    unit.name = "Unit" + i.ToString();
        //    i += 1;
        //    enemies.Add(unit);

        //}
        //startTime = Time.time;
        defenderManager = new UnitManager(8, createDefenderManager(), false);
        baddieManager = new UnitManager(8, createBaddieManager(), true);
    }

    List<UnitReserve> createDefenderManager()
    {
        List<UnitReserve> defenders = new List<UnitReserve>();
        //defenders.Add(new UnitReserve(0, 0, soldierPrefab));
        //defenders.Add(new UnitReserve(0.2f, 1, soldierPrefab));
        //defenders.Add(new UnitReserve(0.4f, 2, soldierPrefab));
        //defenders.Add(new UnitReserve(0.6f, 3, soldierPrefab));
        //defenders.Add(new UnitReserve(0.8f, 4, soldierPrefab));
        //defenders.Add(new UnitReserve(0.8f, 5, soldierPrefab));
        //defenders.Add(new UnitReserve(0.8f, 6, soldierPrefab));
        //defenders.Add(new UnitReserve(0.8f, 7, soldierPrefab));

        defenders.Add(new UnitReserve(0.6f, 3, soldierPrefab));

        return defenders;
    }

    List<UnitReserve> createBaddieManager()
    {
        List<UnitReserve> enemies = new List<UnitReserve>();
        //enemies.Add(new UnitReserve(0, 0, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 1, soldierPrefab));
        //enemies.Add(new UnitReserve(0.4f, 2, soldierPrefab));
        //enemies.Add(new UnitReserve(0.6f, 3, soldierPrefab));
        //enemies.Add(new UnitReserve(0.8f, 4, soldierPrefab));
        //enemies.Add(new UnitReserve(0.8f, 5, soldierPrefab));

        enemies.Add(new UnitReserve(0.6f, 3, soldierPrefab));

        //enemies.Add(new UnitReserve(0.1f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.8f, 7, soldierPrefab));
        //enemies.Add(new UnitReserve(0.1f, 0, soldierPrefab));
        //enemies.Add(new UnitReserve(0.3f, 1, soldierPrefab));
        //enemies.Add(new UnitReserve(0.5f, 2, soldierPrefab));
        //enemies.Add(new UnitReserve(0.6f, 3, soldierPrefab));
        //enemies.Add(new UnitReserve(0.9f, 4, soldierPrefab));
        //enemies.Add(new UnitReserve(0.9f, 5, soldierPrefab));

        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));
        //enemies.Add(new UnitReserve(0.2f, 6, soldierPrefab));

        //enemies.Add(new UnitReserve(0.9f, 7, soldierPrefab));

        return enemies;
    }

    // Update is called once per frame
    void Update()
    {
        //gameTimer = Time.time - startTime;

        baddieManager.updateUnits();
        defenderManager.updateUnits();


        playerManager.updateAbility();

        //if (Input.GetMouseButton(0))
        //{
        //    mouseDownAction();
        //}

        //if(Input.GetMouseButtonUp(0))
        //{
        //    mouseUpAction();
        //    //Debug.Log("Released!");
        //    //grabbedBaddie = null;
        //}

        //if(Input.GetMouseButtonDown(0))
        //{
        //    //Debug.Log(Input.mousePosition.ToString());
        //    mouseDownAction();
        //}

    }

    void mouseDownAction()
    {
        //UnitController unit = grabbedBaddie != null ? grabbedBaddie : FindClosestBaddie(Input.mousePosition);
        //if (unit == null)
        //    return;
        //grabbedBaddie = unit;
        //Debug.Log("Grabbed " + unit.name);
        //unit.grab();

        //if(activeAbility == null)
        //{
        //    activeAbility = new ArcherFire();
        //}

        //if (activeAbility.GetType() == typeof(ArcherFire))
        //{
        //    Vector3 startMousePosition = new Vector3(0, 5, 0);
        //    Vector3 currentMousePosition = new Vector3(10, 5, 0);
        //    ArcherFire.UpdateTrajectory(startMousePosition, currentMousePosition);
        //}

        //playerManager.mouseIsDown();


    }

    void mouseUpAction()
    {
        //if(activeAbility != null)
        //{
        //    if(activeAbility.GetType() == typeof(ArcherFire))
        //    {
        //        ArcherFire.
        //    }
        //}

        //playerManager.mouseIsUp();
    }

    public UnitController FindClosestBaddie(Vector3 position)
    {
        if(grabbedBaddie != null)
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
        //gos = GameObject.FindGameObjectsWithTag("Baddie");
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


//public class TouchCreator
//{
//    static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
//    static Dictionary<string, FieldInfo> fields;

//    object touch;

//    public float deltaTime { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
//    public int tapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
//    public TouchPhase phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
//    public Vector2 deltaPosition { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
//    public int fingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
//    public Vector2 position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
//    public Vector2 rawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

//    public Touch Create()
//    {
//        return (Touch)touch;
//    }

//    public TouchCreator()
//    {
//        touch = new Touch();
//    }

//    static TouchCreator()
//    {
//        fields = new Dictionary<string, FieldInfo>();
//        foreach (var f in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
//        {
//            fields.Add(f.Name, f);
//            Debug.Log("name: " + f.Name);
//        }
//    }
//}
