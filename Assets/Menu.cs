using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    //Main UI
    public Canvas mainMenu;

    //Pause Menu
    public Canvas pauseMenu;
    public Button exitButton;
    public Button resumeButton;
    private bool paused;

    //Players Menu
    public Canvas playersMenu;
    public Button playersButton;
    private bool playersMenuDisplayed;

    //Event Menu
    public Canvas eventMenu;
    public Transform eventSprite;
    public Transform eventName;
    public Button fightButton, chanceButton, danceButton;

    // Use this for initialization
    void Start()
    {
        //Pause Menu
        Button btnExit = exitButton.GetComponent<Button>();
        btnExit.onClick.AddListener(quitGame);
        Button btnResume = resumeButton.GetComponent<Button>();
        btnResume.onClick.AddListener(resumeGame);

        pauseMenu.enabled = false;
        paused = false;

        //Players Menu
        playersMenuDisplayed = false;
        Button btnPlayers = playersButton.GetComponent<Button>();
        btnPlayers.onClick.AddListener(playersInfo);

        //Event Menu
        eventMenuHideButtons();
    }

    // ---------- ---------- ---------- ---------- 
    void Update()
    {
        // ---------- ---------- ---------- ---------- 
        //Paused        //Input.GetButtonDown("Esc")
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (eventMenu.enabled)
            {
                eventMenu.enabled = false;
                eventMenuHideButtons();
            }
            else if (playersMenu.enabled)
                playersMenu.enabled = false;
            else if (paused == true)     //Pause Menu  //Time.timeScale == 0
            {
                resumeGame();
            }
            else
            {
                //Time.timeScale = 0;
                pauseMenu.enabled = true;
                paused = true;
            }
        }

        // ---------- ---------- ---------- ---------- 
        //Players Menu
        if (Input.GetButtonDown("Players"))
            playersInfo();

    }



    // ---------- ---------- ---------- ---------- 
    // EXIT
    void quitGame()
    {
        Application.Quit();
        Debug.Log("|Exit|");
    }

    // ---------- ---------- ---------- ---------- 
    //RESUME
    void resumeGame()
    {
        //Time.timeScale = 1;
        pauseMenu.enabled = false;
        paused = false;
        Debug.Log("|Resuming|");
    }

    // ---------- ---------- ---------- ---------- 
    //PLAYERS
    void playersInfo()
    {
        if (playersMenu.enabled == true)
            playersMenu.enabled = false;
        else
        {
            playersMenu.enabled = true;
        }
        Debug.Log("|Players|");
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT MENU
    public void eventMenuPrepare(Area area)
    {
        eventMenuHideButtons();
        eventMenu.enabled = true;
        eventSprite.GetComponent<Image>().sprite = area.Image;
        eventName.GetComponent<Text>().text = area.Type;

        Debug.Log(area.Type);

        switch (area.Type)
        {
            case "Bar":
                fightButton.gameObject.SetActive(true);
                chanceButton.gameObject.SetActive(true);
                break;
            case "Club":
                danceButton.gameObject.SetActive(true);
                fightButton.gameObject.SetActive(true);
                break;
            case "Disco":
                danceButton.gameObject.SetActive(true);
                break;
            case "Hostel":
                chanceButton.gameObject.SetActive(true);
                break;
        }

        //////////////////////////////
        // Eli:     Color.red       //
        // Nina:    Color.yellow    //
        // Riviera: Color.green     //
        // Blue:    Color.blue      //
        //////////////////////////////

        Color playersColor = Color.blue;
        switch (this.GetComponentInParent<PrimaryLoop>().theControlledPlayer)
        {
            case "Eli":
                playersColor = Color.red;
                break;
            case "Nina":
                playersColor = Color.yellow;
                break;
            case "Riviera":
                playersColor = Color.green;
                break;
            case "Blue":
                playersColor = Color.blue;
                break;
        }

        area.Ring.gameObject.SetActive(true);
        area.Ring.GetComponent<Image>().color = playersColor;
        area.RingOuter.gameObject.SetActive(true);
        Light lt = area.Light.GetComponent<Light>();
        lt.color = playersColor;
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT MENU (HIDE BUTTONS)
    public void eventMenuHideButtons()
    {
        danceButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        chanceButton.gameObject.SetActive(false);
    }
}
