using UnityEngine;
using System.Collections;

public class Courtain : MonoBehaviour {

    public KeyCode newGameKey = KeyCode.S;
    public float courtainSpeed = 5;
    public float startDelay = 1;

    bool gameStarted = false;

    
	// Use this for initialization
	void Start () {
        createSprite();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(newGameKey))
        {
            gameStarted = true;
        }

        if (gameStarted)
        {
            Vector3 destination = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,0);

            transform.position = Vector3.MoveTowards(transform.position,destination,courtainSpeed * Time.deltaTime);

            if (transform.position == destination)
            {
                while (startDelay > 0)
                {
                    startDelay -= Time.deltaTime;
                }
                Application.LoadLevel(1);
            }
        }
	}

    void createSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        Texture2D texture = new Texture2D(100,100);


        sr.sprite = Sprite.Create(texture,new Rect(0,0,90f,90f),new Vector2(0.5f,0.5f));
        sr.color = new Color(0,191,243);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        /*
        transform.localScale.x = worldScreenWidth / width;
        transform.localScale.y = worldScreenHeight / height;
         * */

        transform.localScale = new Vector3((float)(worldScreenWidth / width),(float)(worldScreenHeight/height));
        transform.position = new Vector3((float)(worldScreenHeight * Camera.main.aspect),0);

    }
}
