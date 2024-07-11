using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileSpeed = 100f;
    public float spawnRadius = 2.0f;
    public float outwardSpeed = 5.0f;
    public float rotationSpeed = 50.0f;
    public float projectileLifespan = 5.0f;

    public void spawnProjectiles()
    {
        /// Directions in which to spawn projectiles
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        // Determine the spawn position
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;

        foreach (Vector2 direction in directions)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Projectile instantiated at: " + spawnPosition);

            // Initialize the projectile direction and speed
            projectileControl projScript = projectile.GetComponent<projectileControl>();
            if (projScript != null)
            {
                projScript.InitializeDirection(direction);
                projScript.projSpeed = projectileSpeed;
                Debug.Log("Projectile direction initialized: " + direction);
            }
            else
            {
                Debug.LogError("No projectileControl component found on the projectile prefab.");
            }
        }
    }

    /*
    public void spawnProjectiles()
    {
        // Directions in which to spawn projectiles
        Debug.Log("Spawnea");  
        Vector2[] directions = new Vector2[]
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
        };

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;

        foreach (Vector2 direction in directions)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the Rigidbody2D component to apply velocity
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Projectile set");
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
    */
}
