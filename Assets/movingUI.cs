using UnityEngine;
using System.Collections;

public class movingUI : MonoBehaviour
{

    public RectTransform uiTarget;
    public RectTransform ui;
    Vector3 startingPosition;
    public float speed;
    public bool moveUp;
    public bool timeToMove;

    //////////////////////////////
    //START
    void Start()
    {
        startingPosition = ui.localPosition;
        speed = 10f;
    }

    //////////////////////////////
    //UPDATE
    void Update()
    {
        if (timeToMove && moveUp && ui.position.y < uiTarget.position.y)
            ui.localPosition += new Vector3(0, speed, 0);
        else if (timeToMove && !moveUp && ui.position.y > uiTarget.position.y)
            ui.localPosition -= new Vector3(0, speed, 0);
    }

    //////////////////////////////
    //RESET
    public void ResetToStarting()
    {
        timeToMove = false;
        ui.localPosition = new Vector3(startingPosition.x, startingPosition.y, startingPosition.z);
    }
}
