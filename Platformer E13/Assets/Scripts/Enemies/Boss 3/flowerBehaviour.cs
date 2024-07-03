using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnRadius = 2.0f;
    public float outwardSpeed = 5.0f;
    public float rotationSpeed = 50.0f;
    public float projectileLifespan = 5.0f;

    public void spawnProjectiles()
    {
        Debug.Log("Spawnea");
        for (int i = 0; i < 4; i++)
        {

            float angle = i * Mathf.PI / 2; // 0, 90, 180, 270 degrees in radians
            // Calculate spawn position around the spawner
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position + spawnPosition, Quaternion.identity);

            projectileControlv2 movement = projectile.AddComponent<projectileControlv2>();
            movement.outwardSpeed = outwardSpeed;
            movement.rotationSpeed = rotationSpeed;
            movement.lifespan = projectileLifespan;

        }
    }
}
