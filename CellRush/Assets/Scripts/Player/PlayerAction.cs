using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour {

    GameObject activeCell;
    public KeyCode upAction = KeyCode.UpArrow;
    public KeyCode downAction = KeyCode.DownArrow;
    public KeyCode skipAction = KeyCode.LeftArrow;

    public bool actionMade
    {
        get;
        set;
    }

	void Start () {
        actionMade = true;
	}
	
	// Update is called once per frame
	void Update () {


        activeCell = Cells.getActiveCell();

        if (!actionMade)
        {
            if (Input.GetKey(downAction))
            {
                actionMade = true;
                takeAction(activeCell.GetComponent<CellActionCreater>().downOption);
            }
            else if (Input.GetKey(upAction))
            {
                actionMade = true;
                takeAction(activeCell.GetComponent<CellActionCreater>().upOption);
            }
            else if (Input.GetKey(skipAction))
            {
                actionMade = true;
            }
        }
	}

    void takeAction(ActionType c)
    {
        switch (c)
        {
            case ActionType.Battle:
                {
                    break;
                }
            case ActionType.Explore:
                {
                    break;
                }
            case ActionType.Mine:
                {
                    activeCell.GetComponent<EventsHandler>().takeAction(c);
                    break;
                }
        }
    }
}
