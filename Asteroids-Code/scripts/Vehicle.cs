using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kyle Fasanella
//class for vehicle movement
public class Vehicle : MonoBehaviour {
    
    public Vector3 vehiclePosition;
    //public float speed;
    public Vector3 velocity;
    public Vector3 direction;
    public Vector3 acceleration;
    public float accRate;
    public float maxSpeed;
    public float decRate; 
    public float rotation;
    public float angle;
    public Camera cam; //for wrap
    private Vector3 objPos; //for wrap
    private Vector3 wrapPos; //for wrapping
    public GameObject bullet; //shoot these
    private int cooldown; //cooldown for shooting
    private int checkedCount; //number of times lives have been taken
    public int lives; 
    public GameObject explosion; //for ship death
    public int invincibleTime; //for spawning in/after getting hit

    // Use this for initialization
    void Start () {
        checkedCount = 0;
        lives = 3; //3 lives for game
        vehiclePosition = transform.position;
        cooldown = 60;
        invincibleTime = 240; //invincible to start with
    }
	
	// Update is called once per frame
	void Update () {
        invincibleTime--;
        invincibleCheck();
        objPos = cam.WorldToViewportPoint(transform.position); //for wrapping - get objects location relative to screen edges
        LifeCheck();
        Move();
        Wrap();

        //cooldown and shooting
        cooldown++;
        if (Input.GetKey(KeyCode.Space))
        {
            if(cooldown > 16)
            {
                Shoot();
                cooldown = 0;
            }
        }
	}

    //check if ship is invincible, check if invincible time is positive
    void invincibleCheck()
    {
        if (invincibleTime > 0)
        {
            GameObject.Find("Manager").GetComponent<CollisionDetection>().enabled = false;
            gameObject.GetComponent<SpriteInfo>().spr.color = Color.blue;
            GameObject.Find("TextShields").GetComponent<Text>().text = "SHIELDS UP"; //let user know they are invincible
        }
        else
        {
            GameObject.Find("TextShields").GetComponent<Text>().text = "";
            GetComponent<SpriteInfo>().spr.color = Color.white;
            GameObject.Find("Manager").GetComponent<CollisionDetection>().enabled = true;
        }
    }

    //Move method - detect key presses, rotate accordingly, if up is pressed accelerate, if not then lose velocity
    void Move()
    {
        //rotate
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation += angle;
            direction = Quaternion.Euler(0, 0, angle) * direction;
            velocity += acceleration;
      
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation -= angle;
            direction = Quaternion.Euler(0, 0, -angle) * direction;
            velocity += acceleration;

        }

        //accel
        if (Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Animator>().SetBool("Ship",true);
            GetComponent<Animator>().SetBool("ShipMoveFromStopping", true);
            GetComponent<Animator>().SetBool("ShipStop", false);
            acceleration = accRate * direction;
            velocity += acceleration;
        }
        else
        { //decelerate           
            GetComponent<Animator>().SetBool("Ship", false);
            GetComponent<Animator>().SetBool("ShipMoveFromStopping", false);
            GetComponent<Animator>().SetBool("ShipStop", true);
            acceleration = Vector3.zero;
            velocity= Vector3.Lerp(velocity, Vector3.zero, decRate * Time.deltaTime);
        }

        velocity += acceleration;
        vehiclePosition += velocity;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position = vehiclePosition;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    //wrap method - check to see if object is outside screen boundary.
    void Wrap()
    {
        //if outside x, make it go to other side of screen
        if(objPos.x < -.05 || objPos.x > 1.05)
        {
            //get current position
            wrapPos = transform.position;
            //set it accordingly

            if(objPos.x < -.05)
            {
                wrapPos.x = -wrapPos.x - 1f;
            }
            if(objPos.x > 1.05)
            {
                wrapPos.x = -wrapPos.x + 1f;
            }
            
            //set transform to new position, and vehicle position to transform position
            transform.position = wrapPos;
            vehiclePosition = transform.position;
        }
        //if outside y, go to other side of screen
        if (objPos.y < -.1 || objPos.y > 1.1)
        {
            wrapPos = transform.position;
            if(objPos.y > 1.1)
            {
                wrapPos.y = -wrapPos.y + 1f;
            }
            if (objPos.y < -.1)
            {
                wrapPos.y = -wrapPos.y - 1f;
            }

            transform.position = wrapPos;
            vehiclePosition = transform.position;
        }

    }

    //shoot method - fire bullet from object, timed destruction
    void Shoot()
    {
        Instantiate(bullet, transform.position + transform.up,transform.rotation);
    }

    //check number of lives. If it goes down, add to count checked and destroy iamge of vehicle - to show life lost.
    void LifeCheck()
    {
        if(lives == 2 && checkedCount == 0)
        {
           invincibleTime = 240;
           Destroy(GameObject.Find("life1"));
           checkedCount++;
           GameObject.Find("Manager").GetComponent<CollisionDetection>().hit = false;
        }
        if (lives == 1 && checkedCount == 1)
        {
            invincibleTime = 240;
            Destroy(GameObject.Find("life2"));
            checkedCount++;
            GameObject.Find("Manager").GetComponent<CollisionDetection>().hit = false;

        }
        if (lives == 0 && checkedCount == 2)
        {
            Destroy(GameObject.Find("life3"));
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject.Find("Text").GetComponent<Text>().text = "GAME OVER";
        }
    }

}
