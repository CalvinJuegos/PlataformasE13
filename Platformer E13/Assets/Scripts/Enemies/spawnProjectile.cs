    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnProjectile : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject projectilePrefab;

    public Vector2 spawnPosition;
    public float fireRate;
    public float nextFireTime;

    void Update()
    {
        // !! Add condition if attack is ongoing 
        if (Time.time >= nextFireTime)
        { 
            // !! Spawn position is SET HERE (Can set different types of projectiles to different places with function)
            spawnPosition = transform.position;       
            FireProjectile(spawnPosition);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public void FireProjectile(Vector2 spawn)
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector2 direction = playerTransform.position - transform.position;
            direction.Normalize();

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, spawn, Quaternion.identity);

            // Initialize the projectile direction
            projectileControl projScript = projectile.GetComponent<projectileControl>();
            if (projScript != null)
            {
                projScript.InitializeDirection(direction);
            }
        }
    }

    public void setProjSpawn()
    {
        // For other projectiles spawn
        // Create different empty objects to get their coordinates and so they can spawn different types depending on the attacks
    }

}
