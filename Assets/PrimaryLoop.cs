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
        //SetupUi();

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
    // CYCLE
    private IEnumerable Cycle()
    {
        Debug.Log(state);

        IncrementCycles();

        yield return new WaitForSeconds(10);
        state = Cycle();
    }

    //////////////////////////////
    // WORLDMAP
    private IEnumerable worldmap()
    {
        //Debug.Log(state);

        yield return null;
    }

    //////////////////////////////
    // FIGHT
    private IEnumerable fight()
    {
        //Debug.Log(state);

        yield return null;
    }



    //////////////////////////////
    // Setup UI
    private void SetupUi()
    {
        cyclesUI = GameObject.Find("Text_cycle_amount").transform;
    }

    //////////////////////////////
    // Increment Cycles
    private void IncrementCycles()
    {
        cycles += 1;
        cyclesUI.GetComponent<Text>().text = (cycles).ToString();
    }

}
