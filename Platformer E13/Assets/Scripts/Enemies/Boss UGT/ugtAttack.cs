using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtAttack : ugtState
{
    public ugtAttack(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private float distanceToAttack = 5.0f;

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

    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        // Cleanup code for attack state
    }

}
