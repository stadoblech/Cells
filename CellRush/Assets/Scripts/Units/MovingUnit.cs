using UnityEngine;
using System.Collections;

public class MovingUnit : MonoBehaviour {

    GameObject player;

    public float maxGravDist = 4.0f;
    public float maxGravity = 35.0f;

    public float maxDistance = 8;

    public bool returning = false;
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        

        if (Vector3.Distance(player.transform.position, transform.position) >= maxDistance)
        {
            returning = true;
        }

        if (returning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (player.GetComponent<PlayerMovement>().moveSpeed*2) * Time.deltaTime);
            if (transform.position == player.transform.position)
                returning = false;
        }
        else
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= maxGravDist)
            {
                Vector3 v = player.transform.position - transform.position;
                GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);
            }
        }
        /*
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            returning = true;
        }
        */
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            transform.position = player.transform.position;
        }
	}
}


/*

 using UnityEngine;
 using System.Collections;
 
 public class Bullet2 : MonoBehaviour {
     
     public float maxGravDist = 4.0f;
     public float maxGravity = 35.0f;
 
     GameObject[] planets;
 
     void Start () {
         planets = GameObject.FindGameObjectsWithTag("Planet");
     }
     
     void FixedUpdate () {
         foreach(GameObject planet in planets) {
             float dist = Vector3.Distance(planet.transform.position, transform.position);
             if (dist <= maxGravDist) {
                 Vector3 v = planet.transform.position - transform.position;
                 rigidbody2D.AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);
             }
         }
     }
 }

*/