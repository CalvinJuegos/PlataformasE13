using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseThrust : roseState
{
    public roseThrust(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;
    private Rigidbody rb;

    public override void Enter()
    {
        Debug.Log("Entering Thrust State");
        // Attack setup code
        boss.FacePlayer();

    }

    public override void Execute()
    {
        // Attack code here
        animator.SetTrigger(animatorStrings.thrustAttack);
        // Set path for attack, thrust, dodge or transform

    }

    public override void Exit()
    {
        Debug.Log("Exiting Thrust State");
        // Cleanup code for attack state
        animator.ResetTrigger(animatorStrings.thrustAttack);
    }

}
