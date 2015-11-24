using UnityEngine;
using System;
using System.Collections;

public enum ActionType
{
    Battle,
    Explore,
    Mine,
    Build
}

public class CellActionCreater : MonoBehaviour {

    public ActionType upOption;
    public ActionType downOption;

    int numberOfActions;

	void Start () {
        numberOfActions = Enum.GetValues(typeof(ActionType)).Length;

        while((upOption == downOption))
        {
            upOption = (ActionType)UnityEngine.Random.Range(0, numberOfActions);
            downOption = (ActionType)UnityEngine.Random.Range(0, numberOfActions);

            while (upOption == ActionType.Mine && PlayerStats.numberOfWorkers <= 0)
            {
                upOption = (ActionType)UnityEngine.Random.Range(0, numberOfActions);
            }

            while (downOption == ActionType.Mine && PlayerStats.numberOfWorkers <= 0)
            {
                downOption = (ActionType)UnityEngine.Random.Range(0, numberOfActions);
            } 
        }

        //print(upOption + " " + downOption);

	}
	
	void Update () {
        //activeCell = Cells.getActiveCell();
	}
}
