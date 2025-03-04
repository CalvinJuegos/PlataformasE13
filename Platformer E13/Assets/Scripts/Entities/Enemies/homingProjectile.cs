using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingProjectile : MonoBehaviour
{
    public float initialForce = 7f; // Initial force applied when spawned
    public float homingSpeed = 200f;   // Speed at which the projectile homes in on the player
    public float lifeTime = 5f;      // Time after which the projectile self-destructs
    private Animator animator;

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        // Get component for animator
        animator = GetComponent<Animator>();

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Find the player by tag or reference it directly
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Apply the initial force
        rb.AddForce(transform.up * initialForce, ForceMode2D.Impulse);

        // Destroy the projectile after a certain time
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Debug.Log("PROYECTIIIIL");
        //This doesn't seem to be working
        // Calculate direction towards the player
        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly interpolate the current angle towards the target angle
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, homingSpeed * Time.fixedDeltaTime);

        // Apply the rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Move the projectile forward
        rb.velocity = transform.right * homingSpeed * Time.fixedDeltaTime;
    }

    void OnTrigger2DEnter(Collider other)
    {
        // Optionally, destroy the projectile upon collision with something
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {
            StartCoroutine(destroyProjectile());
        }
    }

    IEnumerator destroyProjectile()
    {
        Debug.Log("Destroying projectile");
        animator.SetTrigger(animatorStrings.playerHit); // Trigger the "playerHit" animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for the animation to finish playing
        Debug.Log("Projectile is destroying");
        Destroy(gameObject); // Destroy the projectile
    }
}