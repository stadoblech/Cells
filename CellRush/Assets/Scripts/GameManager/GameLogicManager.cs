using UnityEngine;
using System.Collections;

public class GameLogicManager : MonoBehaviour {

    public bool testMode = true;
    public KeyCode startGame = KeyCode.Space;
    public KeyCode restartGame = KeyCode.R;


    private bool restartedGame;

	void Start () {
        //Time.timeScale = 0;
        restartedGame = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!testMode)
        {
            if (PlayerStats.threat >= 100)
            {
                PlayerStats.gameOver = true;
            }
        }

        if (Input.GetKeyDown(startGame) && PlayerStats.gameOver)
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        if (Input.GetKey(restartGame))
        {
            PlayerStats.gameOver = true;
            restartedGame = true;
            

        }

        if (restartedGame)
        {
            if (Camera.main.orthographicSize <= 0.2)
            {
                //rPlayerStats.gameOver = false;
                Application.LoadLevel(Application.loadedLevel);
            } 
        }
        
	}
}
