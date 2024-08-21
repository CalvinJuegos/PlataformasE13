using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseDrop : roseState
{
    public roseDrop(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
        // Attack setup code
        boss.FacePlayer();

    }

    public override void Execute()
    {
        // Attack code here
        animator.SetTrigger(animatorStrings.meleeHit);
        // Set path for attack, thrust, dodge or transform

    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        // Cleanup code for attack state
    }
}
