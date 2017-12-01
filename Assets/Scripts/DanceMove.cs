using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DanceMove : MonoBehaviour
{
    public float speed = 7f;
    private GameObject game;
    public int moveChoice;


    //////////////////////////////
    //START
    void Start()
    {
        //Find Game object
        game = GameObject.Find("_Game");

        //Button
        Button BtnMove = this.transform.GetChild(1).GetComponent<Button>();
        BtnMove.onClick.AddListener(doMove);

        StartCoroutine(SelfDestroy());
    }

    //////////////////////////////
    //UPDATE
    void Update()
    {
        this.GetComponent<RectTransform>().localPosition += new Vector3(0, speed, 0);
    }

    //////////////////////////////
    //FADE
    IEnumerator DoFade()
    {
        yield return new WaitForSeconds(1f);

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    //////////////////////////////
    //SELFDESTROY
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(3.2f);

        //lose point then destroy
        if (moveChoice != 5)
            game.GetComponent<DanceEvent>().MoveChoice(6);

        Destroy(gameObject);
        yield return null;
    }

    //////////////////////////////
    //DO MOVE (btn)
    void doMove()
    {
        game.GetComponent<DanceEvent>().MoveChoice(moveChoice);
        Destroy(gameObject);
    }
}
