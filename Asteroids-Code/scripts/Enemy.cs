using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//Class for handling enemy behaviour, shooting.

public class Enemy : MonoBehaviour {

    public Camera cam;
    private Vector3 objPos; //for wrap
    private Vector3 wrapPos; //for wrapping
    public Vector3 enemyPosition;
    public Vector3 velocity;
    public Vector3 distance;
    public float mag;
    public GameObject explosion;
    public int fire;
    public GameObject bomb;

    // Use this for initialization
    void Start()
    {
        fire = 150;
        enemyPosition = transform.position;
        velocity = new Vector3(Random.Range(-.05f, .06f), Random.Range(-.05f, .06f), 0); //random velocity
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //fire a bomb every so often
        fire--;
        if(fire < 0)
        {
            GameObject.Find("Manager").GetComponent<Asteroids>().asteroids.Add(Instantiate(bomb,gameObject.transform.position,Quaternion.identity));
            fire = 60;
        }
        objPos = cam.WorldToViewportPoint(transform.position); //for wrapping - get objects location relative to screen edges 
        Wrap();
        Move();
    }

    //move ship in same direction
    void Move()
    {
        enemyPosition += velocity;
        transform.position = enemyPosition;
    }

    //wrap on screen
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
            enemyPosition = transform.position;
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
            enemyPosition = transform.position;
        }

    }

    //on destroy, remove from list
    void OnDestroy()
    {
        GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids.Remove(gameObject);
    }
}
