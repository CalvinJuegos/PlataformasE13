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
    private float lifeTimer;

    public boss3Behaviour Boss3Behaviour;

    void Start()
    {
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
            }

            Destroy(gameObject);
            return;
        }

        // Calculate new position
        angle += rotationSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * outwardSpeed * Time.deltaTime;
        transform.position += offset;

        // Move outward
        transform.position += (transform.position - rotationCenter).normalized * outwardSpeed * Time.deltaTime;
    }
}
