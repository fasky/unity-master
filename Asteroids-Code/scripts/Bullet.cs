using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kyle Fasanella
//Class for handling individual bullets, their collision with enemies

public class Bullet : MonoBehaviour {

    public Manager handler;
    public Vector3 distance;
    public float mag;
    public GameObject explosion;

    // Use this for initialization
    void Start () {
        handler = GameObject.Find("Manager").GetComponent<Manager>(); //use for scoring
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.up * Time.deltaTime * 12; //go in direction ship was facing, since it wsas given the ships rotation
        Destroy(gameObject, .6f); //destroy after short time
        Collision();

	}

    //go through asteroid list
    void Collision()
    {
        for (int i = 0; i < GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids.Capacity; i++)
        {
            //if it exists
            if (GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i])
            {
                //get the distance between objects and the magnitude of that, then compare it to the radii sum.
                distance = GetComponent<SpriteInfo>().center - GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<SpriteInfo>().center;
                mag = distance.magnitude; //get its magnitude and compare
                if (mag < (GetComponent<SpriteInfo>().radius + GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<SpriteInfo>().radius))
                {
                    //if normal asteroid, 25 points
                    if (GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<Asteroid>())
                    {
                        GameObject.Find("Manager").gameObject.GetComponent<Manager>().score += 25;
                    }
                    //if enemy ship, 75 points
                    else if (GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<Enemy>())
                    {
                        GameObject.Find("Manager").gameObject.GetComponent<Manager>().score += 75;
                    }
                    //if bomb, 100  points
                    else if (GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<Bomb>())
                    {
                        GameObject.Find("Manager").gameObject.GetComponent<Manager>().score += 100;
                    }
                    //if second level asteroid, 50 points
                    else
                    {
                        GameObject.Find("Manager").gameObject.GetComponent<Manager>().score += 50;
                    }

                    //produce explosion, destroy asteroid, update score, destroy bullet
                    Instantiate(explosion, GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i].transform.position, Quaternion.identity);
                    Destroy(GameObject.Find("Manager").gameObject.GetComponent<Asteroids>().asteroids[i]);          
                    GameObject.Find("Text").GetComponent<Text>().text = GameObject.Find("Manager").gameObject.GetComponent<Manager>().score.ToString();
                    Destroy(gameObject);
                }
            }

        }

    }
}
