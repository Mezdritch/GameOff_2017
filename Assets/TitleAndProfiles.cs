using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleAndProfiles : MonoBehaviour {

    public Canvas canvasProfile1;
    public Canvas canvasProfile2;
    public Canvas canvasTitle;
    public GameObject particle1;
    public GameObject particle2;

    public Button profileButton;
    public Button exitButton;

    public Button zenButton, ninaButton, rivieraButton, blueButton;

    public GameObject playerEli, playerNina, playerRiviera, playerBlue;

    //Info
    public Transform infoPanel;
    private bool seeInfo = false;

    // ---------- ---------- ---------- ---------- 
    // START
    void Start () {
		canvasProfile1.enabled = false;
        canvasProfile2.enabled = false;

        particle1.GetComponent<Renderer>().enabled = false;
        particle2.GetComponent<Renderer>().enabled = false;

        //Buttons
        Button btnProfile = profileButton.GetComponent<Button>();
        btnProfile.onClick.AddListener(SwitchToProfiles);
        Button btnExit = exitButton.GetComponent<Button>();
        btnExit.onClick.AddListener(quitGame);

        //Buttons Player Profiles (Choices)
        Button btnZen = zenButton.GetComponent<Button>();
        btnZen.onClick.AddListener(() => chooseProfile(1));
        Button btnNina = ninaButton.GetComponent<Button>();
        btnNina.onClick.AddListener(() => chooseProfile(2));
        Button btnRiviera = rivieraButton.GetComponent<Button>();
        btnRiviera.onClick.AddListener(() => chooseProfile(3));
        Button btnBlue = blueButton.GetComponent<Button>();
        btnBlue.onClick.AddListener(() => chooseProfile(4));
    }


    // ---------- ---------- ---------- ---------- 
    // SWITCH TO PROFILE
    void SwitchToProfiles()
    {
        canvasProfile1.enabled = true;
        canvasProfile2.enabled = true;
        canvasTitle.enabled = false;

        particle1.GetComponent<Renderer>().enabled = true;
        particle2.GetComponent<Renderer>().enabled = true;
    }

    // ---------- ---------- ---------- ---------- 
    // EXIT
    void quitGame()
    {
        Application.Quit();
        Debug.Log("|Exit|");
    }

    // ---------- ---------- ---------- ---------- 
    // CHOOSE PROFILES
    void chooseProfile(int choice)
    {
        switch(choice)
        {
            case 1:
                playerEli.GetComponent<Player>().controlled = true;
                break;
            case 2:
                playerNina.GetComponent<Player>().controlled = true;
                break;
            case 3:
                playerRiviera.GetComponent<Player>().controlled = true;
                break;
            case 4:
                playerBlue.GetComponent<Player>().controlled = true;
                break;
        }
        Debug.Log("|Choice: " + choice +" |");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        Debug.Log("Loading" + 1);
    }

    // ---------- ---------- ---------- ---------- 
    //INFOPANELS
    // Info
    // 
    public void actionInfo()
    {
        if (!seeInfo)
        {
            StartCoroutine(FadeBack());
            seeInfo = true;
        }
        else if (seeInfo)
        {
            StartCoroutine(DoFade());
            seeInfo = false;
        }
    }

    //FADE /////
    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = infoPanel.gameObject.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    //FADEBACK /////
    IEnumerator FadeBack()
    {
        CanvasGroup canvasGroup = infoPanel.gameObject.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 0.98f)
        {
            canvasGroup.alpha += 0.1f;
            yield return null;
        }
    }
}
