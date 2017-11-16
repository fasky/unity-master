using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//Kyle Fasanella
//Class for handling score

public class Manager : MonoBehaviour {

    public int score;

    // Use this for initialization
    void Start () {
        //starting score
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //escape to exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
