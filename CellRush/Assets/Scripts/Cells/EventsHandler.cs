using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventsHandler : MonoBehaviour {

    [System.Serializable]
    public class MineEvent
    {
        public float workersCoeficient = 0.3f;
        public int returnWorkersProbabilityCoeficient = 50;

        public bool workersReturned = false;

        /// <summary>
        /// pocet workeru kteri se dectou od celkoveho poctu workeru
        /// </summary>
        /// <returns></returns>
        public int workersRequire()
        {
            return (int)(PlayerStats.numberOfWorkers * workersCoeficient + PlayerStats.currentLevel);
        }

        /// <summary>
        /// kontrola zdali se nejaky pocet workeru vrati
        /// </summary>
        /// <returns></returns>
        public bool isReturningWorkers()
        {
            int returnWorkersPercentage = (int)(returnWorkersProbabilityCoeficient + (returnWorkersProbabilityCoeficient * PlayerStats.threat/100));
            //print(returnWorkersPercentage);
            if (Random.Range(0, 100) < returnWorkersPercentage)
            {
                workersReturned = true;
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
            return (int)(((PlayerStats.numberOfWorkers * workersCoeficient + PlayerStats.currentLevel) * 10) - (PlayerStats.threat / 3));
        }
    }

    public MineEvent mine = new MineEvent();


    bool actionTaken;

    void Start () {
        actionTaken = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void takeAction(ActionType a)
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
                    //print(mine.isReturningWorkers());
                    PlayerStats.numberOfResources += mine.obtainResources();
                    PlayerStats.numberOfWorkers -= mine.workersRequire();
                    if (mine.isReturningWorkers())
                    {
                        PlayerStats.numberOfWorkers += mine.getNumberOfReturnedWorkers();
                    }

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
                    if (!actionTaken)
                        return "Mine: je treba " + mine.workersRequire().ToString() + " na vytezeni " + mine.obtainResources();
                    else
                    {
                        if (mine.workersReturned)
                        {
                            return "vytezeno, vraceno workers";
                        }
                        else
                        {
                            return "vytezeno";
                        }
                    }
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

}
