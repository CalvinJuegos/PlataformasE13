using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtFollowUp : ugtState
{
    public ugtFollowUp(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    public override void Enter()
    {
        Debug.Log("Entering Follow Up State");
        // Attack setup code
        
    }

    public override void Execute()
    {
        animator.SetTrigger(animatorStrings.followUp);
        // Ataque

        boss.ChangeState(new ugtAgro(boss, player, animator));
    }

    public override void Exit()
    {
        Debug.Log("Exiting Follow Up State");
        // Cleanup code for attack state
    }
}
