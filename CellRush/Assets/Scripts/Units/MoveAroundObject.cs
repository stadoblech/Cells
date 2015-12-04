using UnityEngine;
using System.Collections;

public class MoveAroundObject : MonoBehaviour {

    public float maxDistanceFromPlayer;
    public float speed;

    GameObject player;

    float playerSize;

    Vector3 destination;

    bool living = true;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSize = player.GetComponent<SpriteRenderer>().bounds.size.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (living)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);

            if (dist >= playerSize)
            {
                destination = player.transform.position;
            }
            if (transform.position == destination)
            {
                destination = player.transform.position + new Vector3(Random.Range(-playerSize, playerSize), Random.Range(-playerSize, playerSize));
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        else
        {
            transform.position += destination * speed * Time.deltaTime;
        }
    }


    public void setAwayDestination()
    {
        destination = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        living = false;
    }
}
