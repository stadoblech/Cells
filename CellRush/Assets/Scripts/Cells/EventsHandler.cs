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

    [Range(1, 100),Tooltip("probability + (probability * (threat/100))")]
    public int workersReturnProbability = 50;

    [Range(1, 100),Tooltip("probability + (threat/10)")]
    public int failMinningProbability = 5;

    [Range(1,10),Tooltip("v procentech. Pro vypocet pridani threat pri 0 workers a snahu o mining")]
    public int threatPenalty;

    [Tooltip("v procentech. Prida thread pri fail minning")]
    public int threatForFailMinning;

    [Tooltip("v procentech. Pro moznost extra lootu. extraLootPossibility + actualLevel * 2")]
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

    public bool foundTroops
    {
        get;
        set;
    }

    public bool extraLoot
    {
        get;
        set;
    }

    public MineEvent()
    {
        workersReturned = false;
        MonoBehaviour.print(extraLoot);
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

    public bool isExtraLooted()
    {
        int lootPos = extraLootPossibility + (PlayerStats.currentLevel * 2);
        if (lootPos < 10)
        {
            lootPos = 10;
        }
        if (lootPos > 85)
        {
            lootPos = 85;
        }
        if (Random.Range(0, 100) < lootPos)
        {
            extraLoot = true;
            return true;
        }
        else
        {
            extraLoot = false;
            return false;
        }

            
    }

    public int getExtraTroops()
    {
        int extraTr;
        extraTr = PlayerStats.currentLevel * PlayerStats.currentLevel;
        if (extraTr > PlayerStats.numberOfTroops)
        {
            extraTr = PlayerStats.numberOfTroops;
        }
        if (extraTr < 5)
        {
            extraTr = 5;
        }
        return extraTr;
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

    public void calculateCausalities(int numOfTroops)
    {
        int numberOfTroops = numOfTroops;

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
    [Range(0f,1f),Tooltip("koeficient troops. Pocet vyzadovanych jednotek : num of troops * coeficient")]
    public float troopsRequireCoeficient;

    [Tooltip("v procentech. Postih za 0 troops. Snizeni threat")]
    public int notEnoughTroopsPenalty = 5;

    [Tooltip("v procentech. Mnoystvi increase/decrease threat pri lootu")]
    public int increaseOrDecreaseThreat = 2;

    public bool fightCommited
    {
        get;
        set;
    }

    public int lostTroops
    {
        get;
        set;
    }

    public int requireTroops
    {
        get;
        set;
    }

    public bool notEnoughTroops
    {
        get
        {
            if (PlayerStats.numberOfTroops <= 0)
            {
                return true;
            }
            else
                return false;

        }
    }

    public string lootOutput
    {
        get;
        set;
    }

    public ExploreEvent()
    {
        fightCommited = false;
    }

    public int numberOfTroopsRequired()
    {
        float troops = (PlayerStats.numberOfTroops * troopsRequireCoeficient);
        if (troops > 0 && troops < 1)
        {
            troops = 1;
        }
        requireTroops = (int)troops;
        return (int)troops;
    }

    void commitFight()
    {
        int numberOfTroops = numberOfTroopsRequired();
        int lostTroopsRatio = Random.Range(20,50);

        lostTroops = (int)(numberOfTroops - ((numberOfTroops / 100f) * lostTroopsRatio));
        if (lostTroops == 0 && !notEnoughTroops)
        {
            lostTroops = 1;
        }

        PlayerStats.numberOfTroops -= lostTroops; 

    }

    public void explore()
    {
        if (Random.Range(0, 100) < PlayerStats.threat)
        {
            fightCommited = true;
            commitFight();
        }

        getLoot();
    }

    void getLoot()
    {
        if (fightCommited) /// jestli byla bitva
        {
            int rand = Random.Range(0,2);
            
            if (rand == 0) /// jenom jeden loot
            {
                rand = Random.Range(0,2);
                if (rand == 0) /// prida resources
                {
                    lootOutput = "added resources: " + ((PlayerStats.currentLevel * 2) + requireTroops);
                    PlayerStats.numberOfResources += (PlayerStats.currentLevel * 2) + requireTroops;
                }
                else if (rand == 1) /// prida workers
                {
                    lootOutput = "added workers: " + ((PlayerStats.currentLevel * 2) + requireTroops);
                    PlayerStats.numberOfWorkers += (PlayerStats.currentLevel * 2) + requireTroops; 
                }
            }
            else if (rand == 1) // dvakrat loot 
            {
                lootOutput = "added workers and resources: " + ((PlayerStats.currentLevel * 2) + requireTroops / 2);
                PlayerStats.numberOfResources += (PlayerStats.currentLevel * 2) + requireTroops / 2;
                PlayerStats.numberOfWorkers += (PlayerStats.currentLevel * 2) + requireTroops / 2;
            }

            decreaseThreat(increaseOrDecreaseThreat);
        }
        else /// bez bitvy
        {
            int randPick = Random.Range(0,4);
            if (randPick == 3)
            {
                lootOutput = "added resources: " + PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfResources += PlayerStats.currentLevel * randPick;
            }
            else if (randPick == 2)
            {
                lootOutput = "added resources,workers: " + PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfResources += PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfWorkers += PlayerStats.currentLevel * randPick;
            }
            else if (randPick == 1)
            {
                lootOutput = "added resources,workers,troops: " + PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfResources += PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfWorkers += PlayerStats.currentLevel * randPick;
                PlayerStats.numberOfTroops += PlayerStats.currentLevel * randPick;
            }else if(randPick == 0)
            {
                lootOutput = "nothing found...";
            }

            randPick = Random.Range(0,2);
            if (randPick == 0)
            {
                decreaseThreat(increaseOrDecreaseThreat);
            }
            else if (randPick == 1)
            {
                decreaseThreat(increaseOrDecreaseThreat);
            }
        }
    }
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
    public ExploreEvent explore = new ExploreEvent();


    bool topActionTaken;
    bool botActionTaken;

    void Start () {
        topActionTaken = false;
        botActionTaken = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (topActionTaken && !botActionTaken)
        {
            topDescription = printDescription(GetComponent<CellActionCreater>().upOption, topActionTaken);
            botDescription = "";
            return;
        }else if (!topActionTaken && botActionTaken)
        {
            botDescription = printDescription(GetComponent<CellActionCreater>().downOption, botActionTaken);
            topDescription = "";
            return;
        }
        topDescription = printDescription(GetComponent<CellActionCreater>().upOption, topActionTaken);
        botDescription = printDescription(GetComponent<CellActionCreater>().downOption, botActionTaken);
        
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
                    exploreAction();
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
                    return exploreText(actionTaken);
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
            
            if (mine.isExtraLooted())
            {
                PlayerStats.numberOfTroops += mine.getExtraTroops();
            }
        }
    }

    void battleAction()
    {
        if (!fight.notEnoughFighters)
        {
            fight.calculateCausalities(fight.numberOfTroopsRequired());
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

    void exploreAction()
    {
        if (!explore.notEnoughTroops)
        {
            explore.explore();
        }
        else
        {
            explore.addThreat(explore.notEnoughTroopsPenalty);
        }
        
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
        string text = "";
        if (!actionTaken)
        {
            if (PlayerStats.numberOfWorkers > 0)
            {
                text="Mine: je treba " + mine.workersRequire().ToString() + " na vytezeni " + mine.obtainedResources();
            }
            else
            {
                text = "Not enough workers --TODO:punishment--";
            }
        }
        else
        {
            if (PlayerStats.numberOfWorkers >= 0)
            {
                if (mine.workersReturned && mine.getNumberOfReturnedWorkers() > 0)
                {
                    text = "vytezeno, vraceno workers.";
                }
                else if (mine.minningFailed)
                {
                    text = "oups something gone wrong, mined but less.";
                }
                else
                {
                    text = "vytezeno.";
                }
            }
            else
            {
                text = "here im obligated to tell you that you were punished!";
            }

            if (mine.extraLoot)
            {
                text += "Extra troops loot:" + mine.getExtraTroops();
            }
        }

        return text;
    }

    string exploreText(bool actionTaken)
    {
        if (!actionTaken)
        {
            return "send " + explore.numberOfTroopsRequired() + " troops to explore.";
        }
        else
        {
            if (explore.fightCommited)
            {
                return "there was battle. Troops died:" + explore.lostTroops + ". " + explore.lootOutput;
            }
            else
            {
                return explore.lootOutput;
            }
        }
        return "";
    }

    #endregion

}
