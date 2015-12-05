using UnityEngine;
using System.Collections;

public class ElementLife : MonoBehaviour {

    public bool living = true;

    public float scaleSizingSpeed = 0.1f;

	void Start () {
        transform.localScale = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.localScale.x <= 1 && living)
        {
            transform.localScale += new Vector3(scaleSizingSpeed,scaleSizingSpeed,scaleSizingSpeed); 
        }

        if (!living)
        {
            transform.localScale -= new Vector3(scaleSizingSpeed, scaleSizingSpeed, scaleSizingSpeed);
        }

        if (transform.localScale.x <= 0.01)
        {
            Destroy(gameObject);
        }
	}
}
