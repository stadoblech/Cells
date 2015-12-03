using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementsHandler : MonoBehaviour {

    public GameObject element;

    private List<GameObject> elements = new List<GameObject>();

    [Tooltip("kolik elementu je vzdy pritomno nahore + dole tzn numberOfElements... * 2")]
    public int numberOfElementsByOneSide;
    

    private int actualNumberOfElements;

    void Start () {
        actualNumberOfElements = numberOfElementsByOneSide * 2;
        
        for (int bot = 0; bot < numberOfElementsByOneSide; bot++)
        {
            element.transform.position = Element.startingElementPosition("bot");
            element.GetComponent<ElementMovement>().position = "bot";
            Instantiate(element);
        }

        for (int top = 0; top < numberOfElementsByOneSide; top++)
        {
            element.transform.position = Element.startingElementPosition("top");
            element.GetComponent<ElementMovement>().position = "top";
            Instantiate(element);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
