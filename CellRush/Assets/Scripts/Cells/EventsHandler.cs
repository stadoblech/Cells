using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#region event classes 

public class Event
{
    public void addThreat(int percentage)
    {
        int thr = (int)(PlayerStats.threat/100f * percentage);
        if (thr == 0)
        {
            thr = 1;
        }
        PlayerStats.threat += thr;
    }

    public void decreaseThreat(int percentage)
    {
        int thr = (int)(PlayerStats.threat / 100f * percentage);
        if (thr == 0)
        {
            thr = 1;
        }
        PlayerStats.threat -= thr;
    }
}

/// <summary>
/// TODO 
/// - extra loot possibility
/// </summary>
[System.Serializable]
public class MineEvent : Event
{
    [Tooltip("pro vypocet akci spojenych s workery"), Range(0.1f, 1)]
    public float workersCoeficient = 0.3f;

    /// <summary>
    /// TODO
    /// </summary>
    public int miningCoeficient;

    [Range(1, 100),Tooltip("probability + (probability * (threat/100))")]
    public int workersReturnProbability = 50;

    [Range(1, 100),Tooltip("probability + (threat/10)")]
    public int failMinningProbability = 5;

    [Range(1,10),Tooltip("v procentech. Pro vypocet pridani threat pri 0 workers a snahu o mining")]
    public int threatPenalty;

    [Tooltip("v procentech. Prida thread pri fail minning")]
    public int threatForFailMinning;

    [Tooltip("v procentech. Pro moznost extra lootu. Zatim bez funcknosti")]
    public int extraLootPossibility;

    public int succesfullMineXp = 200;
    public int unsuccesfullMineXp = 100;
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
        int reqWorkers = (int)(PlayerStats.numberOfWorkers * workersCoeficient);
        if((PlayerStats.numberOfWorkers - reqWorkers) < 0)
        {
            return PlayerStats.numberOfWorkers;
        }

        if (reqWorkers <= 0)
        {
            return 1;
        }
        return reqWorkers;
    }

    /// <summary>
    /// kontrola zdali se nejaky pocet workeru vrati
    /// </summary>
    /// <returns></returns>
    public bool isReturningWorkers()
    {
        int returnWorkersPercentage = (int)(workersReturnProbability - (workersReturnProbability * PlayerStats.threat / 100));
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
        if (returnW == 0)
        {
            return 1;
        }
        return returnW;
    }

    /// <summary>
    /// pocet obdrzenych jednotek
    /// </summary>
    /// <returns></returns>
    public int obtainedResources()
    {
        //int obtRes = (int)(((PlayerStats.numberOfWorkers * workersCoeficient + PlayerStats.currentLevel) * 5) - (PlayerStats.threat / 3));
        int obtRes = (int)((PlayerStats.numberOfWorkers * workersCoeficient*2.5f) - 
            ((PlayerStats.numberOfWorkers * workersCoeficient)/100f * PlayerStats.threat))+PlayerStats.currentLevel;
        
        if(minningFailed)
        {
           obtRes /= 2;
        }
        if (obtRes == 0)
        {
            obtRes = 1;
        }
        return obtRes;
    }
}


/// <summary>
/// TODO 
/// - extra loot possibility (based on surviving troops)
/// </summary>
[System.Serializable]
public class FightEvent : Event
{
    [Range(1,100),Tooltip("v procentech. Pravdepodobnost failu troops. basefailProbability + threat - level")]
    public int baseFailTroopProbability;

    [Tooltip("Zakladni pocet vyzadovanych troops")]
    public int baseTroopRequirement;
    
    [Range(0,100),Tooltip("v procentech. Penalty za fight bez troops")]
    public int notEnoughtTroopsPunishment;

    [Range(0,100),Tooltip("v procentech. Kolik procent threat se snizi za kazdeho usmrceneho troop")]
    public int decreaseThreatPerTroop;

    [Tooltip("pocet xp kdyz je defeated troops vyzsi nez survived troops. Vice xp")]
    public int xpForDefeatedTroops;

    [Tooltip("pocet xp kdyz je defeated troops mensi nez survived troops. Mene xp")]
    public int xpForSurvivingTroops;

    public int xpForDraw
    {
        get
        {
            return (xpForDefeatedTroops + xpForSurvivingTroops) / 2;
        }
    }

    public int survivingTroops
    {
        get;
        set;
    }

    public int defeatetTroops
    {
        get;
        set;
    }

    public bool notEnoughFighters
    {
        get;
        set;
    }

    public int numberOfTroopsRequired()
    {
        float troopsRatio = PlayerStats.threat / 100f;
        float numOfTroops = ((PlayerStats.numberOfTroops * troopsRatio)) + baseTroopRequirement;
        
        if (PlayerStats.numberOfTroops - numOfTroops < 0)
        {
            numOfTroops = PlayerStats.numberOfTroops;
        }

        if (numOfTroops < 1 && numOfTroops > 0)
        {
            numOfTroops = 1;
        }

        if (numOfTroops <= 0)
        {
            notEnoughFighters = true;
        }
        else
        {
            notEnoughFighters = false;
        }

        return (int)numOfTroops;
    }

    public void calculateCausalities()
    {
        int numberOfTroops = numberOfTroopsRequired();

        int defeatOdds = PlayerStats.threat - PlayerStats.currentLevel + baseFailTroopProbability;
        if (defeatOdds > 90)
        {
            defeatOdds = 90;
        }
        else if (defeatOdds < 10)
        {
            defeatOdds = 10;
        }

        for (int i = 0; i < numberOfTroops; i++)
        {
            if (Random.Range(0, 100) > defeatOdds)
            {
                survivingTroops++;
            }
            else
                defeatetTroops++;
        }
        //MonoBehaviour.print("defeated:"+defeatetTroops+" surviving:"+survivingTroops);
    }

}

[System.Serializable]
public class ExploreEvent : Event
{

}

[System.Serializable]
public class BuildEvent : Event
{

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


    bool topActionTaken;
    bool botActionTaken;

    void Start () {
        topActionTaken = false;
        botActionTaken = false;
	}
	
	// Update is called once per frame
	void Update () {
        topDescription = printDescription(GetComponent<CellActionCreater>().upOption,topActionTaken);
        botDescription = printDescription(GetComponent<CellActionCreater>().downOption,botActionTaken);
	}

    public void takeAction(ActionType a,string actionDirection)
    {
        if (actionDirection == "up")
            topActionTaken = true;
        else if (actionDirection == "down")
            botActionTaken = true;

        switch (a)
        {
            case ActionType.Battle:
                {
                    battleAction();
                    break;
                }
            case ActionType.Mine:
                {
                    mineAction();
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

    public string printDescription(ActionType a,bool actionTaken)
    {
        switch (a)
        {
            case ActionType.Battle:
                {
                    return fightText(actionTaken);
                }
            case ActionType.Mine:
                {
                    return MinningText(actionTaken);
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
    
    void mineAction()
    {
        /// Pozor !!! pri poctu workers 0 nebo mene se na UI zobrazuje 0 ale realna hodnota je vzdy -1 !!!!!
        if (PlayerStats.numberOfWorkers <= 0)
        {
            PlayerStats.numberOfWorkers = -1;
            //PlayerStats.threat += mine.addThreat();
            mine.addThreat(mine.threatPenalty);
        }
        else
        {
            if (mine.isMinningFailed())
            {
                PlayerStats.experience += mine.unsuccesfullMineXp;
                mine.addThreat(mine.threatForFailMinning);
            }
            else
            {
                PlayerStats.experience += mine.succesfullMineXp;
            }

            PlayerStats.numberOfResources += mine.obtainedResources();
            PlayerStats.numberOfWorkers -= mine.workersRequire();

            if (mine.isReturningWorkers())
            {
                PlayerStats.numberOfWorkers += mine.getNumberOfReturnedWorkers();
                PlayerStats.experience += mine.returnWorkersXp;
            }
        }
    }

    void battleAction()
    {
        if (!fight.notEnoughFighters)
        {
            fight.calculateCausalities();
            fight.decreaseThreat((fight.defeatetTroops*fight.decreaseThreatPerTroop)+PlayerStats.currentLevel);

            if (fight.defeatetTroops > fight.survivingTroops)
            {
                PlayerStats.experience += fight.xpForDefeatedTroops;
            }
            else if (fight.defeatetTroops < fight.survivingTroops)
            {
                PlayerStats.experience += fight.xpForSurvivingTroops;
            }
            else if (fight.defeatetTroops == fight.survivingTroops)
            {
                PlayerStats.experience += fight.xpForDraw;
            }
        }
        else
        {
            /// TADY JE PUNISHMENT ZA MALO TROOPS 
            fight.addThreat(fight.notEnoughtTroopsPunishment);
        }

        

        PlayerStats.numberOfTroops -= fight.defeatetTroops;
    }

    #endregion

    #region Event text region

    private string fightText(bool actionTaken)
    {
        if (!actionTaken)
        {
            if (fight.numberOfTroopsRequired() >= 1)
            {
                return "battle: je treba " + fight.numberOfTroopsRequired() + " troops.";
            }
            else
            {
                return "Not enough fighters --TODO:punishment--";
            }
        }
        else
        {
            if (!fight.notEnoughFighters)
            {
                return "returned:" + fight.survivingTroops + " died:"+fight.defeatetTroops;
            }
            else
            {
                return "you are punished!";
            }
            return "";
        }
    }

    private string MinningText(bool actionTaken)
    {
        if (!actionTaken)
        {
            if (PlayerStats.numberOfWorkers > 0)
            {
                return "Mine: je treba " + mine.workersRequire().ToString() + " na vytezeni " + mine.obtainedResources();
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
