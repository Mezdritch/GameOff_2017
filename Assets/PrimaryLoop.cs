using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimaryLoop : MonoBehaviour
{

    //Init, cycle, worldmap, fight,, dance, chance
    private IEnumerable state;
    public string stateP;

    //Stats & UI
    private Transform cyclesUI;
    private int cycles;

    //Players
    public GameObject Eli, Nina, Riviera, Blue;
    public string theControlledPlayer;

    //////////////////////////////
    // Start
    void Start()
    {
        //Start with the Init state
        state = Init();

        //Start state machine
        StartCoroutine(RunStateMachine());
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
    // INIT
    private IEnumerable Init()
    {
        Debug.Log(state);

        //UI

        //Get Players
        Eli = GameObject.Find("Player_Eli");
        Nina = GameObject.Find("Player_Nina");
        Riviera = GameObject.Find("Player_Riviera");
        Blue = GameObject.Find("Player_Blue");
        
        //Who's controlled?
        if (Eli.GetComponent<Player>().controlled)
            theControlledPlayer = "Eli";
        else if (Nina.GetComponent<Player>().controlled)
            theControlledPlayer = "Nina";
        else if (Riviera.GetComponent<Player>().controlled)
            theControlledPlayer = "Riviera";
        else
            theControlledPlayer = "Blue";

        yield return new WaitForSeconds(2);
        state = worldmap();
    }

    //////////////////////////////
    // WORLDMAP
    private IEnumerable worldmap()
    {

        yield return null;
    }

    //////////////////////////////
    // END
    private IEnumerable End()
    {
        yield return new WaitForSeconds(2);
        this.GetComponentInParent<Menu>().playersInfo();
        yield return new WaitForSeconds(1);
        this.GetComponentInParent<Menu>().GameEndPanel.SetActive(true);
        yield return new WaitForSeconds(6);
        Application.Quit();
        yield return null;
    }

    //////////////////////////////
    // GameOver
    public string GameOver()
    {
        string name = null;

        if (DoesThePlayerWin(Eli))
            name = "Eli";
        if (DoesThePlayerWin(Nina))
            name = "Nina";
        if (DoesThePlayerWin(Riviera))
            name = "Riviera";
        if (DoesThePlayerWin(Blue))
            name = "Blue";

        if(name != null)
            state = End();

        return name;
    }

    //////////////////////////////
    // GameOver?
    public bool DoesThePlayerWin( GameObject player)
    {
        bool won = false;

        if (player.GetComponent<Player>().Money > 1999)
            won = true;
        else if (player.GetComponent<Player>().Success > 1999)
            won = true;
        else if (player.GetComponent<Player>().Fame > 7)
            won = true;

        return won;
    }

}
