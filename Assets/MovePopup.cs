using UnityEngine;
using System.Collections;

public class MovePopup : MonoBehaviour
{
    private float speed = 0.0125f;

    //START
    //Fades
    void Start()
    {
        StartCoroutine(DoFade());
    }

    //////////////////////////////
    //UPDATE
    void Update()
    {
            this.GetComponent<RectTransform>().localPosition += new Vector3(0, speed, 0);
    }

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
}
