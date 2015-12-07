using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    public float moveSpeed;
    public KeyCode nextCellKey = KeyCode.RightArrow;

    public static int movesCounter;
    Vector3 positionToMove;
    bool firtsMove;

    [Tooltip("threat za jedno posunuti policka. V procentech. Vypocet: (threat/100) * threatByAction + threatFromLastTurn")]
    public int threatByAction;
    float threatFromLastTurn = 0;

    public bool finishedMoving
    {
        get;
        set;
    }

	void Start () {
        movesCounter = 0;
        finishedMoving = true;
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
                movesCounter++;
                addThreat();

                positionToMove = Cells.getActiveCell().transform.position;
                finishedMoving = false;
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

    void addThreat()
    {
        float F_thr = ((PlayerStats.threat / 100f) * threatByAction) + threatFromLastTurn;
        int I_thr = (int)F_thr;
        threatFromLastTurn = F_thr - I_thr;
        PlayerStats.threat += I_thr;
    }

    
}
