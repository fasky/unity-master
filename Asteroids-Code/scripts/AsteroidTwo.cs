using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kyle Fasanella
//Class for individual asteroids(second type)

public class AsteroidTwo : MonoBehaviour
{

    public float rotation;
    public float angle;
    public Camera cam;
    private Vector3 objPos; //for wrap
    private Vector3 wrapPos; //for wrapping
    public Vector3 asteroidPosition;
    public Vector3 velocity;
    public Vector3 distance;
    public float mag;
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {
        asteroidPosition = transform.position;
        angle = Random.Range(-1.3f, 1.4f); // angle to turn by
        velocity = new Vector3(Random.Range(-.04f, .05f), Random.Range(-.04f, .05f), 0); //for the asteroids movement
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        objPos = cam.WorldToViewportPoint(transform.position); //for wrapping - get objects location relative to screen edges 
        Wrap();
        Rotate();
        Move();
        //Collision();
    }

    //move object constantly
    void Move()
    {
        asteroidPosition += velocity;
        transform.position = asteroidPosition;
    }

    //rotate object constantly
    void Rotate()
    {
        rotation += angle;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    //wrap object around screen when exits borders
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
            asteroidPosition = transform.position;
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
            asteroidPosition = transform.position;
        }

    }

    /*void Collision()
    {
        for (int i = 0; i < GameObject.Find("Manager").gameObject.GetComponent<Bullets>().bullets.Capacity; i++)
        {
            //get distance
            if (GameObject.Find("Manager").gameObject.GetComponent<Bullets>().bullets[i])
            {
                distance = GetComponent<SpriteInfo>().center - GameObject.Find("Manager").gameObject.GetComponent<Bullets>().bullets[i].GetComponent<SpriteInfo>().center;
                mag = distance.magnitude; //get its magnitude and compare
                if (mag < (GetComponent<SpriteInfo>().radius + GameObject.Find("Manager").gameObject.GetComponent<Bullets>().bullets[i].GetComponent<SpriteInfo>().radius))
                {
                    GameObject.Find("Manager").gameObject.GetComponent<Bullets>().score += 50;
                    Destroy(GameObject.Find("Manager").gameObject.GetComponent<Bullets>().bullets[i]);
                    Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                    GameObject.Find("Text").GetComponent<Text>().text = GameObject.Find("Manager").gameObject.GetComponent<Bullets>().score.ToString();
                    Destroy(gameObject);
                }
            }

        }
        
    }*/

    //on destroy - remove from list
    void OnDestroy()
    {
        GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids.Remove(gameObject);
    }

}
