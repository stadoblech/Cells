using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

    public float cameraZoomingSpeed = 0.5f;

    private float defaultCameraSize;

	void Start () {
        defaultCameraSize = Camera.main.orthographicSize;

        Camera.main.orthographicSize = 0.1f;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (!PlayerStats.gameOver)
            {
                if (Camera.main.orthographicSize >= defaultCameraSize)
                {
                    Camera.main.orthographicSize = defaultCameraSize;
                }
                else
                {
                    Camera.main.orthographicSize += cameraZoomingSpeed;
                }
            }
            else
            {
                Camera.main.orthographicSize -= cameraZoomingSpeed;
                if (Camera.main.orthographicSize <= 0.1f)
                {
                    Camera.main.orthographicSize = 0.1f;
                }
            }
        }
    }
}
