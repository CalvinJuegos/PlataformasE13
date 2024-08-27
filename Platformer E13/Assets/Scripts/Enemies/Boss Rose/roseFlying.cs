using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseFlying : roseState
{

    private CapsuleCollider2D collider;
    public roseFlying(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) {

        collider = boss.GetComponent<CapsuleCollider2D>();
        
     }

    private bossRoseBehaviour stateMachine;

    public float moveSpeed = 10f;
    public float leftBound = -10f;
    public float rightBound = 15f;
    public float moveDuration = 4f;

    private float elapsedTime = 0f;
    private bool movingRight = true;
    private bool movementStarted = false;

    public override void Enter()
    {
        Debug.Log("Entering Flying State");
        boss.FacePlayer();
        collider.enabled = false;
        boss.StartCoroutine(MoveHorizontally());
    }

    public override void Execute()
    {
        Debug.Log("Elapsed Time: " + elapsedTime);
        Debug.Log("Move Duration: " + moveDuration);
        // Only check for state change
        if (elapsedTime >= moveDuration)
        {
            boss.ChangeState(new roseDrop(boss, player, animator));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Flying State");
        collider.enabled = true;
    }

    private IEnumerator MoveHorizontally()
    {
        while (elapsedTime < moveDuration)
        {
            float moveDirection = movingRight ? 1f : -1f;
            boss.transform.Translate(Vector2.right * moveDirection * moveSpeed * Time.deltaTime);

            if (boss.transform.position.x >= rightBound)
                movingRight = false;
            else if (boss.transform.position.x <= leftBound)
                movingRight = true;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Here coroutine instantiating projectiles

    // private IEnumerator EnolaGay()
    //{
        //Intstantiating three projectiles with boss.projectilePrefab
    //}
}
