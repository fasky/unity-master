using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Kyle Fasanella
//Class for collision detection of asteroids and ship

public class CollisionDetection : MonoBehaviour {

    private SpriteInfo sprInfo1;
    private SpriteInfo sprInfo2;
    Vector3 distance;
    float mag;
    public GameObject explosion;
    public GameObject ship; //for referencing gameobjects
    int i;
    public bool hit;

    // Use this for initialization
    void Start () {
        sprInfo1 = ship.GetComponent<SpriteInfo>();
        hit = false;
	}

    // Update is called once per frame
    void Update() {

        //for each asteroid in the list
        for(i = 0; i < GetComponent<Asteroids>().asteroids.Capacity; i++)
        {
            //check if it exists
            if (gameObject.GetComponent<Asteroids>().asteroids[i])
            {
                //then get the sprite info to use for calculating distance, then magnitude, then comparing the magnitude of the distance between the centers and the sum of the radii
                sprInfo2 = gameObject.GetComponent<Asteroids>().asteroids[i].GetComponent<SpriteInfo>();
                //get distance
                distance = sprInfo1.center - sprInfo2.center;
                mag = distance.magnitude; //get its magnitude and compare
                if (mag < (sprInfo1.radius + sprInfo2.radius))
                {
                    if(hit == false)
                    {
                        ship.GetComponent<Vehicle>().lives--;
                        Instantiate(explosion, GetComponent<Asteroids>().asteroids[i].transform.position, Quaternion.identity);
                        Destroy(GetComponent<Asteroids>().asteroids[i]);
                        hit = true;
                    }

                }
            }

        }

        //if you press escape, exit app
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        /*for (j = 0; j < GetComponent<AsteroidTwos>().asteroidTwos.Capacity; j++)
        {
            if (gameObject.GetComponent<AsteroidTwos>().asteroidTwos[j])
            {
                sprInfo3 = gameObject.GetComponent<AsteroidTwos>().asteroidTwos[j].GetComponent<SpriteInfo>();
                //get distance
                distance = sprInfo1.center - sprInfo3.center;
                mag = distance.magnitude; //get its magnitude and compare
                if (mag < (sprInfo1.radius + sprInfo3.radius))
                {
                    ship.GetComponent<Vehicle>().lives--;
                    Instantiate(explosion, GetComponent<AsteroidTwos>().asteroidTwos[j].transform.position, Quaternion.identity);
                    Destroy(GetComponent<AsteroidTwos>().asteroidTwos[j]);
                }
            }
        }*/

    }
}
