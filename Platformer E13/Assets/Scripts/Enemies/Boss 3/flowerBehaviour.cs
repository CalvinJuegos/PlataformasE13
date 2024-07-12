using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerBehaviour : MonoBehaviour
{
    public boss3Behaviour Boss3Behaviour;
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

                StartCoroutine(endAttack());
            }
            else
            {
                Debug.LogError("No projectileControl component found on the projectile prefab.");
            }
        }
    }

    IEnumerator endAttack()
    {
        Boss3Behaviour = Boss3Behaviour.GetComponent<boss3Behaviour>();
        Boss3Behaviour.SetFinishedMainAttack(true);
        yield return new WaitForSeconds(projectileLifespan);
    }

}
