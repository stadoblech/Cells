﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#region event classes 

public class Event
{
    /// <summary>
    /// TODO : implement threat!!
    /// </summary>
    /// <returns></returns>
    public int addThreat()
    {
        return 10;
    }

    public int decreaseThreat()
    {
        return 10;
    }
}

[System.Serializable]
public class MineEvent : Event
{
    [Tooltip("pro vypocet akci spojenych s workery"), Range(0.1f, 1)]
    public float workersCoeficient = 0.3f;

    [Range(1, 100),Tooltip("probability + (probability * (threat/100))")]
    public int returnWorkersProbabilityCoeficient = 50;

    [Range(1, 100),Tooltip("probability + (threat/10)")]
    public int failMinningProbability = 5;

    public int succesfullMineXp = 200;
    public int returnWorkersXp = 50;

    public bool workersReturned
    {
        get;
        set;
    }

    public bool minningFailed
    {
        get;
        set;
    }

    public MineEvent()
    {
        workersReturned = false;
    }

    /// <summary>
    /// pocet workeru kteri se dectou od celkoveho poctu workeru
    /// </summary>
    /// <returns></returns>
    public int workersRequire()
    {
        int reqWorkers = (int)(PlayerStats.numberOfWorkers * workersCoeficient + PlayerStats.currentLevel);
        if((PlayerStats.numberOfWorkers - reqWorkers) < 0)
        {
            return PlayerStats.numberOfWorkers;
        }
        return reqWorkers;
    }

    /// <summary>
    /// kontrola zdali se nejaky pocet workeru vrati
    /// </summary>
    /// <returns></returns>
    public bool isReturningWorkers()
    {
        int returnWorkersPercentage = (int)(returnWorkersProbabilityCoeficient + (returnWorkersProbabilityCoeficient * PlayerStats.threat / 100));
        //print(returnWorkersPercentage);
        if (Random.Range(0, 100) < returnWorkersPercentage && !minningFailed)
        {
            workersReturned = true;
            return true;
        }
        return false;
    }

    public bool isMinningFailed()
    {
        minningFailed = false;
        if (Random.Range(0, 100) < failMinningProbability + (PlayerStats.threat / 10))
        {
            minningFailed = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// pocet workeru kteri se vrati
    /// </summary>
    /// <returns></returns>
    public int getNumberOfReturnedWorkers()
    {
        int returnW = (int)(PlayerStats.numberOfWorkers * workersCoeficient);
        return returnW;
    }

    /// <summary>
    /// pocet obdrzenych jednotek
    /// </summary>
    /// <returns></returns>
    public int obtainResources()
    {
        int obtRes = (int)(((PlayerStats.numberOfWorkers * workersCoeficient + PlayerStats.currentLevel) * 10) - (PlayerStats.threat / 3));
        if(minningFailed)
        {
            return obtRes/2;
        }
        return obtRes;
    }
}

[System.Serializable]
public class FightEvent : Event
{
    [Range(0.1f,1)]
    public float fighterCoeficient;

}

#endregion

public class EventsHandler : MonoBehaviour {

    public string topDescription
    {
        get;
        set;
    }

    public string botDescription
    {
        get;
        set;
    }

    public MineEvent mine = new MineEvent();
    public FightEvent fight = new FightEvent();


    bool actionTaken;

    void Start () {
        actionTaken = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        topDescription = printDescription(GetComponent<CellActionCreater>().upOption);
        botDescription = printDescription(GetComponent<CellActionCreater>().downOption);
	}

    public void takeAction(ActionType a,string actionDirection)
    {
        actionTaken = true;
        switch (a)
        {
            case ActionType.Battle:
                {
                    
                    break;
                }
            case ActionType.Mine:
                {
                    mineAction(a,actionDirection);
                    break;
                }
            case ActionType.Explore:
                {
                    break;
                }
            case ActionType.Build:
                {
                    break;
                }
        }
    }

    public string printDescription(ActionType a)
    {
        switch (a)
        {
            case ActionType.Battle:
                {
                    return("battle");

                }
            case ActionType.Mine:
                {
                    return MinningText();
                }
            case ActionType.Explore:
                {
                    return("Explore");
                }
            case ActionType.Build:
                {
                    return("Build");
                }
        }
        return "";
    }

    #region Event action region
    
    void mineAction(ActionType a,string actionDirection)
    {
        if (actionDirection == "up")
        {
            topDescription = printDescription(a);
        }
        else if (actionDirection == "down")
        {
            botDescription = printDescription(a);
        }

        if (PlayerStats.numberOfWorkers <= 0)
        {
            PlayerStats.numberOfWorkers = -1;
            PlayerStats.threat += mine.addThreat();
        }
        else
        {
            mine.isMinningFailed();
            PlayerStats.numberOfResources += mine.obtainResources();
            PlayerStats.numberOfWorkers -= mine.workersRequire();

            if (mine.isReturningWorkers())
            {
                PlayerStats.numberOfWorkers += mine.getNumberOfReturnedWorkers();
                PlayerStats.experience += mine.returnWorkersXp;
            }
        }

        PlayerStats.experience += mine.succesfullMineXp;
    }

    #endregion

    #region Event text region

    private string MinningText()
    {
        if (!actionTaken)
        {
            if (PlayerStats.numberOfWorkers > 0)
            {
                return "Mine: je treba " + mine.workersRequire().ToString() + " na vytezeni " + mine.obtainResources();
            }
            else
            {
                return "Not enough workers --TODO:punishment--";
            }
        }
        else
        {
            if (PlayerStats.numberOfWorkers >= 0)
            {
                if (mine.workersReturned && mine.getNumberOfReturnedWorkers() > 0)
                {
                    return "vytezeno, vraceno workers";
                }
                else if (mine.minningFailed)
                {
                    return "oups something gone wrong, mined but less";
                }
                else
                {
                    return "vytezeno";
                }
            }
            else
            {
                return "here im obligated to tell you that you were punished!";
            }
        }
    }

    #endregion

}
