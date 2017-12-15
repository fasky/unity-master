using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Kyle Fasanella
//for managing the scene - spawning, keeping in a list

public class Manager : MonoBehaviour {

    public GameObject flockerPre;
    public GameObject obstacle;
    public List<GameObject> flockers;
    public GameObject[] obstacles;
    public GameObject[] waypoints;
    public List<GameObject> debugsStuff;
    public List<GameObject> pathers;
    public float spawnRange;
    public float flockersNum;
    public float obstacleNum;
    public Vector3 flockVelocity;
    public Vector3 flockPos;
    public Material mat2;
    public Terrain terrain;
    public bool lines;
    public GameObject nut;
    public GameObject nutInst;
    public int countdownNut;

    // Use this for initialization
    // instatiate needed valuers, instantiate the flockers and nut for them to go after
    // Flockers will seek the nut
    void Start () {

        //create humans, zombies, obstacles
        flockers = new List<GameObject>();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        waypoints = GameObject.FindGameObjectsWithTag("WP").OrderBy(g=>g.transform.GetSiblingIndex()).ToArray();
        debugsStuff = GameObject.FindGameObjectsWithTag("Debug").ToList();
        nutInst = Instantiate(nut, new Vector3(Random.Range(0, spawnRange), 0f, Random.Range(0, spawnRange)), Quaternion.identity);
        nutInst.transform.position = new Vector3(nutInst.transform.position.x, terrain.SampleHeight(nutInst.transform.position), nutInst.transform.position.z);
        countdownNut = 600;

        lines = false; //for debug stuff

        for (int i = 0; i < flockersNum; i++)
        {
            GameObject guy = Instantiate(flockerPre, new Vector3(Random.Range(0, 100), 0.5f, Random.Range(0, 100)), Quaternion.identity);
            flockers.Add(guy);
        }

    }
	

	// Update is called once per frame
	// Move the nut after time has passed or a flocker gets to it
    // Check for key input to toggle debug stuff
    // flock velocity and position calculated
    void Update () {
        countdownNut--;
        if(countdownNut < 0)
        {
            countdownNut = 600;
            nutInst.transform.position = new Vector3(Random.Range(0, spawnRange), 0f, Random.Range(0, spawnRange));
            nutInst.transform.position = new Vector3(nutInst.transform.position.x, terrain.SampleHeight(nutInst.transform.position), nutInst.transform.position.z);
        }
        foreach(GameObject flocky in flockers)
        {
            if(flocky.transform.position == nutInst.transform.position)
            {
                countdownNut = 600;
                nutInst.transform.position = new Vector3(Random.Range(0, spawnRange), 0f, Random.Range(0, spawnRange));
                nutInst.transform.position = new Vector3(nutInst.transform.position.x, terrain.SampleHeight(nutInst.transform.position), nutInst.transform.position.z);
            }
        }


        //press e to toggle debug/path lines stuff
        if (Input.GetKeyDown(KeyCode.E))
        {
            lines = !lines;
        }

        if (lines == false)
        {
            foreach (GameObject e in debugsStuff)
            {
                e.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject e in debugsStuff)
            {
                e.SetActive(true);
            }
        }

        flockVelocity = Vector3.zero;
        flockPos = Vector3.zero;

		foreach(GameObject flockee in flockers)
        {
            flockVelocity += flockee.GetComponent<Flocker>().velocity;
            flockPos += flockee.GetComponent<Flocker>().vehiclePosition;
        }

        flockPos = (flockPos / flockers.Count);
	}

}
