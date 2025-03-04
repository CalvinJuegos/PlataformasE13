using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtSummon : ugtState
{
    public ugtSummon(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator){ }

    public override void Enter()
    {
        Debug.Log("Entering Summon State");
        
        // Attack setup code

    }

    public override void Execute()
    {
        animator.SetTrigger(animatorStrings.summon);
        // Attack behavior code
        Debug.Log("Attaque summon");
        // Example transition back to IdleState
        // Possibly if (RNG) closeUp attack
        boss.ChangeState(new ugtAgro(boss, player, animator));
        animator.SetTrigger(animatorStrings.agroTrigger);

    }

    public override void Exit()
    {
        Debug.Log("Exiting Summon State");
        // Cleanup code for attack state
    }

}
