using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    //Main UI
    public Canvas mainMenu;
    public Transform EliLog, NinaLog, RivieraLog, BlueLog, ExtraLog;

    //Pause Menu
    public Canvas pauseMenu;
    public Button exitButton;
    public Button resumeButton;
    private bool paused;

    //Players Menu
    public Canvas playersMenu;
    public Button playersButton;
    private bool playersMenuDisplayed;
    public Transform EliMoney, NinaMoney, RivieraMoney, BlueMoney;
    public Transform EliSuccess, NinaSuccess, RivieraSuccess, BlueSuccess;
    public Transform EliFame, NinaFame, RivieraFame, BlueFame;

    //Event Menu
    public Canvas eventMenu;
    public Transform eventSprite, eventName, eventPlayer, eventMoney, eventSuccess, eventFame;
    public Button fightButton, chanceButton, danceButton;

    //Area properties
    public Area eventArea;
    private string areaPlayer;

    //Map
    public Button mapButton;
    public Camera mainCam;
    public Camera mapCam;

    //GAme End
    public GameObject GameEndPanel;
    public Transform GameEndText;

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

        //Map
        Button btnmap = mapButton.GetComponent<Button>();
        btnmap.onClick.AddListener(showMap);

        //Event Menu
        eventMenuHideButtons();

        //Game End
        GameEndPanel.SetActive(false);
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
        if (Input.GetButtonDown("Map"))
            showMap();

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
    public void playersInfo()
    {
        if (playersMenu.enabled == true)
            playersMenu.enabled = false;
        else
        {
            GameObject eli = this.GetComponentInParent<PrimaryLoop>().Eli;
            GameObject nina = this.GetComponentInParent<PrimaryLoop>().Nina;
            GameObject riviera = this.GetComponentInParent<PrimaryLoop>().Riviera;
            GameObject blue = this.GetComponentInParent<PrimaryLoop>().Blue;
            //Update
            //Eli
            EliMoney.GetComponent<Text>().text = eli.GetComponent<Player>().Money.ToString();
            EliSuccess.GetComponent<Text>().text = eli.GetComponent<Player>().Success.ToString();
            EliFame.GetComponent<Text>().text = eli.GetComponent<Player>().Fame.ToString();
            //Nina
            NinaMoney.GetComponent<Text>().text = nina.GetComponent<Player>().Money.ToString();
            NinaSuccess.GetComponent<Text>().text = nina.GetComponent<Player>().Success.ToString();
            NinaFame.GetComponent<Text>().text = nina.GetComponent<Player>().Fame.ToString();
            //Riviera
            RivieraMoney.GetComponent<Text>().text = riviera.GetComponent<Player>().Money.ToString();
            RivieraSuccess.GetComponent<Text>().text = riviera.GetComponent<Player>().Success.ToString();
            RivieraFame.GetComponent<Text>().text = riviera.GetComponent<Player>().Fame.ToString();
            //Blue
            BlueMoney.GetComponent<Text>().text = blue.GetComponent<Player>().Money.ToString();
            BlueSuccess.GetComponent<Text>().text = blue.GetComponent<Player>().Success.ToString();
            BlueFame.GetComponent<Text>().text = blue.GetComponent<Player>().Fame.ToString();

            //Show
            playersMenu.enabled = true;
        }
        Debug.Log("|Players|");
    }

    // ---------- ---------- ---------- ---------- 
    //MAP
    void showMap()
    {
        if (playersMenu.enabled == true)
            playersMenu.enabled = false;

        //Cams
        if (mainCam.enabled)
        {
            mainCam.enabled = false;
            mapCam.enabled = true;
        }
        else if (mapCam.enabled)
        {
            mainCam.enabled = true;
            mapCam.enabled = false;
        }
    }


    // ---------- ---------- ---------- ---------- 
    //EVENT MENU
    public void eventMenuPrepare(Area area)
    {
        //Keep the area properties
        eventArea = area;
        eventPlayer.GetComponent<Text>().text = eventArea.Player;
        eventMoney.GetComponent<Text>().text = eventArea.Money.ToString();
        eventSuccess.GetComponent<Text>().text = eventArea.Success.ToString();
        eventFame.GetComponent<Text>().text = eventArea.Fame.ToString();

        //UI
        eventMenuHideButtons();
        eventMenu.enabled = true;
        eventSprite.GetComponent<Image>().sprite = area.Image;
        eventName.GetComponent<Text>().text = area.Type;

        Debug.Log(area.Type);

        //Display choices if not already owned by the player
        if (eventArea.Player != this.GetComponentInParent<PrimaryLoop>().theControlledPlayer)
        {
            switch (area.Type)
            {
                case "Bar":
                    fightButton.gameObject.SetActive(true);
                    chanceButton.gameObject.SetActive(true);
                    break;
                case "Casino":
                    chanceButton.gameObject.SetActive(true);
                    break;
                case "Club":
                    danceButton.gameObject.SetActive(true);
                    break;
                case "Disco":
                    danceButton.gameObject.SetActive(true);
                    fightButton.gameObject.SetActive(true);
                    break;
                case "Hotel":
                    chanceButton.gameObject.SetActive(true);
                    break;
                case "Industrial":
                    fightButton.gameObject.SetActive(true);
                    break;
            }
        }
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT MENU (HIDE BUTTONS)
    public void eventMenuHideButtons()
    {
        danceButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        chanceButton.gameObject.SetActive(false);
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT - WIN AREA
    public void winArea(string player)
    {
        //////////////////////////////
        // Eli:     Color.red       //
        // Nina:    Color.yellow    //
        // Riviera: Color.green     //
        // Blue:    Color.blue      //
        //////////////////////////////

        //Reward with area
        eventArea.Player = player;

        //Change UI to player
        Color playersColor = Color.white;
        switch (eventArea.Player)
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

        //Log
        updateLogs(player, "Gain", eventArea.Type);

        eventArea.Ring.gameObject.SetActive(true);
        eventArea.Ring.GetComponent<Image>().color = playersColor;
        eventArea.RingOuter.gameObject.SetActive(true);
        Light lt = eventArea.Light.GetComponent<Light>();
        lt.color = playersColor;
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT - FAIL AREA
    public void failArea(string player)
    {
        //Log
        updateLogs(player, "Fail", eventArea.Type);
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT - WIN AREA
    public void aiWinArea(Area area, string player)
    {
        Debug.Log("Win - area:" + area);
        Debug.Log("Win - player:" + player);
        eventArea = area;
        eventArea.Player = player;
        winArea(player);
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT - LOSE AREA
    public void aiLoseArea(Area area, string player)
    {
        Debug.Log("Lose - area:" + area);
        Debug.Log("Lose - player:" + player);
        updateLogs(player, "Fail", area.Type);
    }

    // ---------- ---------- ---------- ---------- 
    //EVENT - FEEDBACK (UPDATE LOGS)
    public void updateLogs(string player, string type, string area)
    {
        string LogEntry = "";
        string LogType = "";

        //Event
        switch (type)
        {
            case "Fail":
                LogType = "Failed to take control of a " + area + ".";
                break;
            case "Gain":
                LogType = "Took control of a " + area + ".";
                break;
            case "Take":
                break;
            case "Win":
                break;

        }

        LogEntry = player + " ‒ " + LogType;
        //Player
        switch (player)
        {
            case "Eli":
                EliLog.GetComponent<Text>().text = LogEntry;
                break;
            case "Nina":
                NinaLog.GetComponent<Text>().text = LogEntry;
                break;
            case "Riviera":
                RivieraLog.GetComponent<Text>().text = LogEntry;
                break;
            case "Blue":
                BlueLog.GetComponent<Text>().text = LogEntry;
                break;
            case "Extra":
                ExtraLog.GetComponent<Text>().text = LogEntry;
                break;
        }
        //Check if someone wins
        string winner = this.GetComponentInParent<PrimaryLoop>().GameOver();
        if (winner != null)
            GameEnd(winner);
    }

    // ---------- ---------- ---------- ---------- 
    //GAME ENDS
    public void GameEnd(string player)
    {
        string who;
        if (player == this.GetComponentInParent<PrimaryLoop>().theControlledPlayer)
            who = "You";
        else
            who = player;

        GameEndText.GetComponent<Text>().text = who + " win!";
    }

}
