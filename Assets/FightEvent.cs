using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightEvent : MonoBehaviour {

    //Init, choice, resolve
    private IEnumerable state;

    //Camera
    public Camera mainCam;
    public Camera fightCam;
    public GameObject camController;
    public Canvas fightCanvas;

    //UI
    public RectTransform VsPanel;
    public RectTransform ButtonsPanel;
    public RectTransform RoundIncreased;
    public Image vsPlayer, vsOpponent;
    public Sprite imgEli, imgNina, imgRiviera, imgBlue;
    public Sprite imgNW, imgCY;

    //Stats
    private GameObject thePlayer;
    private int theChoice;
    private int p_HP, p_STR, p_DEX, p_LCK;
    private int o_HP, o_STR, o_DEX, o_LCK;
    private string display;
    public RectTransform p_Healthbar, o_Healthbar;

    //Feedback
    public Transform p_Popup, o_Popup, p_end;
    public GameObject Punch_Popup, Kick_Popup, Pump_Popup, Sucker_Popup;
    public GameObject Lost_Popup, Win_Popup, Draw_Popup;

    //////////////////////////////
    // Start
    void Start()
    {
    }


    //////////////////////////////
    //STATE MACHINE
    public IEnumerator RunStateMachine()
    {
        while (state != null)
        {
            foreach (var cur in state)
            {
                yield return cur;
            }
        }
    }


    //////////////////////////////
    // START FIGHT
    public void StartFight()
    {
        //Start with the Init state
        state = Init();


        //Start state machine
        StartCoroutine(RunStateMachine());
    }


    //////////////////////////////
    // INIT
    private IEnumerable Init()
    {
        Debug.Log("| Fight |" + state);

        //Prepare stats
        ///////////////
        //PLAYER
        switch (this.GetComponentInParent<PrimaryLoop>().theControlledPlayer)
        {
            case "Eli":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Eli;
                vsPlayer.sprite = imgEli;
                break;
            case "Nina":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Nina;
                vsPlayer.sprite = imgNina;
                break;
            case "Riviera":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Riviera;
                vsPlayer.sprite = imgRiviera;
                break;
            case "Blue":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Blue;
                vsPlayer.sprite = imgBlue;
                break;
        }
        p_HP = thePlayer.GetComponent<Player>().HP;
        p_STR = thePlayer.GetComponent<Player>().STR;
        p_DEX = thePlayer.GetComponent<Player>().DEX;
        p_LCK = thePlayer.GetComponent<Player>().LCK;

        //OPPONENT
        o_HP = 20;
        o_STR = 3;
        o_DEX = 3;
        o_LCK = 3;

        //Avatar
        vsOpponent.sprite = imgNW;

        //UI & Camera (switch from main to fight)
        mainCam.enabled = false;
        fightCam.enabled = true;
        camController.GetComponent<CameraControl>().ready = false;
        fightCanvas.enabled = true;
        GetComponent<Menu>().mainMenu.enabled = false;
        GetComponent<Menu>().eventMenu.enabled = false;
        GetComponent<Menu>().eventMenuHideButtons();

        yield return new WaitForSeconds(1.5f);

        VsPanel.GetComponent<movingUI>().timeToMove = true;
        ButtonsPanel.GetComponent<movingUI>().timeToMove = true;

        state = Choice();
    }


    //////////////////////////////
    // CHOICE
    private IEnumerable Choice()
    {
        Debug.Log("| Fight |" + state);


        yield return StartCoroutine(WaitForPlayersChoice());

        switch (theChoice)
        {
            case 1:
                Move1('p');
                break;
            case 2:
                Move2('p');
                break;
            case 3:
                Move3('p');
                break;
            case 4:
                Move4('p');
                break;
        }



        AIchoice();
        adjustHealthbars('o');
        adjustHealthbars('p');

        //Reset choice
        theChoice = 0;
        yield return null; //new WaitForSeconds(0.25f);

        //End?
        if (p_HP <= 0 && o_HP > 0)
            state = End("Lost");
        else if (p_HP > 0 && o_HP <= 0)
            state = End("Win");
        else if (p_HP <= 0 && o_HP <= 0)
            state = End("Draw");
        else
            state = Choice();
    }


    //////////////////////////////
    // RESOLVE
    private IEnumerable Resolve()
    {
        Debug.Log("| Fight |" + state);

        yield return new WaitForSeconds(2);
        state = End("");
    }


    //////////////////////////////
    // END
    private IEnumerable End(string WinLost)
    {
        Debug.Log("| Fight |" + state);

        switch (WinLost)
        {
            case "Lost":
                Debug.Log("| ~ LOST ~ ");
                GameObject go3 = Instantiate(Lost_Popup) as GameObject;
                go3.transform.position = p_end.position;

                thePlayer.GetComponent<Player>().Money -= 10;
                this.GetComponentInParent<Menu>().failArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer); 
                break;
            case "Win":
                Debug.Log("| ~ WIN ~ ");
                GameObject go4 = Instantiate(Win_Popup) as GameObject;
                go4.transform.position = p_end.position;

                //thePlayer.GetComponent<Player>().Money += 20;
                thePlayer.GetComponent<Player>().Money += System.Int32.Parse(this.GetComponentInParent<Menu>().eventMoney.GetComponent<Text>().text);
                thePlayer.GetComponent<Player>().Success += System.Int32.Parse(this.GetComponentInParent<Menu>().eventSuccess.GetComponent<Text>().text);
                thePlayer.GetComponent<Player>().Fame += System.Int32.Parse(this.GetComponentInParent<Menu>().eventFame.GetComponent<Text>().text);
                this.GetComponentInParent<Menu>().winArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer);
                break;
            case "Draw":
                Debug.Log("| ~ DRAW ~ ");
                GameObject go5 = Instantiate(Draw_Popup) as GameObject;
                go5.transform.position = p_end.position;

                this.GetComponentInParent<Menu>().failArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer);
                break;
            default:
                break;
        }

        //Hold on for 2 sec
        yield return new WaitForSeconds(2);

        //UI & Camera (switch from fight to main)
        mainCam.enabled = true;
        fightCam.enabled = false;
        camController.GetComponent<CameraControl>().ready = true;
        fightCanvas.enabled = false;
        GetComponent<Menu>().mainMenu.enabled = true;

        //Reset UI
        p_Healthbar.sizeDelta = new Vector2(20 * 44.5f, p_Healthbar.sizeDelta.y);
        o_Healthbar.sizeDelta = new Vector2(20 * 44.5f, o_Healthbar.sizeDelta.y);

        VsPanel.GetComponent<movingUI>().ResetToStarting();
        ButtonsPanel.GetComponent<movingUI>().ResetToStarting();


        //Increase the amount of round by 1;
        //RoundIncreased += 1;
        int _roundIncreased = System.Int32.Parse(RoundIncreased.GetComponent<Text>().text);
        RoundIncreased.GetComponent<Text>().text = (_roundIncreased + 1).ToString();

        //Event for AIs
        this.GetComponentInParent<City>().TurnEvents();

        yield return null;
        state = null;
    }


    //////////////////////////////
    // Waiting for the user to choose
    IEnumerator WaitForPlayersChoice()
    {
        theChoice = 0;
        while (theChoice == 0)
            yield return null;
    }


    //////////////////////////////
    // Players choice (input from button)
    public void MoveChoice( int btnChoice)
    {
        theChoice = btnChoice;
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    // MOVES
    ////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////
    //PUNCH         (STR)
    public void Move1(char who_)
    {
        Debug.Log("PUNCH " + who_);
        switch (who_)
        {
            case 'o':
                p_HP = p_HP - o_STR;
                //Popup (Opponent)
                GameObject go2 = Instantiate(Punch_Popup) as GameObject;
                go2.transform.position = o_Popup.position;
                break;
            case 'p':
                o_HP = o_HP - p_STR;
                //Popup (Player)
                GameObject go = Instantiate(Punch_Popup) as GameObject;
                go.transform.position = p_Popup.position;
                break;
        }
    }
    ////////////////////////////////////////////////////////////
    //CHARGE        (STR*2) (DEX+2)
    public void Move2(char who_)
    {
        Debug.Log("CHARGE " + who_);
        switch (who_)
        {
            case 'o':
                o_STR = o_STR * 2;
                o_DEX = o_DEX + 2;
                //Popup (Opponent)
                GameObject go2 = Instantiate(Pump_Popup) as GameObject;
                go2.transform.position = o_Popup.position;
                break;
            case 'p':
                p_STR = p_STR * 2;
                p_DEX = p_DEX + 2;
                //Popup (Player)
                GameObject go = Instantiate(Pump_Popup) as GameObject;
                go.transform.position = p_Popup.position;
                break;
        }
    }
    ////////////////////////////////////////////////////////////
    //SUCKERPUNCH   (1 damage & -2 STR & heal 1 Health))
    public void Move3(char who_)
    {
        Debug.Log("SUCKERPUNCH " + who_);
        switch (who_)
        {
            case 'o':
                o_HP = o_HP + 1;
                if (o_HP > 20)
                    o_HP = 20;
                p_HP = p_HP - 1;
                p_STR = p_STR - 2;
                if (p_STR < 1)
                    p_STR = 1;
                //Popup (Opponent)
                GameObject go2 = Instantiate(Sucker_Popup) as GameObject;
                go2.transform.position = o_Popup.position;
                break;
            case 'p':
                p_HP = p_HP + 2;
                if (p_HP > 20)
                    p_HP = 20;
                o_HP = o_HP - 1;
                o_STR = o_STR - 2;
                if (o_STR < 1)
                    o_STR = 1;
                //Popup (Player)
                GameObject go = Instantiate(Sucker_Popup) as GameObject;
                go.transform.position = p_Popup.position;
                break;
        }
    }
    ////////////////////////////////////////////////////////////
    //KICK          (DEX)
    public void Move4(char who_)
    {
        Debug.Log("KICK " + who_);
        switch (who_)
        {
            case 'o':
                p_HP = p_HP - o_DEX;
                //Popup (Opponent)
                GameObject go2 = Instantiate(Kick_Popup) as GameObject;
                go2.transform.position = o_Popup.position;
                break;
            case 'p':
                o_HP = o_HP - p_DEX;
                //Popup (Player)
                GameObject go = Instantiate(Kick_Popup) as GameObject;
                go.transform.position = p_Popup.position;
                break;
        }
    }

    private void adjustHealthbars(char who_)
    {
        switch (who_)
        {
            case 'o':
                Debug.Log("HP o " + o_HP);
                o_Healthbar.sizeDelta = new Vector2(o_HP * 44.5f, o_Healthbar.sizeDelta.y);
                break;
            case 'p':
                Debug.Log("HP p " + p_HP);
                p_Healthbar.sizeDelta = new Vector2(p_HP * 44.5f, p_Healthbar.sizeDelta.y);
                break;
        }

    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    // AI (dummy)
    ////////////////////////////////////////////////////////////

    private void AIchoice()
    {

        int randomOf = 1;
        randomOf = Random.Range(1, 5);

        switch (randomOf)
        {
            case 1:
                Move1('o');
                break;
            case 2:
                Move2('o');
                break;
            case 3:
                Move3('o');
                break;
            case 4:
            default:
                Move4('o');
                break;
        }

        
    }
}
