using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtSummon : ugtState
{
    public ugtSummon(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator){ }

    public override void Enter()
    {
        Debug.Log("Entering Summon State");
        animator.SetTrigger(animatorStrings.summon);
        // Attack setup code

    }

    public override void Execute()
    {
        // Attack behavior code

        // Example transition back to IdleState
        
    }

    public override void Exit()
    {
        Debug.Log("Exiting Summon State");
        // Cleanup code for attack state
    }

}
