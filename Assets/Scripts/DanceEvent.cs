using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanceEvent : MonoBehaviour
{

    //Init, choice, resolve
    private IEnumerable state;

    //Camera
    public Camera mainCam, danceCam;
    public GameObject camController;
    public Canvas danceCanvas;

    //UI
    public RectTransform VsPanel;
    public RectTransform RoundIncreased;
    public Image vsPlayer, vsOpponent;
    public Sprite imgEli, imgNina, imgRiviera, imgBlue;
    public Sprite imgCY;

    //Stats
    private GameObject thePlayer;
    private int theChoice;
    private int p_DEX, p_LCK;
    private string display;
    public RectTransform pointsBar;
    private int _totalPoints, _points;
    private int _wrongMove;

    //Feedback
    public Transform p_Popup, o_Popup, p_end;
    public GameObject Hustle_Popup, NightFever_Popup, Bump_Popup, Magicfoot_Popup;
    public GameObject Lost_Popup, Win_Popup;

    //Spawning
    public List<GameObject> spawner;

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
    // START DANCE
    public void StartDance()
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
        Debug.Log("| Dance |" + state);

        //////////////////////////////
        //Destroy previously unclicked moves
        var UnMoves = GameObject.FindGameObjectsWithTag("Move");
        DestroyMoves(UnMoves);

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
        p_DEX = thePlayer.GetComponent<Player>().DEX;
        p_LCK = thePlayer.GetComponent<Player>().LCK;

        _wrongMove = 0;
        _points = 0;
        _totalPoints = 2;
        adjustPointbar(1);

        //Avatar
        vsOpponent.sprite = imgCY;

        //UI & Camera (switch from main to fight)
        mainCam.enabled = false;
        danceCam.enabled = true;
        camController.GetComponent<CameraControl>().ready = false;
        danceCanvas.enabled = true;
        GetComponent<Menu>().mainMenu.enabled = false;
        GetComponent<Menu>().eventMenu.enabled = false;
        GetComponent<Menu>().eventMenuHideButtons();

        yield return new WaitForSeconds(1.5f);

        //start spawning
        for (int i = 0; i < spawner.Count; i++)
        {
            spawner[i].GetComponent<DanceMoveSpawner>().StartMovesSpawning();
        }


        VsPanel.GetComponent<movingUI>().timeToMove = true;

        state = Choice();
    }


    //////////////////////////////
    // CHOICE
    private IEnumerable Choice()
    {
        Debug.Log("| Dance |" + state);

        yield return StartCoroutine(WaitForPlayersChoice());
        
        switch (theChoice)
        {
            case 1:
                Move1();
                break;
            case 2:
                Move2();
                break;
            case 3:
                Move3();
                break;
            case 4:
                Move4();
                break;
            case 5:
                Move5();
                break;
            case 6:
                pointLoss();
                break;
        }

        _totalPoints += _points;

        if (_totalPoints > 1000)
            _totalPoints = 1000;
        if (_totalPoints < 1)
            _totalPoints = 1;

        adjustPointbar(_totalPoints);

        //Reset choice
        theChoice = 0;
        yield return null; //new WaitForSeconds(0.25f);

        //End?
        if (_totalPoints < 2)
            state = End("Lost");
        else if (_totalPoints >= 1000)
            state = End("Win");
        else
            state = Choice();
    }


    //////////////////////////////
    // RESOLVE
    private IEnumerable Resolve()
    {
        Debug.Log("| Dance |" + state);

        yield return new WaitForSeconds(2);
        state = End("");
    }


    //////////////////////////////
    // END
    private IEnumerable End(string WinLost)
    {
        Debug.Log("| Dance |" + state);

        //stop spawning
        for (int i = 0; i < spawner.Count; i++)
        {
            spawner[i].GetComponent<DanceMoveSpawner>().StopSpawn();
        }

        //////////////////////////////
        //Won/Lost
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

                thePlayer.GetComponent<Player>().Money += System.Int32.Parse(this.GetComponentInParent<Menu>().eventMoney.GetComponent<Text>().text);
                thePlayer.GetComponent<Player>().Success += System.Int32.Parse(this.GetComponentInParent<Menu>().eventSuccess.GetComponent<Text>().text);
                thePlayer.GetComponent<Player>().Fame += System.Int32.Parse(this.GetComponentInParent<Menu>().eventFame.GetComponent<Text>().text);
                this.GetComponentInParent<Menu>().winArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer);
                break;
            default:
                break;
        }

        //Hold on for 2 sec
        yield return new WaitForSeconds(2);

        //////////////////////////////
        //Destroy unclicked moves
        var UnMoves = GameObject.FindGameObjectsWithTag("Move");
        DestroyMoves(UnMoves);

        //UI & Camera (switch from fight to main)
        mainCam.enabled = true;
        danceCam.enabled = false;
        camController.GetComponent<CameraControl>().ready = true;
        danceCanvas.enabled = false;
        GetComponent<Menu>().mainMenu.enabled = true;

        //Reset UI
        pointsBar.sizeDelta = new Vector2(1f, pointsBar.sizeDelta.y);

        VsPanel.GetComponent<movingUI>().ResetToStarting();

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
    public void MoveChoice(int btnChoice)
    {
        theChoice = btnChoice;
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    // MOVES
    ////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////
    //HUSTLE         (DEX*10)
    public void Move1()
    {
        _points = 0;
        Debug.Log("HUSTLE ");
        _points = p_DEX * 10;
        p_DEX += 3;

        //Popup (Player)
        GameObject go = Instantiate(Hustle_Popup) as GameObject;
        go.transform.position = p_Popup.position;
    }
    ////////////////////////////////////////////////////////////
    //BUMP        (DEX*5)(DEX stat +2)
    public void Move2()
    {
        _points = 0;
        Debug.Log("BUMP ");
        _points = p_DEX * 5;
        p_DEX += 2;

        //Popup (Player)
        GameObject go = Instantiate(Bump_Popup) as GameObject;
        go.transform.position = p_Popup.position;
    }
    ////////////////////////////////////////////////////////////
    //NIGHT FEVER   (DEX*2*LUCK)
    public void Move3()
    {
        _points = 0;
        Debug.Log("NIGHT FEVER  ");
        _points = p_DEX * 2 * p_LCK;

        //Popup (Player)
        GameObject go = Instantiate(NightFever_Popup) as GameObject;
        go.transform.position = p_Popup.position;
    }
    ////////////////////////////////////////////////////////////
    //MAGICFOOT          (LUCK*5*X)
    public void Move4()
    {
        _points = 0;
        Debug.Log("MAGICFOOT ");
        _points = Random.Range(2, 5) * 5 * p_LCK;

        //Popup (Player)
        GameObject go = Instantiate(Magicfoot_Popup) as GameObject;
        go.transform.position = p_Popup.position;
    }
    ////////////////////////////////////////////////////////////
    //WRONGMOVE          (Point loss (100-300))
    public void Move5()
    {
        _points = 0;
        Debug.Log("WRONGMOVE ");
        _points -= (Random.Range(100, 300));

        //Popup (Player)
        GameObject go = Instantiate(Magicfoot_Popup) as GameObject;
        go.transform.position = p_Popup.position;
    }

    ////////////////////////////////////////////////////////////
    //WRONGMOVE
    public void pointLoss()
    {
        _points = 0;
        _points -= 25;
    }
    

    ////////////////////////////////////////////////////////////
    //POINT BAR
    private void adjustPointbar(int points)
    {
        Debug.Log("Points (tot): " + points);
        pointsBar.sizeDelta = new Vector2(points, pointsBar.sizeDelta.y);
    }

    ////////////////////////////////////////////////////////////
    //DESTROY MOVES
    private void DestroyMoves(GameObject[] m)
    {
        foreach (GameObject moves in m)
        {
            Destroy(moves);
        }
    }
}
