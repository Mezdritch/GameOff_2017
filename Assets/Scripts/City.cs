using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class City : MonoBehaviour
{
    //City
    public List<Area> Bars, Casinos, Clubs, Discos, Hotel, Industrial;
    private string who;
    private int _RandomArea;

    //AIs
    private string controlledP;
    public bool pEli, pNina, pRiviera, pBlue = false;
    public GameObject Eli, Nina, Riviera, Blue;

    private int whichType, which;   //use to determine the area (random int)
    private Area _Area;

    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    ///////// City    ////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////
    // Use this for initialization
    void Start()
    {
        //Who is controlled by the player?
        controlledP = this.GetComponentInParent<PrimaryLoop>().theControlledPlayer;

        if (controlledP != "Eli")
            pEli = true;
        if (controlledP != "Nina")
            pNina = true;
        if (controlledP != "Riviera")
            pRiviera = true;
        if (controlledP != "Blue")
            pBlue = true;

        //The players
        //Get Players
        Eli = GameObject.Find("Player_Eli");
        Nina = GameObject.Find("Player_Nina");
        Riviera = GameObject.Find("Player_Riviera");
        Blue = GameObject.Find("Player_Blue");
    }

    ////////////////////////////////////////
    //GRAB RANDOM AREA
    public void GrabRandomArea(List<Area> type)
    {
        //
        for (int i = 0; i < type.Count; i++)
        {
            who = type[i].GetComponent<Area>().Player;
            Debug.Log(who);
        }
    }

    ////////////////////////////////////////
    // RANDOM AREA
    public int RandomArea(List<Area> type)
    {
        Debug.Log("type.Count: " + type.Count);
        _RandomArea = Random.Range(0, type.Count);
        Debug.Log("_RandomArea: "+ _RandomArea);
        return _RandomArea;
    }

    //////////////////////////////
    // GET an Area
    public bool GetTheArea(Area area, string ai, int difficulty, int aiSkill)
    {
        bool success;
        //Does the AI succeed?
        if (AIsuccess(difficulty, aiSkill))
        {
            this.GetComponentInParent<Menu>().aiWinArea(area, ai);       //He gets the area
            success = true;
        }
        else
        {
            this.GetComponentInParent<Menu>().aiLoseArea(area, ai);       //Message
            success = false;
        }
        return success;
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    ///////// AIs     ////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////
    //TURN EVENTS
    public void TurnEvents()
    {
        if (pEli)
            EliTurn();
        if (pNina)
            NinaTurn();
        if (pRiviera)
            RivieraTurn();
        if (pBlue)
            BlueTurn();
    }

    //////////////////////////////
    // Eli
    //////////////////////////////
    // Grabs ANY Area
    //
    //
    void EliTurn()
    {
        //Bar   | 10    
        //Casino| 2
        //Club  | 7
        //Disco | 3
        //Hotel | 2
        //Indust| 1
        whichType = Random.Range(1, 25);
        which = 0;

        if (whichType < 11)     // <Bar>    ///
        {
            which = RandomArea(Bars);
            _Area = Bars[which];
        }
        else if (whichType < 13)// <Casino>  ///
        {
            which = RandomArea(Casinos);
            _Area = Casinos[which];
        }
        else if (whichType < 20)// <Club>  ///
        {
            which = RandomArea(Clubs);
            _Area = Clubs[which];
        }
        else if (whichType < 23)// <Disco>  ///
        {
            which = RandomArea(Discos);
            _Area = Discos[which];
        }
        else if (whichType < 25)// <Hotel>  ///
        {
            which = RandomArea(Hotel);
            _Area = Hotel[which];
        }
        else                    // <Industr>///
        {
            which = RandomArea(Industrial);
            _Area = Industrial[0];
        }

        if (GetTheArea(_Area, "Eli", 5, 4))
            getReward(Eli, _Area);
        else
            Eli.GetComponent<Player>().Money -= 10;
    }

    //////////////////////////////
    // Nina
    //////////////////////////////
    // Grabs: Clubs / Disco
    //
    //
    void NinaTurn()
    {
        //Clubs | 7   
        //Disco | 3
        whichType = Random.Range(1, 10);
        which = 0;

        if (whichType < 8)     // <Club>     ///
        {
            which = RandomArea(Bars);
            _Area = Bars[which];
        }
        else                    // <Disco>  ///
        {
            which = RandomArea(Discos);
            _Area = Discos[which];
        }

        if (GetTheArea(_Area, "Nina", 5, 4))
            getReward(Nina, _Area);
        else
            Nina.GetComponent<Player>().Money -= 10;
    }

    //////////////////////////////
    // Riviera
    //////////////////////////////
    // Grabs: Bars / Industrial / Disco
    //
    //
    void RivieraTurn()
    {
        //Bar   | 10    
        //Disco | 3
        //Indust| 1
        whichType = Random.Range(1, 14);
        which = 0;

        if (whichType < 11)     // <Bar>    ///
        {
            which = RandomArea(Bars);
            _Area = Bars[which];
        }
        else if (whichType < 14)// <Disco>  ///
        {
            which = RandomArea(Discos);
            _Area = Discos[which];
        }
        else                    // <Industr>///
        {
            which = RandomArea(Industrial);
            _Area = Industrial[0];
        }

        if (GetTheArea(_Area, "Riviera", 5, 4))
            getReward(Riviera, _Area);
        else
            Riviera.GetComponent<Player>().Money -= 10;
    }

    //////////////////////////////
    // Blue
    //////////////////////////////
    // Grabs: Bars / Casino / Hotel
    //
    //
    void BlueTurn()
    {
        //Bar   | 10    
        //Casino| 2
        //Hotel | 2
        whichType = Random.Range(1, 14);
        which = 0;

        if (whichType < 11)     // <Bar>    ///
        {
            which = RandomArea(Bars);
            _Area = Bars[which];
        }
        else if (whichType < 13)// <Casion>  ///
        {
            which = RandomArea(Discos);
            _Area = Discos[which];

        }
        else                    // <Hotel>///
        {
            which = RandomArea(Hotel);
            _Area = Hotel[which];
        }

        if (GetTheArea(_Area, "Blue", 5, 4))
            getReward(Blue, _Area);
        else
            Blue.GetComponent<Player>().Money -= 10;
    }

    //////////////////////////////

    void AItake(Area area, string ai)
    {

    }

    //////////////////////////////

    void getReward(GameObject player, Area area)
    {
        player.GetComponent<Player>().Money += area.Money;
        player.GetComponent<Player>().Success += area.Success;
        player.GetComponent<Player>().Fame += area.Fame;
    }

    //
    //AISUCCESS roll
    bool AIsuccess(int difficulty, int aiSkill)
    {
        bool success;
        int successRoll = Random.Range(1, difficulty);
        Debug.Log("Success Roll: " +successRoll + "|D: "+ difficulty + "|S: "+ aiSkill);
        if (successRoll < aiSkill)
            success = true;
        else
            success = false;

        return success;
    }
}
