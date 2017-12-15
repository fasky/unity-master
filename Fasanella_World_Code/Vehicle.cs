using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//abstract class for movers to derive from

public abstract class Vehicle : MonoBehaviour
{

    //go and amts
    public Manager manager;
    public GameObject closest;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public GameObject avoid;
    public List<GameObject> tempList;
    public List<GameObject> closeList;
    public List<Vector3> closeSteerList;
    public GameObject center;
    public Terrain myTerrain;

    //vectors
    public Vector3 vehiclePosition;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 targetDiff;
    public Vector3 steeringForce;
    public Vector3 wanderSpot;
    public Vector3 steerForce;

    //for obstacle avoiding
    public Vector3 vectorToCenter;
    public float dotVer;
    public float dotProdHor;
    public float dotProdVer;

    //floats
    public float range;
    public float wanderRange;
    public float mag;
    public float radius;
    public float maxSpeed;
    public float maxForce;
    public float angle;
    public float closestDist;
    public float timePassed;
    public float sepRange;
    public float drawRad;

    //weights
    public float sepWeight;
    public float avoidWeight;
    public float cohWeight;
    public float aliWeight;
    public float wanWeight;
    public float seekWeight;

    public bool wander;
    public bool lines;
    public bool outside;

    //steering methods
    //calc all forces
    public abstract Vector3 CalcSteeringForces();

    //seek method
    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVel = targetPos - vehiclePosition;
        desiredVel = desiredVel.normalized * maxSpeed;
        return (desiredVel - velocity);
    }

    //flee method
    public Vector3 Flee(Vector3 fleeVel)
    {
        Vector3 desiredVel = vehiclePosition - fleeVel;
        desiredVel = desiredVel.normalized * maxSpeed;
        return (desiredVel - velocity);
    }

    //follow flow field
    public Vector3 FollowField(FlowField script, Vector3 position)
    {
        Vector3 desiredVel = script.GetFlowDirection(position);
        desiredVel = desiredVel.normalized * maxSpeed;
        return (desiredVel - velocity);
    }

    //wander
    public Vector3 Wander()
    {
        if (timePassed > 30)
        {
            wanderSpot = transform.position + new Vector3(velocity.normalized.x * 5, 0, velocity.normalized.z * 5);
            wanderSpot += new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));
            timePassed = 0;
        }
        Vector3 desiredVel = wanderSpot - vehiclePosition;
        desiredVel = desiredVel.normalized * maxSpeed;
        return (desiredVel - velocity);
    }

    //try to keep friendlies from overlapping
    public Vector3 Seperation()
    {
        closeList = new List<GameObject>();
        closeSteerList = new List<Vector3>();
        closestDist = 100;
        steerForce = Vector3.zero;    

        //find too close neighbors
        foreach (GameObject neighbor in tempList)
        {
            targetDiff = neighbor.transform.position - transform.position;
            mag = targetDiff.magnitude;
            if (mag < closestDist && mag < sepRange && mag > 0)
            {
                closestDist = mag;
                closeList.Add(neighbor);
            }
        }

        //calculate steering vector away from each neighbor
        if (closeList.Count > 0)
        {
            foreach (GameObject close in closeList)
            {
                vectorToCenter = close.transform.position - transform.position;
                dotProdHor = Vector3.Dot(vectorToCenter, ((Quaternion.Euler(0, 90, 0) * new Vector3(velocity.x, 0, velocity.z))).normalized);
                dotProdVer = Vector3.Dot(vectorToCenter, (new Vector3(velocity.x, 0, velocity.z)).normalized);

                if (dotProdVer > 0 && dotProdVer < dotVer)
                {
                    if (dotProdHor < (radius + (close.GetComponent<BoxCollider>().size.x / 2)) && (dotProdHor > -(radius + (close.GetComponent<BoxCollider>().size.x / 2))))
                    {
                        if (dotProdHor < 0)
                        {
                            closeSteerList.Add((Quaternion.Euler(0, 90, 0) * new Vector3(velocity.x, 0, velocity.z)) * (1 / vectorToCenter.magnitude)); //weighted
                        }
                        else if (dotProdHor >= 0)
                        {
                            closeSteerList.Add((Quaternion.Euler(0, -90, 0) * new Vector3(velocity.x, 0, velocity.z)) * (1 / vectorToCenter.magnitude));
                        }
                    }
                }
            }

            foreach (Vector3 steerPart in closeSteerList)
            {
                steerForce += steerPart;
            }
        }

        steerForce = steerForce.normalized * maxSpeed;

        return steerForce;
    }

    //avoid obstacles
    public Vector3 ObstacleAvoidance()
    {
        foreach (GameObject obstacle in manager.obstacles)
        {
            vectorToCenter = obstacle.transform.position - transform.position;
            dotProdHor = Vector3.Dot(vectorToCenter, ((Quaternion.Euler(0, 90, 0) * new Vector3(velocity.x, 0, velocity.z))).normalized);
            dotProdVer = Vector3.Dot(vectorToCenter, (new Vector3(velocity.x, 0, velocity.z)).normalized);

            if (dotProdVer > 0 && dotProdVer < dotVer)
            {
                if (dotProdHor < (radius + (obstacle.GetComponent<CapsuleCollider>().radius)) && (dotProdHor > -(radius + (obstacle.GetComponent<CapsuleCollider>().radius))))
                {
                    if (dotProdHor < 0)
                    {
                        return ((Quaternion.Euler(0, 90, 0) * new Vector3(velocity.x, 0, velocity.z)));
                    }
                    else if (dotProdHor >= 0)
                    {
                        return ((Quaternion.Euler(0, -90, 0) * new Vector3(velocity.x, 0, velocity.z)));
                    }
                }
            }
        }

        return Vector3.zero;
    }

    //cohesion
    public Vector3 Cohesion()
    {
        steerForce = Vector3.zero;
        steerForce = Seek(manager.flockPos);
        return steerForce;
    }

    //alignment
    public Vector3 Alignment()
    {
        steerForce = Vector3.zero;
        steerForce = manager.flockVelocity.normalized;
        return steerForce;
    }

    //other methods
    //bounce off edge of plane
    public void Bounce()
    {
        if (vehiclePosition.x < 0 || vehiclePosition.x > manager.spawnRange || vehiclePosition.z < 0 || vehiclePosition.z > manager.spawnRange)
        {
            outside = true;
        }
        else
        {
            outside = false;
        }
    }

    //rotate to face velocity direction
    public void Rotate()
    {
        angle = Mathf.Atan2(velocity.x, velocity.z);
        angle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

}

