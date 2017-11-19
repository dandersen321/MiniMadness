using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitAnimations
{
    
    walk,
    run,
    attack1,
    attack2,
    getHit,
    death,
    victory,
    


}

public class UnitController : MonoBehaviour {

    // private bool onGround;
    //private bool falling;
    private float fallSpeed = 0.25f;
    private float movementSpeed = 0.05f;



    public bool playerGrabbed;
    private bool playerLetGo = false;
    private bool falling;
    private Rigidbody rb;
    private Animator animator;
    public bool alive;
    private Vector3 startPosition;

    // what direction baddie/goodie is facing
    private int zDirection;

    public string name;

    public int lane;

    public bool isBaddie;

    private float attackRadius;
    private float health;
    private float attackDamage;
    private float attackCooldown;
    private float timeUntilNextAttack = 0;

    // Use this for initialization
    void Start()
    {
        //onGround = true;
        falling = false;
        playerGrabbed = false;
        alive = true;
        playerLetGo = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;
        //screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        //if(rb.velocity.y < 0)
        //{
        //    Debug.Log("Falling!!!");
        //}

        //rb.velocity = transform.forward * movementSpeed;
        

        if (playerGrabbed && !Input.GetMouseButton(0))
            playerLetGo = true;

        

        if (playerLetGo && alive)
        {
            //rb.useGravity = false;
            

            {
                transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);
                if (transform.position.y <= 0.5f)
                {
                    //die();
                    takeDamage(150f);
                }
                //rb.useGravity = true;
            }
            
        }

        var closestEnemy = getOpponentInAttackRadius();
        if (closestEnemy != null)
        {
            if(timeUntilNextAttack < Time.time)
                attack(closestEnemy);
        }
        else
        {
            move();
        }
        

        //if (transform.position.y < -1f)
        //{
        //    transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        //}

        //if (rb.velocity.y < 0 && playerGrabbed)
        //{
        //    falling = true;
        //    //Debug.Log("Falling!!!");
        //}

        //if (rb.velocity.y == 0 && falling)
        //{
        //    die();
        //}

    }

    private void move()
    {
        rb.transform.Translate(Vector3.forward * movementSpeed);
        playAnimation(UnitAnimations.run);
    }

    private UnitController getOpponentInAttackRadius()
    {
        // TODO: consider direction?
        var colliders = Physics.OverlapBox(this.rb.position, this.GetComponent<Collider>().bounds.size + new Vector3(0, 0, attackRadius));
        // 1 to ignore floor
        float smallestDistance = Mathf.Infinity;
        UnitController closest = null;

        foreach (var collider in colliders)
        {
            var unit = collider.GetComponentInParent<UnitController>();
            if(unit == null || !unit.alive || unit.isBaddie == this.isBaddie)
            {
                continue;
            }
            Vector3 diff = unit.rb.position - rb.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < smallestDistance)
            {
                closest = unit;
                smallestDistance = curDistance;
            }
        }

        return closest;

    }

    private void attack(UnitController targetUnit)
    {
        // TODO: if unit hasn't moved or attacked in a while, die?
        this.playAnimation(UnitAnimations.attack1);
        targetUnit.takeDamage(attackDamage);
        timeUntilNextAttack = Time.time + attackCooldown;

    }

    public void playAnimation(UnitAnimations anim)
    {
        string animName = null;
        if (anim == UnitAnimations.death)
            animName = "Death";
        else if (anim == UnitAnimations.walk)
            animName = "Walk";
        else if (anim == UnitAnimations.run)
            animName = "Run";
        else if (anim == UnitAnimations.attack1)
            animName = "Attack01";
        else if (anim == UnitAnimations.attack2)
            animName = "Attack02";
        else if (anim == UnitAnimations.getHit)
            animName = "GetHit";

        if (animName != null)
            //Debug.Log(animName);
            animator.Play(animName);
    }

    public void init(UnitReserve reserve, bool isEnemy)
    {
        this.lane = reserve.lane;
        this.isBaddie = isEnemy;
        this.zDirection = isEnemy ? 1 : -1;
        this.health = 100;
        //if(isBaddie)
        //{
        //    this.health /= 2;
        //}
        this.attackDamage = 20f;
        this.attackCooldown = 1;
        this.attackRadius = 2.25f;
        if(!isBaddie)
        {
            // give friendly units first hit
            this.attackRadius += 0.1f;
        }
    }

    private void takeDamage(float damage)
    {
        if (!alive)
            return;
        health -= damage;
        if(health > 0)
        {
            //playAnimation(UnitAnimations.getHit);
        }
        else
        {
            die();
        }
    }

    public void grab()
    {

        //Debug.Log("MouseButtonDown yo");
        //Debug.Log(String.Format("MouseX: {0} MouseY: {1}", Input.mousePosition.x, Input.mousePosition.y));
        //Debug.Log(String.Format("ObjectX: {0} ObjectY: {1}", transform.position.x, transform.position.y));
        var objectScreenPosition = Camera.main.WorldToScreenPoint(startPosition);
        //var mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectWorldPosition.z);
        //var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 rayPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectScreenPosition.z);
        var mouseWorldPoint = Camera.main.ScreenToWorldPoint(rayPosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(rayPosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        //transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        //var moveUp = transform.position.y - hit.point.y;
        //transform.parent.gameObject.transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

        //transform.parent.gameObject.transform.position = new Vector3(transform.parent.gameObject.transform.position.x, Mathf.Max(0.5f, mouseWorldPoint.y), transform.parent.gameObject.transform.position.z);
        transform.position = new Vector3(transform.position.x, Mathf.Max(0.5f, mouseWorldPoint.y), transform.position.z);
        //Debug.Log(transform.position.y);
        //Debug.Log(hit.point.y);
        //playerGrabbed = true;
        //rb.useGravity = false;
        //rb.velocity = Vector3.zero;
        //}
        playerGrabbed = true;

        this.playAnimation(UnitAnimations.getHit);


    }

    private void die()
    {
        Debug.Log("dead!!!");
        this.playAnimation(UnitAnimations.death);
        //animator.Play("Death");
        alive = false;
        GetComponent<Collider>().enabled = false;

    }

    //private void OnMouseDown()
    //{
    //    Debug.Log("Footman on mouse down");
    //    playerGrabbed = true;
    //    startPosition = transform.position;


    //    //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

    //    //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    //    //transform.position = curPosition;

    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    Debug.Log("MouseButtonDown yo");
    //    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    //    RaycastHit hit;

    //    //    if (Physics.Raycast(ray, out hit))
    //    //    {
    //    //        var targetPosition = hit.point;
    //    //        transform.position = targetPosition;
    //    //    }
    //    //}

    //}

    //private void OnMouseUp()
    //{
    //    if (playerGrabbed)
    //        playerLetGo = true;
    //    playerGrabbed = false;
        
    //}

    private void OnMouseDrag()
    {

    }
}
