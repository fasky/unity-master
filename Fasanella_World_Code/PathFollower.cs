using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//for path following agents

public class PathFollower : Vehicle {

    public int currentMarker;
    public GameObject[] markers;

    // Use this for initialization
    void Start () {
        vehiclePosition = transform.position;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        tempList = new List<GameObject>();
        timePassed = 30;
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        center = GameObject.Find("Bounder");
        currentMarker = 0;
    }
	
	// Update is called once per frame
    //Check if it's reached a marker yet, and if so seek the next one
    // Apply resistance if in water area.
	void Update () {

        // get waypoints
        markers = manager.waypoints;

        //keep feet on terrain
        transform.position = new Vector3(transform.position.x, myTerrain.SampleHeight(transform.position), transform.position.z);

        //check current marker distance
        if((markers[currentMarker].transform.position - transform.position).magnitude < 5)
        {
            if(currentMarker != 13)
            {
                currentMarker++;
            }
            else
            {
                currentMarker = 0;
            }
        }

        //for seperation
        tempList = manager.pathers;

        timePassed++;

        //grab transoform pos
        vehiclePosition = transform.position;
        Bounce();

        acceleration = CalcSteeringForces();

        //add acceleration to velocity
        velocity += acceleration * Time.deltaTime;

        //check if in water area, resistance force applied if so
        if (transform.position.y < 30 && transform.position.x > 90 && transform.position.x < 132 && transform.position.z > 77 && transform.position.z < 118)
        {
            velocity /= 1.013f;
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

        //rotate to face direction it's moving
        Rotate();
        //reset acceleration
        acceleration = Vector3.zero;
    }

    //calc steering
    //keep them seperate, have them seek the markers
    public override Vector3 CalcSteeringForces()
    {
        steeringForce = Vector3.zero;
        steeringForce += Seek(markers[currentMarker].transform.position) * seekWeight;
        steeringForce += Seperation() * sepWeight;
        return Vector3.ClampMagnitude(steeringForce, maxForce);
    }
}
