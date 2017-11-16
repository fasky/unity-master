using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Kyle Fasanella
//Class for holding/referencing all asteroids, and objects that can be collided with (enemy, bomb, 2nd level asteroids). Create initial asteroids, spawn enemy

public class Asteroids : MonoBehaviour {

    public Camera cam;
    public GameObject[] prefab;
    public GameObject enemyPrefab;
    public int pop;
    //public GameObject[] asteroids;
    public List<GameObject> asteroids;
    private int countdown; //for enemy spawn
    private int enemySpawned;

    // Use this for initialization
    void Start () {
        enemySpawned = 0; //enemy not spawned
        countdown = 500; //countdown to enemy spawn
        cam = Camera.main;
        asteroids = new List<GameObject>();
        Create();
    }
	
	// Update is called once per frame
	void Update () {
        countdown--; 
        //if no asteroids, and ship exists, display you won and score
        if(asteroids.Count == 0 && GameObject.Find("Ship"))
        {
            GameObject.Find("Text").GetComponent<Text>().text = "YOU WON! Score: " + GameObject.Find("Manager").gameObject.GetComponent<Manager>().score.ToString();
        }
        //if countdown is less than zero, and enemy is not spawned, spawn the enemy in corner
        if(countdown < 0 && enemySpawned == 0)
        {
            enemySpawned = 1;
            asteroids.Add(Instantiate(enemyPrefab, new Vector3(-9.33f, 4.91f, cam.farClipPlane - 1000), Quaternion.identity));
        }
    }

    //on create, make and add asteroids to list (first level asteroids)
    void Create()
    {
        for(int i = 0; i < pop; i++)
        {
            Vector3 objPos = cam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), cam.farClipPlane-990));
            asteroids.Add(Instantiate(prefab[Random.Range(0, prefab.Length)], objPos, Quaternion.identity));
        }
    }

}
