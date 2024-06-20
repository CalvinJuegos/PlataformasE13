using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileControl : MonoBehaviour
{
    // Given parameters generate Collider with script dealsDamage

    private Animator animator;

    private dealsDamage DealsDamage;

    // For other possible future types of projectiles
    //public int typeOf;

    public Vector2 startingPosition;
    public Vector2 targetDirection;
    public float projSpeed;
    public float projDamage;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void InitializeDirection(Vector2 direction)
    {
        targetDirection = direction.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Here place IF for other possible types and different functions depending on type
        projMovement();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Entered collider, dealing "+projDamage);

            playerHealth player = collision.GetComponent<playerHealth>();
            dealsDamage DealsDamage = GetComponent<dealsDamage>();

            if (DealsDamage != null) // HERE IS THE ERROR
            {
                DealsDamage.dealDamage(projDamage, player);
            }
            else { Debug.Log("Errro"); }
            Destroy(gameObject);

            
        }
    }

    private void projMovement()
    {
        transform.Translate(targetDirection * projSpeed * Time.deltaTime);
    }
}
