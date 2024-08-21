using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseTransform : roseState
{
    public roseTransform(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering Transform State");
        // Attack setup code
        boss.FacePlayer();

    }

    public override void Execute()
    {
        // Attack code here
        animator.SetTrigger(animatorStrings.transformStart);
        // Set path for attack, thrust, dodge or transform
        boss.ChangeState(new roseFlying(boss, player, animator));

    }

    public override void Exit()
    {
        Debug.Log("Exiting Transform State");
        // Cleanup code for attack state
    }
}
