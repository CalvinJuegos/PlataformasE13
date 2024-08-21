using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseAttack : roseState
{
    public roseAttack(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private float distanceToAttack = 5.0f;

    private bossRoseBehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
        // Attack setup code

    }

    public override void Execute()
    {
        // Attack code here
        animator.SetTrigger(animatorStrings.meleeHit);
        // Attack logic here
        //boss.ChangeState()


    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        // Cleanup code for attack state
    }
}
