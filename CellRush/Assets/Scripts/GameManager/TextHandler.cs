﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour {

    GameObject activeCell;
    public Text topText;
    public Text bottomText;
    public Text statsText;

    private int numOfWorkers;
    private string numOfTroops;
    private string currentLevel;
    private string threat;
    private string numOfresources;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (PlayerStats.numberOfWorkers > 0)
        {
            numOfWorkers = PlayerStats.numberOfWorkers;
        }
        else
        {
            numOfWorkers = 0;
        }
        activeCell = Cells.getActiveCell();

        if (activeCell != null)
        {
            //topText.text = activeCell.GetComponent<EventsHandler>().printDescription(activeCell.GetComponent<CellActionCreater>().upOption);
            //bottomText.text = activeCell.GetComponent<EventsHandler>().printDescription(activeCell.GetComponent<CellActionCreater>().downOption);
            topText.text = activeCell.GetComponent<EventsHandler>().topDescription;
            bottomText.text = activeCell.GetComponent<EventsHandler>().botDescription;
        }
        else
        {
            topText.text = "";
            bottomText.text = "";
        }

        statsText.text = "Current level " + PlayerStats.currentLevel +
            ";Workers " + numOfWorkers +
            ";Troops " + PlayerStats.numberOfTroops +
            ";Threat " + PlayerStats.threat +
            ";Resources " + PlayerStats.numberOfResources;

        //print(activeCell.GetComponent<CellHandler>().activeCell);
	}
}
