using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3Behaviour : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    public GameObject projectilePrefab;

    private bool _finishedMainAttack = true;
    public bool finishedMainAttack
    {
        get
        {
            return _finishedMainAttack;
        }
        private set
        {
            if (value == true && !_finishedMainAttack) // Only start cooldown if setting to true
            {
                StartCoroutine(bossCooldown());
            }
            else
            {
                _finishedMainAttack = value;
            }

            Debug.Log("Ended attack!!!!!");
        }
    }

    public void SetFinishedMainAttack(bool value)
    {
        finishedMainAttack = value;
    }

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

        
        //flashAnim = GameObject.FindWithTag("flashAnimator").GetComponent<Animator>();
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
                        StartCoroutine(lightBeamZone());
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
                        StartCoroutine(lightBeamZone());
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
  
    /*
    public void ChooseAttack()
    {
        StartCoroutine(LightBeam());
    }
    */

    public float timeToNextAttack = 5f;

    IEnumerator bossCooldown()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(timeToNextAttack);
        _finishedMainAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Attack"+finishedMainAttack);
        if (finishedMainAttack)
        {
            // CHooses main attack depending on player position
            //Debug.Log("NOW");
            ChooseAttack();           
        }

        // For continuos attacks (Fire rate phase 1 low, fire rate phase 2 higher)
        if (Time.time >= nextFireTime)
        {
            //Debug.Log("Basic Spawn Starts");
            // !! Spawn position is SET HERE (Can set different types of projectiles to different places with function)
            spawnPosition = transform.position;
            StartCoroutine(FireProjectile(spawnPosition));
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
        attackAnim = attackAnim.GetComponent<Animator>();
        Debug.Log(animatorStrings.backOff);
        if(attackAnim != null) 
        {
            attackAnim.SetTrigger(animatorStrings.backOff);
        } else
        {
            Debug.Log("Empyt backoff");
        }
        

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

    #region Flower Attack
    public Animator attackFlower;
    public GameObject[] flowers;
    public float flowerLifespan = 3.2f;

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
        yield return new WaitForSeconds(flowerLifespan);
        finishedMainAttack = true;
    }

    #endregion
    // Deals the most damage
    // Forces player to hide behind walls (if not in trigger collider > take damage)
    // Rare attack except is done whenever at 50% life

    #region Light Flash
    public Animator flashAnim;
    public float flashLifespan = 3.2f;

    IEnumerator LightFlash()
    {
        finishedMainAttack = false;
        flashAnim.SetTrigger(animatorStrings.flashStart);
        Debug.Log("lightFlash");

        yield return new WaitForSeconds(flashLifespan);
    }
    #endregion

    // Light beams that get players position and after a delay it starts
    // Needs animation IMPLEMENTATION >>> IMPROVEMENT of logic based on animations
    #region Light Beam

    public float beamLenght = 1.2f;
    public float setIterations = 2f;
    public float setDelay = 0.5f;
    public GameObject beamSet1;
    public GameObject beamSet2;

    IEnumerator LightBeam()
    {
        finishedMainAttack = false;
        Debug.Log("LightBeam");

        
        // Activates the objectsColliders with dealsDamage and triggers their lightning animation
        // There are to differnt types, chosen one after the other
        for(int i = 0; i < setIterations; i++)
        {
            System.Random rng = new System.Random();
            int randomValue = rng.Next(100);
            if (randomValue <= 50)
            {
                StartCoroutine(handleBeam(beamSet1));
            }
            else
            {
                StartCoroutine(handleBeam(beamSet2));
            }
            yield return new WaitForSeconds(beamLenght+setDelay);
        }
        yield return null;
    }

    IEnumerator handleBeam(GameObject set)
    {
        Debug.Log("Entered");
        beamSet1.SetActive(true);
        beamSet2.SetActive(true);
        /*
        foreach (GameObject beam in set)
        {
            Debug.Log("Set entered");
            beam.SetActive(true);

            
            //Animator animator = beam.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(animatorStrings.beam);
                Debug.Log("animadorEmpieza");
            }
            else
            {
                Debug.LogError($"Animator component not found on GameObject: {beam.name}");
            }
            

        yield return null;
        }
        */
        yield return new WaitForSeconds(beamLenght);
        beamSet1.SetActive(false);
        beamSet2.SetActive(false);
        /*
        foreach (GameObject beam in set)
        {
            beam.SetActive(false); // Maybe can be done later in animator

            
            //Animator animator = beam.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(animatorStrings.flowerStart);
                Debug.Log("animadorEmpieza");
            }
            else
            {
                Debug.LogError($"Animator component not found on GameObject: {beam.name}");
            }
            

        yield return null;
        }
        */
        finishedMainAttack = true;
    }

    #endregion

    // Set active lighting beams, if 1, one set of items,
    // If 2, the other set
    #region Light Beam Zone


    public GameObject beamPrefab;
    public float spawnInterval = 0.5f;
    public int iterations = 3;
    public float beamDelay = 0.2f;

    IEnumerator lightBeamZone()
    {
        finishedMainAttack = false;

        for (int i = 0; i < iterations; i++)
        {
            // Spawn the beam at the player's position with a delay
            Vector2 beamLocation = playerTransform.position;
            yield return new WaitForSeconds(beamDelay);
            GameObject beam = Instantiate(beamPrefab, beamLocation, Quaternion.identity);

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);
            Destroy(beam);
        }
        finishedMainAttack = true;
    }

    #endregion

    // MAYbe?
    IEnumerator ImmuneBackOff()
    {
        Debug.Log("ImmuneBackoff");
        finishedMainAttack = false;

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
        Debug.Log("FireProjectile");
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
                Debug.Log("InstantiateProjectileBasic");
                projScript.InitializeDirection(direction);
            }
        }
        yield return null;
    }
}
