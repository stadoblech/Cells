using UnityEngine;
using System.Collections;

public class UnitLivingHandler : MonoBehaviour {

    public bool living;

	void Start () {
        living = true;
	}
	
	// Update is called once per frame
	void Update () {
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
