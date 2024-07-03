using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3Behaviour : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    public GameObject projectilePrefab;

    //public Collider2D colliderContactDmg;
    //public Collider2D colliderBackOff;
    //public Collider2D colliderCover;

    public bool finishedMainAttack = true;
    public int takenHits;

    private damagable Damagable;
    private detectionPushRange DetectionPushRange;
    private playerControl PlayerControl;
    //private animatorStrings AnimatorStrings;

    private void Awake()
    {
        //animator = GetComponent<Animator>();

        PlayerControl = GameObject.FindWithTag("Player").GetComponent<playerControl>();       
        Damagable = GetComponent<damagable>();
        DetectionPushRange = GameObject.FindWithTag("Area").GetComponent<detectionPushRange>();
        

        attackAnim = GameObject.FindWithTag("attackAnimator").GetComponent<Animator>();
    }

    private void ChooseAttack()
    {
        if (Damagable == null)
        {
            Debug.LogError("Damagable is not initialized!");
            return;
        }

        if (PlayerControl== null){
            Debug.LogError("PlayerControl is not initialized!");
            return;
        }
        //bool isPlayerAlive = PlayerControl.IsAlive;
        //Debug.Log("Elegir ataque"+PlayerControl.IsAlive); // Error popping up HERE!!!
        if (PlayerControl.IsAlive && !Damagable.IsStunned)
        {
            Debug.Log("Eligiendo");
            System.Random rng = new System.Random();
            int randomValue = rng.Next(100); // Random number between 0 and 99

            if (DetectionPushRange.InRangeForPush)
            {
                Debug.Log("En rango");

                if (randomValue < 50) // 50% chance
                {
                    // High chance attacks
                    int highChanceRandom = rng.Next(3);
                    if (highChanceRandom == 0)
                    {
                        Debug.Log("Back Off");
                        StartCoroutine(BackOff());
                    }
                    else if (highChanceRandom == 1)
                    {
                        StartCoroutine(FlowerAttack());
                    }
                    else if (highChanceRandom == 2)
                    {
                        StartCoroutine(LightBeam());
                    }
                }
                else // 50% chance
                {
                    // Medium chance attacks
                    int mediumChanceRandom = rng.Next(2);
                    if (mediumChanceRandom == 0)
                    {
                        StartCoroutine(ImmuneBackOff());
                    }
                    else if (mediumChanceRandom == 1)
                    {
                        StartCoroutine(LightFlash());
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
                        StartCoroutine(FlowerAttack());
                    }
                    else if (highChanceRandom == 1)
                    {
                        StartCoroutine(LightBeam());
                    }
                }
                else // 50% chance
                {
                    // Medium chance attacks
                    int mediumChanceRandom = rng.Next(3);
                    if (mediumChanceRandom == 0)
                    {
                        StartCoroutine(ImmuneBackOff());
                    }
                    else if (mediumChanceRandom == 1)
                    {
                        StartCoroutine(LightFlash());
                    }
                    else if (mediumChanceRandom == 2)
                    {
                        StartCoroutine(BackOff());
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
    #region Back Off
    public Collider2D backOffArea;
    public Animator attackAnim;

    IEnumerator BackOff() 
    {
        Debug.Log("BackOff");
        finishedMainAttack = false;
        //Play animation
        attackAnim.SetTrigger(animatorStrings.backOff);

        yield return new WaitForEndOfFrame();

        AnimatorStateInfo stateInfo = attackAnim.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        while (attackAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && !attackAnim.IsInTransition(0))
        {
            yield return null;
        }

        // Damage handled in collider

        finishedMainAttack = true;
        Debug.Log("Finished attack");
    }

    public void EnableBackOffArea()
    {
        if (backOffArea != null)
        {
            backOffArea.enabled = true;
        }
    }

    public void DisableBackOffArea()
    {
        if (backOffArea != null)
        {
            backOffArea.enabled = false;
        }
    }

    #endregion

    // Attack that triggers regularly, most common, main damage

    public Animator attackFlower;
    public GameObject[] flowers;

    IEnumerator FlowerAttack()
    {
        Debug.Log("flowerAttack");
        finishedMainAttack = false;

        //GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        Debug.Log("Setting active"+flowers);

        foreach (GameObject flower in flowers) // This part does not execute
        {
            Debug.Log("Setting active");
            flower.SetActive(true);

            Animator animator = flower.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(animatorStrings.flowerStart);
                Debug.Log("animadorEmpieza");
            }
            else
            {
                Debug.LogError($"Animator component not found on GameObject: {flower.name}");
            }
        }

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
        // There are to differnt types, chosen one after the other

        
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
