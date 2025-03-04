using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseDodge : roseState
{
    public roseDodge(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) 
    {
        collider = boss.GetComponent<CapsuleCollider2D>();
    }
    
    private CapsuleCollider2D collider;

    private bossRoseBehaviour stateMachine;
    public bool dodgeStarted = false;

    public override void Enter()
    {
        Debug.Log("Entering Dodge State");
        // Attack setup code
        boss.FacePlayer();
        animator.SetTrigger(animatorStrings.dodge);
        collider.enabled = false;

    }

    public override void Execute()
    {
        // Attack code here
        if (!dodgeStarted)
        {
            dodgeStarted = true;
            Dodge();  
        }
        
        // Set path for attack, thrust, dodge or transform
        

    }

    public override void Exit()
    {
        Debug.Log("Exiting Dodge State");
        // Cleanup code for attack state
        animator.ResetTrigger(animatorStrings.dodge);
        collider.enabled = true;
    }

    public void Dodge()
    {
        

        boss.ChooseAttack(boss.playerDistance());
    }

}
