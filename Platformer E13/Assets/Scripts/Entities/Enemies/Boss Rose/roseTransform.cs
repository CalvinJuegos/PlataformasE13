using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseTransform : roseState
{
    public roseTransform(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) {   }

    private bossRoseBehaviour stateMachine;

    public float moveSpeed = 5f;
    public float verticalDistance = 6f; // Distance to move upwards
    private bool animationFinished = false;
    private bool movementFinished = false;
    private Vector2 targetPosition;

    public override void Enter()
    {
        Debug.Log("Entering Transform State");
        // Attack setup code
        boss.FacePlayer();
        animator.SetTrigger(animatorStrings.transformStart);

        // Calculate the target position directly above the initial position
        targetPosition = new Vector2(boss.transform.position.x, boss.transform.position.y + verticalDistance);

    }

    public override void Execute()
    {
        MoveUpwards();

        Debug.Log("Animation Finished: " + boss.animationFinished);
        Debug.Log("Movement Finished: " + movementFinished);

        if (movementFinished && boss.animationFinished)
        {
            boss.ChangeState(new roseFlying(boss, player, animator));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Transform State");
        // Cleanup code for attack state
    }

    private void MoveUpwards()
    {
        if (!movementFinished)
        {
            if (Vector2.Distance(boss.transform.position, targetPosition) > 0.1f)
            {
                boss.transform.position = Vector2.MoveTowards(boss.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                boss.transform.position = targetPosition;
                movementFinished = true;
            }
        }
    }
}
