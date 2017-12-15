using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//class for flocking agents

public class Flocker : Vehicle {

    // Use this for initialization
    void Start () {
        vehiclePosition = transform.position;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        radius = GetComponent<BoxCollider>().size.x;
        tempList = new List<GameObject>();
        timePassed = 30;
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        center = GameObject.Find("Bounder");
    }
	
	// Update is called once per frame
	void Update () {

        //keep it on the terrain
        transform.position = new Vector3(transform.position.x,myTerrain.SampleHeight(transform.position),transform.position.z);

        //for seperation
        tempList = manager.flockers;

        timePassed++;

        //grab transoform pos
        vehiclePosition = transform.position;
        Bounce();

        wander = true;

        acceleration = CalcSteeringForces();

        //add acceleration to velocity
        velocity += acceleration * Time.deltaTime;
        
        //check if in water area, resistance applied if so
        if (transform.position.y < 30 && transform.position.x > 90 && transform.position.x < 132 && transform.position.z > 77 && transform.position.z < 118)
        {
            velocity /= 1.05f;
        }

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
        //reset acceleration
        acceleration = Vector3.zero;
    }

    //calculate steering forces to move object
    // Seek the nut while staying as a flock
    public override Vector3 CalcSteeringForces()
    {
        steeringForce = Vector3.zero;

        if (outside)
        {
            steeringForce += Seek(center.transform.position);
        }

        else
        {
            steeringForce += Seek(GameObject.FindGameObjectWithTag("Finish").transform.position) * seekWeight;
            steeringForce += (ObstacleAvoidance() * avoidWeight);
            steeringForce += Seperation() * sepWeight;
            mag = (transform.position - manager.flockPos).magnitude;
            if(mag > 5)
            {
                steeringForce += Cohesion() * cohWeight;
            }
            steeringForce += Alignment() * aliWeight;
        }

        return Vector3.ClampMagnitude(steeringForce, maxForce);
    }
}
