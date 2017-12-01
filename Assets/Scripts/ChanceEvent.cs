using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ChanceEvent : MonoBehaviour
{

    //Init, choice, resolve
    private IEnumerable state;

    //Camera
    public Camera mainCam, chanceCam;
    public GameObject camController;
    public Canvas chanceCanvas;

    //UI
    public RectTransform RoundIncreased;
    public Transform totalOP, totalPlayer, winOrlose;
    public GameObject winOrlosePanel;
    public Button btnHit, btnStand;

    //Stats
    private GameObject thePlayer;
    private int theChoice;
    private int p_LCK, _Plpoints, _OpPoints;
    private string display;

    //Cards
    public List<GameObject> OpCards;
    public List<GameObject> PlayerCards;
    public List<Card> Cards = new List<Card>();
    private int _TopCard, _PlayerCard, _OppCard;
    private GameObject currentCard;
    private bool _PlayerStand, _OpStand;

    //Bet
    private int bet;
    public Transform showBet;


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
    // START CHANCE
    public void StartChance()
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
        Debug.Log("| Chance |" + state);

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
        p_LCK = thePlayer.GetComponent<Player>().LCK;

        //Bet
        bet = Random.Range(2*p_LCK, (10 + 2*p_LCK)) * 10;
        showBet.GetComponent<Text>().text = "Match bet: " + bet;

        //
        _Plpoints = 0;
        _OpPoints = 0;
        _PlayerCard = 0;
        _OppCard = 0;
        currentCard = null;
        _TopCard = 0;
        totalOP.GetComponent<Text>().text = "0";
        totalPlayer.GetComponent<Text>().text = "0";
        _PlayerStand = false;
        _OpStand = false;
        btnHit.enabled = true;

        //UI & Camera (switch from main to fight)
        mainCam.enabled = false;
        chanceCam.enabled = true;
        camController.GetComponent<CameraControl>().ready = false;
        chanceCanvas.enabled = true;
        GetComponent<Menu>().mainMenu.enabled = false;
        GetComponent<Menu>().eventMenu.enabled = false;
        GetComponent<Menu>().eventMenuHideButtons();

        //Prepare table
        PrepareUi();
        createDeck();
        winOrlosePanel.SetActive(false);
        

        yield return new WaitForSeconds(0.5f);

        //Intitial Hand
        PullACard("Player");
        yield return new WaitForSeconds(0.2f);
        PullACard("Player");
        yield return new WaitForSeconds(0.2f);
        PullACard("Opponent");
        yield return new WaitForSeconds(0.2f);
        PullACard("Opponent");

        //End?
        if (_Plpoints > 21)
            state = End("Lost");
        else if (_Plpoints == 21 || ((_Plpoints > _OpPoints) && _PlayerStand && _OpStand))
            state = End("Win");
        else if(_OpPoints == 21 || ((_OpPoints > _Plpoints) && _PlayerStand && _OpStand))
            state = End("Lost");
        else if (_OpPoints > 21)
            state = End("Win");
        else
        state = Choice();
    }


    //////////////////////////////
    // CHOICE
    private IEnumerable Choice()
    {
        Debug.Log("| Chance |" + state);

        yield return StartCoroutine(WaitForPlayersChoice());

        switch (theChoice)
        {
            case 1: //Hit
                PullACard("Player");
                break;
            case 2: //Stand
                _PlayerStand = true;
                break;
            case 3:

                break;
        }

        //Reset choice
        theChoice = 0;
        yield return null; //new WaitForSeconds(0.25f);

        //End?
        if (_Plpoints > 21)
            state = End("Lost");
        else if (_Plpoints == 21 || ((_Plpoints>_OpPoints) && _PlayerStand && _OpStand))
            state = End("Win");
        else
            state = AiChoice();
    }


    //////////////////////////////
    // AI CHOICE
    private IEnumerable AiChoice()
    {
        Debug.Log("| Chance |" + state);

        yield return new WaitForSeconds(1);

        int aiSaltiness = Random.Range(0, 10);

        if ((aiSaltiness <= p_LCK) && _OppCard != 5)
        {
            PullACard("Opponent");
            _OpStand = false;
        }
        else
        {
            if (_OpPoints <= 17)
            {
                PullACard("Opponent");
                _OpStand = false;
            }
            else if (_OppCard == 5)
                _OpStand = true;
            else
                _OpStand = true;
        }

        //End?
        if (_OpPoints == 21 || ((_OpPoints > _Plpoints) && _PlayerStand && _OpStand))
            state = End("Lost");
        else if (_OpPoints > 21)
            state = End("Win");
        else
        state = Choice();
    }


    //////////////////////////////
    // END
    private IEnumerable End(string WinLost)
    {
        Debug.Log("| Chance |" + state);

        winOrlosePanel.SetActive(true);

        //////////////////////////////
        //Won/Lost
        switch (WinLost)
        {
            case "Lost":
                Debug.Log("| ~ LOST ~ ");

                winOrlose.GetComponent<Text>().text = "-Loss-";

                thePlayer.GetComponent<Player>().Money -= bet;
                this.GetComponentInParent<Menu>().failArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer);
                break;
            case "Win":
                Debug.Log("| ~ WIN ~ ");

                winOrlose.GetComponent<Text>().text = "-Win!-";

                thePlayer.GetComponent<Player>().Money += (bet + System.Int32.Parse(this.GetComponentInParent<Menu>().eventMoney.GetComponent<Text>().text));
                thePlayer.GetComponent<Player>().Success += System.Int32.Parse(this.GetComponentInParent<Menu>().eventSuccess.GetComponent<Text>().text);
                thePlayer.GetComponent<Player>().Fame += System.Int32.Parse(this.GetComponentInParent<Menu>().eventFame.GetComponent<Text>().text);
                this.GetComponentInParent<Menu>().winArea(this.GetComponentInParent<PrimaryLoop>().theControlledPlayer);
                break;
            default:
                break;
        }

        //Hold on for 2 sec
        yield return new WaitForSeconds(2);

        //UI & Camera (switch from fight to main)
        mainCam.enabled = true;
        chanceCam.enabled = false;
        camController.GetComponent<CameraControl>().ready = true;
        chanceCanvas.enabled = false;
        GetComponent<Menu>().mainMenu.enabled = true;

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

    //////////////////////////////
    // Prepare UI
    public void PrepareUi()
    {
        for (int i = 0; i < 5; i++)
        {
            OpCards[i].SetActive(false);
            PlayerCards[i].SetActive(false);
        }
        winOrlosePanel.SetActive(false);
    }

    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    // DECK MANIP.
    ////////////////////////////////////////////////////////////

    //////////////////////////////
    // Create Deck
    public void createDeck()
    {
        //new
        Cards.Clear();

        //♠♥♦♣
        char[] type = new char[4];
        type[0] = '♠';
        type[1] = '♥';
        type[2] = '♦';
        type[3] = '♣';

        //Create 52 cards
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 13; j++)
            {
                int num;
                string cName = "";
                //Card number (or amount)
                if (j == 1)
                {
                    num = j;
                    cName = "a";
                }
                else if (j > 10)
                {
                    num = 10;
                    switch (j)
                    {
                    case 11:
                        cName = "j";
                        break;
                    case 12:
                        cName = "q";
                        break;
                    case 13:
                        cName = "k";
                        break;
                    }
                }
                else
                {
                    num = j;
                    cName = j.ToString();
                }

                //Create card
                Card newcard = new Card()
                {
                    name = cName,
                    type = type[i],
                    number = num
                };
                Cards.Add(newcard);
            }
        }

        //Shuffle;

        for (int i = 0; i < Cards.Count; i++)
        {
            Card temp = Cards[i];
            int randomIndex = Random.Range(i, Cards.Count);
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
    }

    //////////////////////////////
    // Pull a card
    public void PullACard(string who)
    {
        switch (who)
        {
            case "Player":
                currentCard = PlayerCards[_PlayerCard];

                //Check for Ace
                if (_Plpoints + Cards[_TopCard].number == 11)
                {
                    Debug.Log("ACE");
                    int i = _PlayerCard;
                    while (-1 < i)
                    {
                        if (PlayerCards[i].GetComponent<CardUI>().Ctext.text == "1")
                        {
                            Debug.Log(PlayerCards[i].GetComponent<CardUI>().Ctext.text);
                            _Plpoints = +11;
                            i = 0;
                        }
                        i--;
                    }
                }

                _Plpoints += Cards[_TopCard].number;
                totalPlayer.GetComponent<Text>().text = (_Plpoints).ToString();

                _PlayerCard += 1;
                if (_PlayerCard == 5)
                    btnHit.enabled = false;
                break;
            case "Opponent":
                currentCard = OpCards[_OppCard];

                //Check for Ace
                if (_OpPoints + Cards[_TopCard].number == 11)
                {
                    Debug.Log("ACE");
                    int i = _OppCard;
                    while (-1 < i)
                    {
                        if (OpCards[i].GetComponent<CardUI>().Ctext.text == "1")
                        {
                            Debug.Log(OpCards[i].GetComponent<CardUI>().Ctext.text);
                            _OpPoints = +11;
                            i = 0;
                        }
                        i--;
                    }
                }

                _OpPoints += Cards[_TopCard].number;
                totalOP.GetComponent<Text>().text = (_OpPoints).ToString(); ;

                _OppCard += 1;
                break;
        }
        currentCard.SetActive(true);
        currentCard.GetComponent<CardUI>().Ctext.text = Cards[_TopCard].name;
        currentCard.GetComponent<CardUI>().Csymbol.text = Cards[_TopCard].type.ToString();
        Debug.Log(_TopCard);
        _TopCard += 1;
    }
}
