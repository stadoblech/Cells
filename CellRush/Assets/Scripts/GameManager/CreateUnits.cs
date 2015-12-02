using UnityEngine;
using System.Collections;

public class CreateUnits : MonoBehaviour {

    public GameObject worker;

    bool firstRun = true;
    void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (firstRun)
        {
            for (int o = 0; o < PlayerStats.numberOfWorkers; o++)
            {
                Instantiate(worker,
                    GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(Random.Range(0f, .2f), Random.Range(0f, .2f)), 
                    Quaternion.identity);
            }
            firstRun = false;
        }
	}
}
