using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;
    public float timeToAction;
    public KeyCode nextCellKey = KeyCode.RightArrow;

    float timeToActionCooldown;
    Vector3 positionToMove;
    bool firtsMove;

    public bool finishedMoving
    {
        get;
        set;
    }

	void Start () {
        if (timeToAction == 0)
        {
            timeToAction = 999;
        }

        finishedMoving = true;
        timeToActionCooldown = timeToAction;
        firtsMove = true;
	}
	
	// Update is called once per frame
	void Update () {
 
        if (Input.GetKeyDown(nextCellKey) && firtsMove)
        {
            positionToMove = Cells.getActiveCell().transform.position;
            finishedMoving = false;
            firtsMove = false;
        }

        if (finishedMoving && !firtsMove && GetComponent<PlayerAction>().actionMade)
        {
            if (Input.GetKeyDown(nextCellKey))
            {
                positionToMove = Cells.getActiveCell().transform.position;
                finishedMoving = false;
                timeToActionCooldown = timeToAction;
            }
            
            if (timeToActionCooldown < 0)
            {
                print("burn");
                timeToActionCooldown = 0;
            }
            else if(timeToActionCooldown > 0)
            {
                timeToActionCooldown -= Time.deltaTime;
            }
        }
        else if (GetComponent<PlayerAction>().actionMade)
        {
            transform.position = Vector2.MoveTowards(transform.position, positionToMove, moveSpeed * Time.deltaTime);

            if (transform.position == positionToMove)
            {
                finishedMoving = true;
                if (!firtsMove)
                {
                    GetComponent<PlayerAction>().actionMade = false;
                }
            }
        }
	}   
}
