using UnityEngine;
using System.Collections;

public class Cells : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static GameObject getActiveCell()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (o.GetComponent<CellHandler>().activeCell)
            {
                return o;
            }
        }
        return null;
    }

    public static int getIdfOfActiveCell()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (o.GetComponent<CellHandler>().activeCell)
            {
                return o.GetComponent<CellHandler>().cellID;
            }
        }
        return 0;

    }
}
