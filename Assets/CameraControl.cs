using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public int speed;
    public bool ready;
    public Camera cam;

	// Use this for initialization
	void Start () {
        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && cam.enabled)
        { 
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        }
    }
}
