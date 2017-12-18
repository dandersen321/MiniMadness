using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitAnimations
{
    none,
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
    //private float movementSpeed = 0.05f;
    private float movementSpeed = 0.15f;



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
    //private float attackCooldown;
    private float attackCooldownMin = 2;
    private float attackCooldownMax;
    private float timeUntilNextAttack = 0;

    private UnitController attackTarget = null;
    // TODO: set attackTimeLength to length of animation or vice versa
    private float attackTimeLength = 0.7f;
    private float attackingETA = 0;

    private float defenderArmyMaxXPosition;

    private UnitAnimations queuedAnim = UnitAnimations.none;

    public void init(UnitReserve reserve, bool isEnemy, float defenderArmyMaxXPosition)
    {
        this.lane = reserve.lane;
        this.isBaddie = isEnemy;
        this.zDirection = isEnemy ? 1 : -1;
        this.health = 100;
        this.defenderArmyMaxXPosition = defenderArmyMaxXPosition;
        //if(isBaddie)
        //{
        //    this.health /= 2;
        //}
        this.attackDamage = 20f;
        //this.attackCooldown = 2;
        //this.attackCooldownMin = 3f;
        //this.attackCooldownMin = 2f;
        this.attackRadius = 2.25f;
        if (!isBaddie)
        {
            // give friendly units first hit
            this.attackRadius += 0.1f;
        }
    }

    //private float queuedAnimTime = 0;
    //private UnitAnimations currentAnim = UnitAnimations.none;
    //private UnitAnimations queuedAnim = UnitAnimations.none;

    //public bool hitThisRound;

    //private float timeSinceLastHit = 0;
    //private bool hitReplaying = false;

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

    bool importantAnimIsPlaying()
    {
        bool animPlaying = false;
        List<string> nonIgnorableAnims = new List<string> { "Death", "Attack01", "Attack02", "GetHit" };
        foreach (string animCross in nonIgnorableAnims)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animCross))
            {
                animPlaying = true;
                break;
            }
        }
        return animPlaying;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;

        if(queuedAnim != UnitAnimations.none)
        {
            if(!importantAnimIsPlaying())
            {
                playAnimation(queuedAnim);
                queuedAnim = UnitAnimations.none;
            }
        }

        if(attackTarget != null && attackingETA < Time.time)
        {
            Debug.Log("Hit target!");
            attackTarget.takeDamage(attackDamage);
            attackTarget = null;
        }

        //hitThisRound = false;

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle").ToString());

        //if (queuedAnim != UnitAnimations.none && queuedAnimTime < Time.time)
        //{
        //    Debug.Log("Playing queued anim");
        //    playAnimation(queuedAnim);
        //}


        //screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        //if (rb.velocity.y < 0)
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

            float bufferTime = 0;
            //if (this.isBaddie && this.health < 100)
            //    bufferTime = 0.2f;

            if(timeUntilNextAttack + bufferTime < Time.time)
                attack(closestEnemy);
        }
        else
        {
            if(isBaddie || (this.transform.position.x < defenderArmyMaxXPosition))
            {
                move();
            }
            
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

        //if(hitThisRound)
        //{
        //    playAnimation(UnitAnimations.getHit);
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
        //targetUnit.takeDamage(attackDamage);
        attackTarget = targetUnit;
        attackingETA = Time.time + attackTimeLength;
        //timeUntilNextAttack = Time.time + Random.Range(attackCooldownMin, attackCooldownMax);
        timeUntilNextAttack = Time.time + attackCooldownMin;

    }

    public void playAnimation(UnitAnimations anim)
    {
        List<UnitAnimations> overrideAnims = new List<UnitAnimations>() { UnitAnimations.attack1, UnitAnimations.attack2, UnitAnimations.death };
        List<UnitAnimations> ignore = new List<UnitAnimations>() { UnitAnimations.run, UnitAnimations.walk};
        //if (!ignore.Contains(anim))
        //{
        //    if (nextAnimTime != 0 && nextAnimTime > Time.time && !overrideAnims.Contains(anim))
        //    {
        //        Debug.Log("Can't play current animation because one is already going. Queing it up.");

        //        nextAnim = anim;
        //        return;
        //    }
        //    else
        //    {
        //        nextAnim = UnitAnimations.none;
        //        nextAnimTime = Time.time + 2.5f;
        //    }
        //}

        //Debug.Log("attacking!");

        

        string animName = null;
        float animLength = -1f;
        if (anim == UnitAnimations.death)
            animName = "Death";
        else if (anim == UnitAnimations.walk)
            animName = "Walk";
        else if (anim == UnitAnimations.run)
            animName = "Run";
        else if (anim == UnitAnimations.attack1)
        {
            animLength = this.attackTimeLength;
            Debug.Log("Attacking!");
            //animName = "Attack-Hit";
            animName = "Attack01";
            //animName = "GetHit";
        }
        else if (anim == UnitAnimations.attack2)
            animName = "Attack02";
        else if (anim == UnitAnimations.getHit)
            animName = "GetHit";

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).fullPathHash.ToString());

        if (animName != null)
        {
            //animator.CrossFade(animName, 1f);
            //Debug.Log(animName);
            //Debug.
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle").ToString());
            //List<string> nonIgnorableAnims = new List<string> { "Death", "Attack01", "Attack02", "GetHit" };
            //bool animPlaying = false;
            //foreach (string animCross in nonIgnorableAnims)
            //{
            //    if(animator.GetCurrentAnimatorStateInfo(0).IsName(animCross))
            //    {
            //        animPlaying = true;
            //        break;
            //    }
            //}
            //if(animLength != -1f)
            //{
            //    animator.Play(animName, 0, animLength);
            //}
            //else
            //{
            //    animator.Play(animName);
            //}
            if(anim == UnitAnimations.getHit || anim == UnitAnimations.attack1)
            {
                animator.CrossFade(animName, 0.3f);
            }
            else
            {
                animator.Play(animName);
            }
            

            //if(!importantAnimIsPlaying())
            //{
            //    animator.Play(animName);
            //}
            //else if(queuedAnim == UnitAnimations.none)
            //{
            //    queuedAnim = anim;
            //}
            //else
            //{
            //    Debug.Log("Skipping anim");
            //}



            //Debug.Log(crossOver.ToString() + "->" + animName);
            //if (!animPlaying)
            //{
            //    //animator.CrossFade(animName, 1f);
            //    animator.Play(animName);
            //    if(anim == queuedAnim)
            //    {
            //        queuedAnim = UnitAnimations.none;
            //    }
            //}
            //else
            //{
            //    Debug.Log("Defering anmi");
            //    queuedAnimTime = Time.time + 0.5f;
            //    queuedAnim = anim;

            //}

            //if (timeSinceLastHit < Time.time && timeSinceLastHit > Time.time - 0.1f && !hitReplaying)
            //{
            //    animator.CrossFade(animName, 0.5f);
            //    hitReplaying = true;
            //}
        }

    }



    private void takeDamage(float damage)
    {
        if (!alive)
            return;
        health -= damage;
        if(health > 0)
        {
            //this.hitThisRound = true;
            playAnimation(UnitAnimations.getHit);
            //timeSinceLastHit = Time.time;
            //hitReplaying = false;
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
