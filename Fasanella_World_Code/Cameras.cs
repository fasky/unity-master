using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kyle Fasanella
//for handling cameras switching, and the GUI

public class Cameras : MonoBehaviour {

    public Camera[] cameras;
    private int currentCamIndex;

	// Use this for initialization
	void Start () {
        //start in fps
        currentCamIndex = 1;

        for(int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        if(cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //if c is pressed, cycle through the camera index, check if valid index, set active camera accordingly and adjust cam index if needed.
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCamIndex++;
        }

        if(currentCamIndex < cameras.Length)
        {
            if(currentCamIndex > 0)
            {
                cameras[currentCamIndex - 1].gameObject.SetActive(false);
            }
            cameras[currentCamIndex].gameObject.SetActive(true);
        }

        else
        {
            cameras[currentCamIndex - 1].gameObject.SetActive(false);
            currentCamIndex = 0;
            cameras[currentCamIndex].gameObject.SetActive(true);
        }
	}

    //draw the gui, give info based on camera in use
    void OnGUI()
    {
        string current;
        switch (currentCamIndex) {
            case 0: current = "Overhead";
                break;
            case 1: current = "Overview";
                break;
            case 2:
                current = "Pond";
                break;
            case 3:
                current = "Back Path";
                break;
            case 4:
                current = "Front Path";
                break;
            case 5:
                current = "Forest";
                break;
            case 6: current = "Sky";
                break;
            case 7:
                current = "Pond Side";
                break;
            case 8:
                current = "Forest 2";
                break;
            case 9:
                current = "Bird Cam";
                break;
            case 10:
                current = "Pond Overview";
                break;
            case 11:
                current = "Path Cam";
                break;
            case 12:
                current = "Looking Up";
                break;
            case 13:
                current = "FPS Cam";
                break;
            default: current = "Nothing";
                break;
        }

        GUI.Box(new Rect(10, 10, 215, 55), "Press 'c' to change camera\nCurrent: " + current + "\nPress 'e' to show debugs and path");

    }
}
