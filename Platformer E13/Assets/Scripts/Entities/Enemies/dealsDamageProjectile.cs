using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dealsDamageProjectile : MonoBehaviour
{
    //public Collider2D collider;
    //private Animator animator;
    public float damageDealt;

    private void Awake()
    {
        //collider = GetComponent<Collider2D>();
        //animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Entered collider");
            // Check if the other collider has the PlayerHealth component
            playerHealth player = collision.GetComponent<playerHealth>();
            
            dealDamage(damageDealt,player);
            Destroy(gameObject);
        }
    }

    public void dealDamage(float damage,playerHealth player)
    {
        Debug.Log("Hit for"+damage);
        player.Hit(damage);
    }
}