using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseFollowUp : roseState
{
    public roseFollowUp(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering FollowUp State");
        // Attack setup code
        //boss.FacePlayer();

    }

    public override void Execute()
    {
        // Attack code here
        animator.SetTrigger(animatorStrings.followUp);
        // Set path for attack, thrust, dodge or transform
        boss.ChooseAttack(boss.playerDistance());


    }

    public override void Exit()
    {
        Debug.Log("Exiting FollowUp State");
        // Cleanup code for attack state
    }
}
