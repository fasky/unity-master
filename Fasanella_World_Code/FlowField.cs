using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//for making and updating the flow field and its debug arrows

[System.Serializable]
public class FlowField : MonoBehaviour {

    Vector3[,] field;
    public int x;
    public int z;
    public int width;
    public int length;
    public int resolution;
    public GameObject arrow;
    public GameObject[,] arrows;
    float zPerl;
    float xPerl;
    float countdown;

    // Use this for initialization
    // uses perlin noise to generate a flow field that goes over the forest
    // also creates arrows for debugging
    void Start () {

        x = width / resolution;
        z = length / resolution;

        field = new Vector3[x,z];
        arrows = new GameObject[x, z];

        zPerl = 0;
        for (int i = 0; i < x; i++)
        {
            xPerl = 0;
            for (int j = 0; j < z; j++)
            {
                //use perlin
                float angle = Mathf.PerlinNoise(xPerl, zPerl) * 5;

                field[i, j] = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                arrows[i, j] = Instantiate(arrow, new Vector3(i * resolution, 40, j * resolution), new Quaternion(Mathf.Sin(angle), 0, Mathf.Cos(angle), 0));
                GameObject.Find("Manager").GetComponent<Manager>().debugsStuff.Add(arrows[i, j]);

                xPerl += .1f;
            }
            zPerl += .3f;
        }
    }
	
	// Update is called once per frame
    // after a set amount of time, update the flow field
	void Update () {

        countdown++;
        if(countdown > 90)
        {
            countdown = 0;
            for (int i = 0; i < x; i++)
            {
                xPerl = 0;
                for (int j = 0; j < z; j++)
                {
                    //use perlin
                    float angle = Mathf.PerlinNoise(xPerl, zPerl) * 5;

                    field[i, j] = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                    arrows[i, j].transform.rotation = new Quaternion(Mathf.Sin(angle), 0, Mathf.Cos(angle), 0);

                    xPerl += .1f;
                }
                zPerl += .3f;
            }
        }


    }


    //get flow direction - used for agent movement
    //find position, closest field value, return the vector
    public Vector3 GetFlowDirection(Vector3 location)
    {
        int i = (int)(Mathf.Clamp(location.x/resolution, 0, x - 1));
        int j = (int)(Mathf.Clamp(location.z/resolution, 0, z - 1));

        return field[i, j];
    }
}
