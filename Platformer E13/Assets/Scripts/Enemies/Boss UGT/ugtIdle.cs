using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtIdle : ugtState
{
    public ugtIdle(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        // Attack setup code
        // Face the player
        boss.FacePlayer();
 
    }


    public override void Execute()
    {
        
    }


    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
        // Cleanup code for attack state
    }
}
