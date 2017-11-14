using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool controlled;
    public int STR, DEX, LCK;
    public int HP;
    public int Money, Success, Fame;


	// Use this for initialization
	void Start () {
        //Initial stats/score for everyone
        //STR = 3;
        //DEX = 3;
        //LCK = 3;
        HP = 20;
        Money = 100;
        Success = 0;
        Fame = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // ---------- ---------- ---------- ---------- 
    // PLAYER PROFILE
    private void Profile()
    {

    }

}
