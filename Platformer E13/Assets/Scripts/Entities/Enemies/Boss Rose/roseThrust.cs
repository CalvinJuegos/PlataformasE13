    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseThrust : roseState
{
    public roseThrust(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) { }

    private bossRoseBehaviour stateMachine;
    private Rigidbody2D rb;

    public float advanceDistance = 7.0f;
    public float advanceSpeed = 5.0f;
    float unitsPast = 3f; // Define how many units to move past the player

    public override void Enter()
    {
        Debug.Log("Entering Thrust State");
        // Attack setup code
        //boss.FacePlayer();
        rb = boss.GetComponent<Rigidbody2D>();
        animator.SetTrigger(animatorStrings.thrustAttack);

    }

    public override void Execute()
    {
        // Attack code here
        if (boss.preparedToThrust)
        {
            Debug.Log("Thrusting");
            boss.preparedToThrust = false;
            AdvanceTowardsPlayer();
            
        }
        
        // Set path for attack, thrust, dodge or transform
        if (boss.thrustCooldown)
        {
            boss.thrustCooldown = false;
            boss.ChooseAttack(boss.playerDistance());
        }

    }

    public override void Exit()
    {
        Debug.Log("Exiting Thrust State");
        // Cleanup code for attack state
        animator.ResetTrigger(animatorStrings.thrustAttack);
    }

    private void AdvanceTowardsPlayer()
    {
        Debug.Log("Advancing towards player");
        float fixedDistance = 10f; // Define the fixed distance to move
        float moveSpeed = 5f; // Define the speed of movement

        // Determine direction based on x-axis
        float direction = Mathf.Sign(player.transform.position.x - boss.transform.position.x);

        // Calculate the target x position
        float targetX = boss.transform.position.x + direction * fixedDistance;

        // Move the boss towards the target position along the x-axis
        Vector2 targetPosition = new Vector2(targetX, boss.transform.position.y);
        
        // Use Rigidbody2D for movement
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime));

        Debug.Log("Advanced towards player");
    }
}
