using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//Class for getting sprite info
public class SpriteInfo : MonoBehaviour {

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public Vector3 size;
    public Color color;
    public SpriteRenderer spr; //for accessing bounds
    public Vector3 center;
    public float radius;

	// Use this for initialization
	void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        MinMaxEtc();
	}

    //get all info 
    void MinMaxEtc()
    {
        maxX = spr.bounds.max.x;
        maxY = spr.bounds.max.y;
        minX = spr.bounds.min.x;
        minY = spr.bounds.min.y;
        size = spr.bounds.size;
        color = spr.color;
        center = spr.bounds.center;
        radius = spr.bounds.extents.x/2;
    }
}
