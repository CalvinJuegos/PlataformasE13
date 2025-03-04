using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roseDrop : roseState
{
    
    public roseDrop(bossRoseBehaviour boss, GameObject player, Animator animator) : base(boss, player, animator) {
        rb = boss.GetComponent<Rigidbody2D>();
        collider = boss.GetComponent<CapsuleCollider2D>();
     }

    private bossRoseBehaviour stateMachine;

    private float hoverDuration = 3f;
    private float dropSpeed = 10f;
    public float hoverHeight = 9f;
    private float dropYPosition = -4.320768f;
    private float elapsedTime = 0f;
    private bool isHovering = true;
    private Vector2 dropPosition;
    private Rigidbody2D rb;
    private CapsuleCollider2D collider;

    private bool coroutineStarted = false;

    public override void Enter()
    {
        Debug.Log("Entering Drop State");
        collider.enabled = false;
        elapsedTime = 0f;
        isHovering = true;
    }

    public override void Execute()
    {
        if (isHovering)
        {
            Hover();
        }
        else
        {
            DropToPosition();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Drop State");
        collider.enabled = true;
        boss.hasTransformed = true;
    }

    private void Hover()
    {
        if (elapsedTime < hoverDuration)
        {
            Vector2 hoverPosition = new Vector2(player.transform.position.x, player.transform.position.y + hoverHeight);
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, hoverPosition, dropSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
        }
        else
        {
            Debug.Log("Hover ended");
            isHovering = false;
            animator.SetTrigger(animatorStrings.boom);
             // Calculate initial hover position
            float hoverOffset = 6.0f;
            float randomXOffset = Random.Range(-hoverOffset, hoverOffset);
            dropPosition = new Vector2(boss.transform.position.x, dropYPosition);
        }
    }

    private void DropToPosition()
    {
        Debug.Log("Dropping to position");
        if (Vector2.Distance(boss.transform.position, dropPosition) > 0.1f)
        {
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, dropPosition, dropSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Drop ended.");
            // Transition to the next state if needed
            boss.ChooseAttack(boss.playerDistance());
        }
    }

    /*

    private void Hover()
    {
        // Hover above the player
        float elapsedTime = 0f;
        float hoverOffset = 5.0f; // Adjust this value to set the offset from the player
        float randomXOffset = Random.Range(-hoverOffset, hoverOffset);

        animator.SetTrigger(animatorStrings.boom);

        while (elapsedTime < hoverDuration)
        {
            // Update hover position with a slight delay and offset from the player
            Vector2 hoverPosition = new Vector2(player.transform.position.x + randomXOffset, player.transform.position.y + hoverHeight);
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, hoverPosition, dropSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            //yield return null;
        }

        Debug.Log("Hover ended");

        // Determine drop position after hover
        float finalXPosition = boss.transform.position.x;
        float finalYPosition = boss.transform.position.y;
        //boss.StartCoroutine(DropToPosition(finalXPosition, dropYPosition));
        DropToPosition(finalXPosition, dropYPosition);
    }

    private void DropToPosition(float targetX, float targetY)
    {
        boss.StopAllCoroutines();
        Debug.Log("Target X: " + targetX + " Target Y: " + targetY);
        
        Vector2 dropPosition = new Vector2(targetX, targetY);
        animator.SetTrigger(animatorStrings.drop);

        while (Vector2.Distance(boss.transform.position, dropPosition) > 0.1f)
        {
            float step = dropSpeed * Time.deltaTime;
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, dropPosition, step);
            Debug.Log("Droping to position");
            //yield return null;
        }

        //boss.transform.position = dropPosition;
        //boss.transform.position = new Vector2(boss.transform.position.x, dropYPosition);
        
        rb.velocity = Vector2.zero;
        Debug.Log("Drop ended .");
    }

    private IEnumerator Drop(float finalXPosition)
    {
        Vector2 dropPosition = new Vector2(finalXPosition, dropYPosition);

        yield return new WaitForSeconds(0.5f); // Small delay before dropping

        // Drop on the player's position
        animator.SetTrigger(animatorStrings.boom);

        while (Vector2.Distance(boss.transform.position, dropPosition) > 0.1f)
        {
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, dropPosition, dropSpeed * Time.deltaTime);
            yield return null;
        }

        boss.transform.position = dropPosition;
        rb.velocity = Vector2.zero;

        Debug.Log("Drop ended!!!!!");

        yield return new WaitForSeconds(5.00f);

        boss.ChangeState(new roseIdle(boss, player, animator));
    }
    */
}
