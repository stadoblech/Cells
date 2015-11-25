using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

    public GameObject cell;
    public int numberOfCells;

    Vector3 actualCellPosition;
    
    GameObject player;
    Vector3 playerSize;

    int lastCellID = 0;
    int activeCellID = -1;

	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        playerSize = player.GetComponent<SpriteRenderer>().bounds.size;

        actualCellPosition = player.transform.position + new Vector3(playerSize.x, 0, 0);

        for (int i = 0; i < numberOfCells+1; i++)
        {
            Instantiate(cell, actualCellPosition, Quaternion.identity);
            actualCellPosition += new Vector3(playerSize.x, 0, 0);
        }

        
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            o.GetComponent<CellHandler>().cellID = lastCellID;
            lastCellID++;
        }

        setActiveCell();
    }


	void Update () {
        if (Input.GetKeyDown(player.GetComponent<PlayerMovement>().nextCellKey) 
            && player.GetComponent<PlayerMovement>().finishedMoving 
            && player.GetComponent<PlayerAction>().actionMade)
        {
            GameObject newCell = cell;
            cell.GetComponent<CellHandler>().cellID = lastCellID;
            Instantiate(newCell, actualCellPosition, Quaternion.identity);
            actualCellPosition += new Vector3(playerSize.x, 0, 0);
            lastCellID++;
            activeCellID++;
        }

        setActiveCell();
        usedCellHandling();   
    }

    void setActiveCell()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (activeCellID-1 == o.GetComponent<CellHandler>().cellID)
            {
                o.GetComponent<CellHandler>().activeCell = false;
            }
            if (activeCellID == o.GetComponent<CellHandler>().cellID)
            {
                o.GetComponent<CellHandler>().activeCell = true;
            }
        }
    }

    void usedCellHandling()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (o.GetComponent<CellHandler>().cellID < activeCellID)
            {
                o.GetComponent<CellHandler>().usedCell = true;
            }

            if (o.GetComponent<CellHandler>().usedCell && !o.GetComponent<SpriteRenderer>().isVisible)
            {
                Destroy(o);
            }
        }
    }
}
