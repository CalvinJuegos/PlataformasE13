using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dealsDamageSpike : MonoBehaviour
{
    //public Collider2D collider;
    //private Animator animator;
    public float damageDealt;
    public Transform playerTransform; // Reference to the player's Transform
    public Transform checkpoint; // The position to teleport the player to


    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Debug.Log("Entered collider");
            // Check if the other collider has the PlayerHealth component
            playerHealth player = collision.GetComponent<playerHealth>();
            
            dealDamage(damageDealt,player);
            ReloadScene();
        }
    }

    public void dealDamage(float damage,playerHealth player)
    {
        Debug.Log("Hit for"+damage);
        player.Hit(damage);
    }

    public void ReloadScene()
    {
        // Teleport the player to the checkpoint position
        if (playerTransform != null && checkpoint != null)
        {
            playerTransform.position = checkpoint.position; // Use the checkpoint's position
            Debug.Log("Teleported to checkpoint at " + checkpoint.position);
        }
        else
        {
            Debug.LogWarning("Player transform or checkpoint not set!");
        }
    }
}