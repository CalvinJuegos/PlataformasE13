using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseIdle : roseState, ICollisionHandler
{
    public roseIdle(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        // Attack setup code
        boss.FacePlayer();

    }

    public override void Execute()
    {
        // Attack code here
        Debug.Log(" Idle State");

        // Set path for attack, thrust, dodge or transform
        //boss.ChooseAttack(boss.playerDistance());


    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
        // Cleanup code for attack state
    }

    public void HandleCollision(Collider2D other)
    {
        Debug.Log("Collision entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("!");
            boss.ChooseAttack(boss.playerDistance());
            //animator.SetBool(animatorStrings.agroRange, true);
            //boss.ChangeState(new ugtAgro(boss, player, animator));

        }
    }
}
