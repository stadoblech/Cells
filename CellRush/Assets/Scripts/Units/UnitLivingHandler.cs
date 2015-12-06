using UnityEngine;
using System.Collections;

public class UnitLivingHandler : MonoBehaviour {

    public bool living;

	void Start () {
        living = true;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (PlayerStats.gameOver)
        {
            GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
         

        if (!living && !GetComponent<SpriteRenderer>().isVisible)
        {
            destroyWorker();
        }
	}

    public void destroyWorker()
    {
        Destroy(gameObject);
    }
}
