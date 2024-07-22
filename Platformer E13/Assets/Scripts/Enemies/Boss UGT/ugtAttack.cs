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


        // Follow up not working!!!
        /*
        // Condition for transition here, may repeat attack and do attack followUp
        float distanceToPlayer = Vector3.Distance(boss.transform.position, player.transform.position);
        bool inRangeForMelee = distanceToPlayer < distanceToAttack;

        if (inRangeForMelee)
        {
            // Trigger follow-up attack
            animator.SetTrigger(animatorStrings.followUp);
        }
        */
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        // Cleanup code for attack state
    }

}
