using UnityEngine;
using System.Collections;

public class UnitsHandler : MonoBehaviour {

    public GameObject worker;
    public GameObject troop;

    bool firstRun = true;

    Vector3 spawnPosition;
    GameObject player;

    int currentNumberOfworkers;
    int currentNumberOfTroops;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        
        spawnPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (firstRun)
        {
            currentNumberOfworkers = PlayerStats.numberOfWorkers;
            currentNumberOfTroops = PlayerStats.numberOfTroops;

            for (int o = 0; o < PlayerStats.numberOfWorkers; o++)
            {
                Instantiate(worker, spawnPosition, Quaternion.identity); 
            }
            
            for (int o = 0; o < PlayerStats.numberOfTroops; o++)
            {
                Instantiate(troop, spawnPosition, Quaternion.identity);
            }

            firstRun = false;
        }

        if (currentNumberOfTroops != PlayerStats.numberOfTroops)
        {
            spawnTroops(currentNumberOfTroops - PlayerStats.numberOfTroops);
        }

        if (currentNumberOfworkers != PlayerStats.numberOfWorkers)
        {
            spawnWorkers(currentNumberOfworkers - PlayerStats.numberOfWorkers);
        }
	}


    void spawnWorkers(int numOfWorkers)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        
        if (numOfWorkers < 0)
        {
            for (int o = 0; o < -numOfWorkers; o++)
            {
                spawnPosition = cameraPos +
                    new Vector3(Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize) * 2,
                    Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize) * 2);
                Instantiate(worker, spawnPosition, Quaternion.identity);
            }
            currentNumberOfworkers = PlayerStats.numberOfWorkers;
        }
        else
        {
            int counter = 0;
            if (GameObject.FindGameObjectsWithTag("Worker").Length <= 0)
            {
                return;
            }
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

    void spawnTroops(int numOfTroops)
    {
        Vector3 cameraPos = Camera.main.transform.position;

        if (numOfTroops < 0)
        {
            for (int o = 0; o < -numOfTroops; o++)
            {
                spawnPosition = cameraPos +
                    new Vector3(Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize) * 2,
                    Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize) * 2);
                Instantiate(troop, spawnPosition, Quaternion.identity);
            }
            currentNumberOfTroops = PlayerStats.numberOfTroops;
        }
        else
        {
            int counter = 0;
            if (GameObject.FindGameObjectsWithTag("Troop").Length <= 0)
            {
                return;
            }
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Troop"))
            {
                if (o.GetComponent<UnitLivingHandler>().living)
                {
                    o.GetComponent<UnitLivingHandler>().living = false;
                    o.GetComponent<MoveAroundObject>().setAwayDestination();
                    counter++;
                    if (counter == numOfTroops)
                    {
                        currentNumberOfTroops = PlayerStats.numberOfTroops;
                        return;
                    }
                }

            }
        }
    }

}
