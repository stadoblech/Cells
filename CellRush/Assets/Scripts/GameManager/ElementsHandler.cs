using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementsHandler : MonoBehaviour
{
    public GameObject element;
    
    [Tooltip("pocet resources ktere reprezentuji jeden element")]
    public int resourceAmountForElement = 5;

    int actualNumberOfElements;

    bool firstUpdate = true;

    void Awake()
    {
        
    }

    void Update()
    {
        if (firstUpdate)
        {
            actualNumberOfElements = PlayerStats.numberOfResources / resourceAmountForElement;
            
            if (actualNumberOfElements > 25)
            {
                actualNumberOfElements = 25;
            }

            for (int i = 0; i < actualNumberOfElements; i++)
            {
                Instantiate(element);
            }

            firstUpdate = false;
        }
        
        

        if (actualNumberOfElements != PlayerStats.numberOfResources / resourceAmountForElement)
        {
            if(actualNumberOfElements >= 25)
            {
                actualNumberOfElements = 25;
                actualNumberOfElements = PlayerStats.numberOfResources / resourceAmountForElement;
                return;
            }

            if (actualNumberOfElements > PlayerStats.numberOfResources / resourceAmountForElement)
            {
                for (int i = 0; i < (actualNumberOfElements - PlayerStats.numberOfResources / resourceAmountForElement); i++)
                {
                    if (GameObject.FindGameObjectsWithTag("Element").Length <= 0)
                    {
                        return;
                    }

                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Element"))
                    {
                        if (o.GetComponent<ElementLife>().living)
                        {
                            o.GetComponent<ElementLife>().living = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < (PlayerStats.numberOfResources / resourceAmountForElement - actualNumberOfElements);i++)
                {
                    Instantiate(element);
                }
            }
            actualNumberOfElements = PlayerStats.numberOfResources / resourceAmountForElement;
        }

        
    }




}















/*  OLD FUNCIONALITY
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
*/