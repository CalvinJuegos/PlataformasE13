using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtJump : ugtState
{
    public ugtJump(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private float distanceToAttack = 5.0f;

    private bossUGTbehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering Jump State");
        // Attack setup code
        boss.FacePlayer();
        
    }

    public override void Execute()
    {
        // Attack code here
        //animator.SetTrigger();
        // Attack logic here
        // boss.setAttack

    }

    public override void Exit()
    {
        boss.FacePlayer();
        Debug.Log("Exiting Jump State");
        // Cleanup code for attack state
    }

}