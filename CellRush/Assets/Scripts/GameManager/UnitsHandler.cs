using UnityEngine;
using System.Collections;

public class UnitsHandler : MonoBehaviour {

    public GameObject worker;

    bool firstRun = true;

    Vector3 spawnPosition;
    GameObject player;

    int currentNumberOfworkers;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        
        spawnPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (firstRun)
        {
            currentNumberOfworkers = PlayerStats.numberOfWorkers;
            for (int o = 0; o < PlayerStats.numberOfWorkers; o++)
            {
                Instantiate(worker, spawnPosition, Quaternion.identity);
            }
            firstRun = false;
        }

        if (currentNumberOfworkers != PlayerStats.numberOfWorkers)
        {
            spawnWorkers(currentNumberOfworkers - PlayerStats.numberOfWorkers);
        }
	}


    void spawnWorkers(int numOfWorkers)
    {
        if (numOfWorkers < 0)
        {
            for (int o = 0; o < -numOfWorkers; o++)
            {
                Instantiate(worker, spawnPosition, Quaternion.identity);
            }
            currentNumberOfworkers = PlayerStats.numberOfWorkers;
        }
        else
        {
            int counter = 0;
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Worker"))
            {
                if (o.GetComponent<UnitLivingHandler>().living)
                {
                    o.GetComponent<UnitLivingHandler>().living = false;
                    o.GetComponent<MoveAroundObject>().setAwayDestination();
                    counter++;
                    if (counter == numOfWorkers)
                    {
                        currentNumberOfworkers = PlayerStats.numberOfWorkers;
                        return;
                    }
                }
                
            }
        }
        
    }

}
