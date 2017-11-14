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

    //Stats
    private GameObject thePlayer;
    private int theChoice;
    private int p_HP, p_STR, p_DEX, p_LCK;
    private int o_HP, o_STR, o_DEX, o_LCK;
    private string display;
    public RectTransform p_Healthbar, o_Healthbar;

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
                break;
            case "Nina":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Nina;
                break;
            case "Riviera":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Riviera;
                break;
            case "Blue":
                thePlayer = this.GetComponentInParent<PrimaryLoop>().Blue;
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


        //UI & Camera (switch from main to fight)
        mainCam.enabled = false;
        fightCam.enabled = true;
        camController.GetComponent<CameraControl>().ready = false;
        fightCanvas.enabled = true;
        GetComponent<Menu>().mainMenu.enabled = false;
        GetComponent<Menu>().eventMenu.enabled = false;
        GetComponent<Menu>().eventMenuHideButtons();

        yield return new WaitForSeconds(2);

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
                thePlayer.GetComponent<Player>().Money -= 10; 
                break;
            case "Win":
                Debug.Log("| ~ WIN ~ ");
                thePlayer.GetComponent<Player>().Money += 20;
                this.GetComponentInParent<Menu>().winArea();
                break;
            case "Draw":
                Debug.Log("| ~ DRAW ~ ");
                break;
            default:
                break;
        }


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
                //display = o_STR.ToString();
                break;
            case 'p':
                o_HP = o_HP - p_STR;
                //display = p_STR.ToString();
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
                //display = "Pumping Up";
                break;
            case 'p':
                p_STR = p_STR * 2;
                p_DEX = p_DEX + 2;
                //display = "Pumping Up";
                break;
        }
    }
    ////////////////////////////////////////////////////////////
    //SUCKERPUNCH   (2 damage & -2 STR & heal 2 Health))
    public void Move3(char who_)
    {
        Debug.Log("SUCKERPUNCH " + who_);
        switch (who_)
        {
            case 'o':
                o_HP = o_HP + 2;
                if (o_HP > 20)
                    o_HP = 20;
                p_HP = p_HP - 2;
                p_STR = p_STR - 2;
                if (p_STR < 1)
                    p_STR = 1;
                //display = "2 + Debuff";
                break;
            case 'p':
                p_HP = p_HP + 2;
                if (p_HP > 20)
                    p_HP = 20;
                o_HP = o_HP - 2;
                o_STR = o_STR - 2;
                if (o_STR < 1)
                    o_STR = 1;
                //display = "2 + Debuff";
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
                //display = o_STR.ToString();
                break;
            case 'p':
                o_HP = o_HP - p_DEX;
                //display = p_STR.ToString();
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
