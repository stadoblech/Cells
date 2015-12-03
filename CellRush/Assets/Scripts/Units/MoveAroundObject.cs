using UnityEngine;
using System.Collections;

public class MoveAroundObject : MonoBehaviour {

    public float maxDistanceFromPlayer;
    public float speed;

    GameObject player;

    bool returning = false;
    bool addedForce = false;

	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        player = GameObject.FindGameObjectWithTag("Player");

        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (!returning)
        {
            if (!addedForce)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-16f,16f),Random.Range(-16f,16f)));
                addedForce = true;
            }
            if (dist >= maxDistanceFromPlayer)
            {
                returning = true;
            }

            if (!GetComponent<SpriteRenderer>().isVisible)
            {
                transform.position = player.transform.position;
                returning = false;
                addedForce = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed * Time.deltaTime);
            if (transform.position == player.transform.position)
            {
                returning = false;
                addedForce = false;
            }
        }

	}
}
