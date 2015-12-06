using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementsHandler : MonoBehaviour
{
    public GameObject element;
    
    [Tooltip("pocet resources ktere reprezentuji jeden element")]
    public int resourceAmountForElement = 5;

    public int maxNumberOfElements = 25;

    int actualNumberOfElements;
    int lastNumberOfElements;

    bool firstUpdate = true;
    bool lastUpdate = false;

    void Awake()
    {
        
    }

    void Update()
    {
        if (firstUpdate)
        {
            actualNumberOfElements = PlayerStats.numberOfResources / resourceAmountForElement;

            

            if (actualNumberOfElements > maxNumberOfElements)
            {
                actualNumberOfElements = maxNumberOfElements;
            }
            lastNumberOfElements = actualNumberOfElements;

            for (int i = 0; i < actualNumberOfElements; i++)
            {
                Instantiate(element);
            }

            firstUpdate = false;
        }

        if (PlayerStats.gameOver && !lastUpdate)
        {
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Element"))
            {
                if (o.GetComponent<ElementLife>().living)
                {
                    o.GetComponent<ElementLife>().living = false;
                }
            }
            lastUpdate = true;
        }

        actualNumberOfElements = PlayerStats.numberOfResources / resourceAmountForElement;
        
        if (actualNumberOfElements > 25)
        {
            actualNumberOfElements = 25;
        }

        if (actualNumberOfElements > lastNumberOfElements)
        {
            for (int i = 0; i < actualNumberOfElements - lastNumberOfElements;i++)
                Instantiate(element);
            lastNumberOfElements = actualNumberOfElements;
        }
        else if(actualNumberOfElements < lastNumberOfElements)
        {
            int elementsToDestroy = -(actualNumberOfElements - lastNumberOfElements);
            int el = 0;
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Element"))
            {
                if (o.GetComponent<ElementLife>().living)
                {
                    o.GetComponent<ElementLife>().living = false;
                }
                el++;
                if (el == elementsToDestroy)
                {
                    break;
                }
            }
            lastNumberOfElements = actualNumberOfElements;
        }

        /*
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
            
        }
         * */



        
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