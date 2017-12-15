using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//for the flow field agents and their movement

public class Flower : Vehicle{

    public FlowField script;

    // Use this for initialization
    void Start () {
        vehiclePosition = transform.position;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        tempList = new List<GameObject>();
        timePassed = 30;
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        center = GameObject.Find("Bounder");
        script = manager.GetComponent<FlowField>();
    }
	
	// Update is called once per frame
    // in addition to the usual, it checks if they are past the borders and send them to the opposite side of map if so
	void Update () {

        timePassed++;

        //grab transoform pos
        vehiclePosition = transform.position;
        Bounce();

        wander = true;

        acceleration = CalcSteeringForces();

        //add acceleration to velocity
        velocity += acceleration * Time.deltaTime;

        //normlaize direction
        direction = velocity.normalized;

        //add velocity to position
        vehiclePosition += velocity * Time.deltaTime;

        //draw
        if (maxSpeed > 0)
        {
            transform.position = vehiclePosition;
        }

        Rotate();

        //check borders of map - send to other side
        if(transform.position.x < 0)
        {
            transform.position = new Vector3(200,transform.position.y,transform.position.z);
        }
        if (transform.position.x > 200)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.z > 200)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 200);
        }

        //reset acceleration
        acceleration = Vector3.zero;
    }

    // Calcualte sttering
    //if outside park, send it back in to the center, otherwise have it follow the field.
    public override Vector3 CalcSteeringForces()
    {
        steeringForce = Vector3.zero;
        if (outside)
        {
            steeringForce += Seek(manager.transform.position);
        }
        else
        {
            steeringForce += FollowField(script, transform.position) * 4;
        }

        return Vector3.ClampMagnitude(steeringForce, maxForce);
    }
}
