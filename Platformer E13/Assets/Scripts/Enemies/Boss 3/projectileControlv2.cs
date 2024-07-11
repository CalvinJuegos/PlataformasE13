using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileControlv2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float outwardSpeed;
    public float rotationSpeed;
    public float lifespan;

    private Vector3 rotationCenter;
    private float angle;
    private float lifeTimer = 4f;

    public boss3Behaviour Boss3Behaviour;

    public float speed = 5f; // Speed of the projectile
    public Vector2 direction; // Direction of the projectile movement

    void Start()
    {
        // Set the initial velocity of the projectile
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            Debug.LogError("No Rigidbody2D component found on the projectile.");
        }
    }

    void Update()
    {
        // Additional movement logic can be added here if needed
    }

    /*
    void Start()
    {

        Boss3Behaviour = GameObject.FindWithTag("Boss").GetComponent<boss3Behaviour>();
        rotationCenter = transform.position - transform.forward * outwardSpeed;
        angle = Vector3.SignedAngle(Vector3.right, transform.position - rotationCenter, Vector3.up);
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifespan)
        {
            
            if (Boss3Behaviour != null)
            {
                Boss3Behaviour.finishedMainAttack = true;
                Debug.Log("NExt Attack");
            }
            else
            {
                Debug.Log("Attack not ending");
            }

            Destroy(gameObject);
            Debug.Log("Lifespan over");
            return;
        }

        // Calculate new position
        angle += rotationSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * outwardSpeed * Time.deltaTime;
        transform.position += offset;

        // Move outward
        transform.position += (transform.position - rotationCenter).normalized * outwardSpeed * Time.deltaTime;
    }
    */
}
