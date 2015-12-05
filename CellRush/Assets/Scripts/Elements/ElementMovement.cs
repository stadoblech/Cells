using UnityEngine;
using System.Collections;

public class ElementMovement : MonoBehaviour
{

    public float minElementSpeed = 1f;
    public float maxElementSpeed = 1.5f;

    private float elementSpeed;
    Vector3 pos;

    Vector3 destination;
    void Start()
    {
        elementSpeed = Random.Range(minElementSpeed,maxElementSpeed);
        pos = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f));
        pos.z = 0;

        destination = Vector3.Scale(
            Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f))),
            new Vector3(1,1,0));
        transform.position = destination;

    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, elementSpeed * Time.deltaTime);
        if (transform.position == destination)
        {
            destination = Vector3.Scale(
            Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f))),
            new Vector3(1, 1, 0));
        }

        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
        //transform.position = Vector3.Scale(Camera.main.ViewportToWorldPoint(destination),new Vector3(1,1,0));
    }
}

/*
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
 * */
