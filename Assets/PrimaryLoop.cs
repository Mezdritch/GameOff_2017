using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimaryLoop : MonoBehaviour
{

    //Init, cycle,
    private IEnumerable state;

    //Stats & UI
    private Transform cyclesUI;
    private int cycles;


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
        SetupUi();

        yield return new WaitForSeconds(2);
        state = Cycle();
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
