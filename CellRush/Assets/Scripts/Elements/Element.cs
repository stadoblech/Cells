using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// pick random element spawn position
    /// </summary>
    /// <param name="pos">"top" or "bot"</param>
    /// <returns></returns>
    public static Vector3 startingElementPosition(string pos)
    {
        Vector3 position = new Vector3();
        if (pos == "bot")
        {
            position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1f, 1.5f), Random.Range(0.1f, 0.4f)));
            position.z = 0;
        }
        else if (pos == "top")
        {
            position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1f, 1.5f), Random.Range(0.9f, 0.6f)));
            position.z = 0;
        }
        return position;
    }
}
