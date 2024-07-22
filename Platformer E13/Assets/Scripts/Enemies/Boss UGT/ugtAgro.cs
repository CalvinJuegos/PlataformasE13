using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ugtAgro : ugtState
{
    public ugtAgro(bossUGTbehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private float ugtMoveSpeed = 5.0f;
    private float distanceToAttack = 5.0f;

    public override void Enter()
    {
        Debug.Log("Entering Agro State");
        // Attack setup code
        
    }

    public override void Execute()
    {
        // Follow the player
        if (player != null)
        {
            // Face the player
            boss.FacePlayer();

            // Calculate horizontal direction
            Vector3 direction = (player.transform.position - boss.transform.position).normalized;
            direction.y = 0; // Ignore vertical movement

            // Move towards the player in horizontal direction only
            if (boss.CanMove)
            {
                boss.transform.position += direction * ugtMoveSpeed * Time.deltaTime;
            }         
        }

        // Example transition to AttackState
        if (Vector3.Distance(boss.transform.position, player.transform.position) < distanceToAttack)
        {
            //boss.InRangeForMelee = true;
            boss.ChangeState(new ugtAttack(boss, player, animator));
        } else
        {
            boss.ChangeState(new ugtAgro(boss, player, animator));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Agro State");
        // Cleanup code for attack state
    }
}
