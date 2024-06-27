using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3Behaviour : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    public GameObject projectilePrefab;

    public Collider2D colliderContactDmg;
    public Collider2D colliderBackOff;
    public Collider2D colliderCover;

    public bool finishedMainAttack = true;
    public int takenHits;

    private damagable Damagable;
    private detectionPushRange DetectionPushRange;
    private playerControl PlayerControl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ChooseAttack()
    {
        Debug.Log("Elegir ataque");
        if (PlayerControl.IsAlive && !Damagable.IsStunned) 
        {
            
            System.Random rng = new System.Random();
            int randomValue = rng.Next(100); // Random number between 0 and 99

            if (DetectionPushRange.InRangeForPush)
            {
                // While in range it can do:
                // High chance: backOff, flowerAttack, lightBeam
                // Medium chance: immuneBackOff, lightFlash

                if (randomValue < 50) // 50% chance
                {
                    // High chance attacks
                    int highChanceRandom = rng.Next(3);
                    if (highChanceRandom == 0)
                    {
                        BackOff();
                    }
                    else if (highChanceRandom == 1)
                    {
                        FlowerAttack();
                    }
                    else if (highChanceRandom == 2)
                    {
                        LightBeam();
                    }
                }
                else // 50% chance
                {
                    // Medium chance attacks
                    int mediumChanceRandom = rng.Next(2);
                    if (mediumChanceRandom == 0)
                    {
                        ImmuneBackOff();
                    }
                    else if (mediumChanceRandom == 1)
                    {
                        LightFlash();
                    }
                }
            } 
            else 
            {
                // While not in range:
                // High chance: flowerAttack, lightBeam
                // Medium chance: immuneBackOff, lightFlash, backOff

                if (randomValue < 50) // 50% chance
                {
                    // High chance attacks
                    int highChanceRandom = rng.Next(2);
                    if (highChanceRandom == 0)
                    {
                        FlowerAttack();
                    }
                    else if (highChanceRandom == 1)
                    {
                        LightBeam();
                    }
                }
                else // 50% chance
                {
                    // Medium chance attacks
                    int mediumChanceRandom = rng.Next(3);
                    if (mediumChanceRandom == 0)
                    {
                        ImmuneBackOff();
                    }
                    else if (mediumChanceRandom == 1)
                    {
                        LightFlash();
                    }
                    else if (mediumChanceRandom == 2)
                    {
                        BackOff();
                    }
                }
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedMainAttack)
        {
            // CHooses main attack depending on player position
            ChooseAttack();
            
        }

        // For continuos attacks (Fire rate phase 1 low, fire rate phase 2 higher)
        if (Time.time >= nextFireTime)
        { 
            // !! Spawn position is SET HERE (Can set different types of projectiles to different places with function)
            spawnPosition = transform.position;       
            FireProjectile(spawnPosition);
            nextFireTime = Time.time + 1f / fireRate;
        }        

    }

    // If boss has taken too many hits of the player the more chance it has of doing the attack
    // If player is range, can do the attack
    // Taken HiTS

    IEnumerator BackOff() 
    {
        Debug.Log("BackOff");
        //Pushes back player and deals medium damage
        yield return null;
    }

    // Attack that triggers regularly, most common, main damage
    IEnumerator FlowerAttack()
    {
        Debug.Log("flowerAttack");
        // Creates flower objects that explode in a certain time
        // Deals high damage
        yield return null;
    }

    // Deals the most damage
    // Forces player to hide behind walls (if not in trigger collider > take damage)
    // Rare attack except is done whenever at 50% life
    IEnumerator LightFlash()
    {
        Debug.Log("lightFlash");
        yield return null;
    }

    IEnumerator LightBeam()
    {
        Debug.Log("LightBeam");
        // Activates the objectsColliders with dealsDamage and triggers their lightning animation
        // Randomness could choose which one to activate.
        
        yield return null;
    }

    IEnumerator ImmuneBackOff()
    {
        Debug.Log("ImmuneBackoff");
        // Animator shows the immunity
        // Calls for backOff three times
        //Prevents boss from taking damage and preforms 3 backOffs in succession
        
        yield return null;
    }

    public Vector2 spawnPosition;
    public float fireRate;
    public float nextFireTime;

    IEnumerator FireProjectile(Vector2 spawn)
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
        yield return null;
    }
}
