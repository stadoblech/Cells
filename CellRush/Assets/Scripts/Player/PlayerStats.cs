using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{

    public int xpForNextLevel;
    public float startSize;
    public int startNumberOfTroops;
    public int startNumberOfWorkers;
    public int startNumberOfResources;
    public int startThreatAmount;

    public static float size;
    public static int numberOfTroops;
    public static int numberOfWorkers;
    public static int numberOfResources;
    public static int threat;
    public static int currentLevel;
    

    public static int experience;

    void Start()
    {
        size = startSize;
        numberOfTroops = startNumberOfTroops;
        numberOfWorkers = startNumberOfWorkers;
        numberOfResources = startNumberOfResources;
        threat = startThreatAmount;
        currentLevel = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (experience >= xpForNextLevel)
        {
            experience = 0;
            currentLevel++;
        }
    }
}
