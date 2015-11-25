using UnityEngine;
using System.Collections;

public class CellHandler : MonoBehaviour {


    public int cellID;
    public bool activeCell;
    public bool usedCell;

	void Start () {
        activeCell = false;
        usedCell = false;
    }
	
    void Update () {
	
	}

    public GameObject getActiveCell()
    {

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (o != null)
            {
                if (o.GetComponent<CellHandler>().activeCell)
                {
                    return o;
                }
            }
        }

        return null;
    }
}
