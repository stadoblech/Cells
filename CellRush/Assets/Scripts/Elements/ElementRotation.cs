using UnityEngine;
using System.Collections;

public class ElementRotation : MonoBehaviour {

    float rotationSpeed = 45;
	// Use this for initialization
	void Start () {
        rotationSpeed = Random.Range(-300,300);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0,0,rotationSpeed*Time.deltaTime);
	}
}
