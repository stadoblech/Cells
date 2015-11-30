using UnityEngine;
using System.Collections;

public class GameControllerForTest : MonoBehaviour {


    public KeyCode restartGame = KeyCode.R;
    public KeyCode quitGame = KeyCode.Q;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(restartGame))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKey(quitGame))
        {
            Application.Quit();
        }
	}
}
