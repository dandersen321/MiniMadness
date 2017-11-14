﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    // private bool onGround;
    //private bool falling;
    private float fallSpeed = 0.25f;

    public bool playerGrabbed;
    private bool playerLetGo = false;
    private bool falling;
    //private Rigidbody rb;
    private Animator animator;
    public bool alive;
    private Vector3 startPosition;

    public string name;

    public int lane;

    // Use this for initialization
    void Start()
    {
        //onGround = true;
        falling = false;
        playerGrabbed = false;
        alive = true;
        playerLetGo = false;
        //rb = GetComponent<Rigidbody>();
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

        if (playerGrabbed && !Input.GetMouseButton(0))
            playerLetGo = true;

        

        if (playerLetGo && alive)
        {
            //rb.useGravity = false;
            

            {
                transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);
                if (transform.position.y <= 0.5f)
                {
                    die();
                }
                //rb.useGravity = true;
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


    }

    private void die()
    {
        Debug.Log("dead!!!");
        animator.Play("Death");
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
