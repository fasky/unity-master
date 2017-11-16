using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
//Kyle Fasanella
//Class for changing scene at start of game

public class sceneChanger : MonoBehaviour {

	void start()
	{

	}
    // Update is called once per frame
    void Update()
    {
        //if left click - load main game
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene(1);
        }

        //if right click, exit app
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Application.Quit();
        }
    }
}

