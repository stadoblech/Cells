using UnityEngine;
using System.Collections;

public class ElementMovement : MonoBehaviour {

    public float minSpeed;
    public float maxSpeed;

    private float speed;

    /// <summary>
    /// get set not working while instantiate
    /// </summary>
    public string position;

    public bool moving = true;

    Vector3 startPosition;
    bool entered = false;

    void Start () {
        startPosition = transform.position;
        speed = getRandomElementSpeed();
	}
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            transform.position -= new Vector3(speed, 0);
        }

        if (GetComponent<SpriteRenderer>().isVisible && !entered)
        {
            entered = true;
        }

        if (!GetComponent<SpriteRenderer>().isVisible && entered)
        {
            if (position == "top")
            {
                transform.position = Element.startingElementPosition("top");
            }
            else if (position == "bot")
            {
                transform.position = Element.startingElementPosition("bot");
            }
            speed = getRandomElementSpeed();
            entered = false;
        }
	}

    private float getRandomElementSpeed()
    {
        return Random.Range(minSpeed,maxSpeed);
    }
}
