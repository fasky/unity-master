using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//Class for handling individual bombs/enemy bullets

public class Bomb : MonoBehaviour {

    public Camera cam;
    private Vector3 objPos; //for wrap
    private Vector3 wrapPos; //for wrapping
    public Vector3 bombPosition;
    public Vector3 velocity;
    public Vector3 distance;
    public float mag;
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {
        bombPosition = transform.position;
        velocity = GameObject.Find("Ship").GetComponent<SpriteInfo>().center - transform.position; //bomb flies towards ship, slower when closer, faster when farther
        velocity = velocity/100;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        objPos = cam.WorldToViewportPoint(transform.position); //for wrapping - get objects location relative to screen edges 
        Wrap();
        Move();
        //Collision();
    }

    //move the bomb towards where the ship was when it instantiated
    void Move()
    {
        bombPosition += velocity;
        transform.position = bombPosition;
    }

    //wrap object on screen when it goes outside borders
    void Wrap()
    {
        //if outside x, make it go to other side of screen
        if (objPos.x < -.1 || objPos.x > 1.1)
        {
            //get current position
            wrapPos = transform.position;
            //set it accordingly

            if (objPos.x < -.1)
            {
                wrapPos.x = -wrapPos.x - 1f;
            }
            if (objPos.x > 1.1)
            {
                wrapPos.x = -wrapPos.x + 1f;
            }

            //set transform to new position, and vehicle position to transform position
            transform.position = wrapPos;
            bombPosition = transform.position;
        }
        //if outside y, go to other side of screen
        if (objPos.y < -.25 || objPos.y > 1.25)
        {
            wrapPos = transform.position;
            if (objPos.y > 1.25)
            {
                wrapPos.y = -wrapPos.y + 1f;
            }
            if (objPos.y < -.25)
            {
                wrapPos.y = -wrapPos.y - 1f;
            }

            transform.position = wrapPos;
            bombPosition = transform.position;
        }

    }

    //on destroy, remove from asteroid list
    void OnDestroy()
    {
        GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids.Remove(gameObject);
    }
}
