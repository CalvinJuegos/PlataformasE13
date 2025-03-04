using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossRoseBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    //Animator animator;
    public CapsuleCollider2D colliderTouch;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.05f;

    private roseState currentState;

    private Animator animator;
    public GameObject player;
    public GameObject projectilePrefab;

    private bool _finishedMainAttack = true;

    public bool animationFinished = false;

    public bool hasTransformed = false;
    public bool preparedToThrust= false;
    public bool thrustCooldown = false;
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

    [SerializeField]
    public bool CanMove
    {
        get
        {
            return animator.GetBool(animatorStrings.canMove);
        }
    }

    [SerializeField]
    private bool _onGround;
    public bool onGround
    {
        get
        {
            return _onGround;
        }
        private set
        {
            _onGround = value;
            animator.SetBool(animatorStrings.onGround, value);
        }
    }

    [SerializeField]
    private bool _onWall;
    public bool onWall
    {
        get
        {
            return _onWall;
        }
        private set
        {
            _onWall = value;
            // animator.SetBool(animatorStrings.onWall, value);
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(animatorStrings.isMoving, _isMoving);
        }
    }

    [SerializeField]
    private bool _isJumping = false;
    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        private set
        {
            _isJumping = value;
            animator.SetBool(animatorStrings.jump, _isJumping);
        }
    }

    private bool _isFacingRight = false; // Initially set to false if the boss starts facing left
    public bool isFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                // Flip the local scale
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                Debug.Log($"Boss flipped. Now facing right: {value}");
            }
            _isFacingRight = value;
        }
    }

    private bool _inRangeForMelee = false;
    public bool InRangeForMelee
    {
        get { return _inRangeForMelee; }
        private set
        {
            _inRangeForMelee = value;
        }
    }

    private damagable Damagable;
    public bool startingSummon = false;
    public float meleeRangeThreshold = 5.0f;
    public float mediumRangeThreshold = 15.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Damagable = GetComponent<damagable>();
        //DetectionRange = GameObject.FindWithTag("Area").GetComponent<detectionRange>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderTouch = GetComponent<CapsuleCollider2D>();

        // Ignore collisions with platforms
        int bossLayer = LayerMask.NameToLayer("Boss");
        int platformLayer = LayerMask.NameToLayer("Platforms");
        Physics2D.IgnoreLayerCollision(bossLayer, platformLayer);
    }

    private void Start()
    {
        ChangeState(new roseIdle(this, player, animator)); // Initial state
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(roseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #region Choice

    public void ChooseAttack(int playerDistance)
    {
        int randomNumber = Random.Range(0, 100);
        Debug.Log("Choosing attack");
        Debug.Log(randomNumber);
        if (playerDistance == 0)
        {
            // Melee range = Attack, Follow Up, --Thrust
            if (randomNumber <= 65)
            {
                // Attack
                ChangeState(new roseAttack(this, player, animator));
            }
            else if (randomNumber > 65 && randomNumber <= 90)
            {
                // Dodge
                ChangeState(new roseDodge(this, player, animator));
            }
            else if (randomNumber > 65)
            {
                // Thrust
                ChangeState(new roseThrust(this, player, animator));
            }
        
        } else if (playerDistance == 1)
        {
            // Medium range = Thrust, Dodge, --Transform
            if (randomNumber <= 35)
            {
                // Thrust
                ChangeState(new roseThrust(this, player, animator));
            }
            else if (randomNumber > 35 && randomNumber <= 70)
            {
                // Dodge
                ChangeState(new roseDodge(this, player, animator));
            }
            else if (randomNumber > 70)
            {
                // Transform
                if (!hasTransformed)
                {
                    ChangeState(new roseTransform(this, player, animator));
                }
                else
                {
                    ChangeState(new roseThrust(this, player, animator));
                }
            }

        } else if (playerDistance == 2)
        {
            // Far range = Thrust, Dodge, --Transform
            
            if (randomNumber <= 35)
            {
                // Thrust
                ChangeState(new roseThrust(this, player, animator));
            }
            else if (randomNumber > 35 && randomNumber <= 70)
            {
                // Dodge
                ChangeState(new roseDodge(this, player, animator));
            }
            else if (randomNumber > 70)
            {
                if (!hasTransformed)
                {
                    // Transform
                    ChangeState(new roseTransform(this, player, animator));
                }
                else
                {
                    ChangeState(new roseThrust(this, player, animator));
                }
            }
        }
    }

    #endregion

    #region Triggers

    public int playerDistance()
    {
        // Calculate the distance between the boss and the player
        float distance = Vector2.Distance(transform.position, player.transform.position);

        // Determine the range based on the distance
        if (distance < meleeRangeThreshold)
        {
            return 0; // Melee range = Attack, Follow Up, --Thrust
            Debug.Log("Melee range");
        }
        else if (distance < mediumRangeThreshold)
        {
            return 1; // Medium range = Thrust, Dodge, --Transform
            Debug.Log("Medium range");
        }
        else
        {
            return 2; // Far range = Thrust, Dodge, --Transform
            Debug.Log("Far range");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState is ICollisionHandler collisionHandler)
        {
            collisionHandler.HandleCollision(collision);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //animator.SetBool(animatorStrings.agroRange, false);
            // Cycle through ranged attacks, starting with summon
            //ChangeState(new ugtSummon(this, player, animator));
            //startingRanged = true;
        }
    }

    public float followUpProbability = 0.65f;

    public void handleFollowUp()
    {
        // Generate a random number between 0 and 1
        float randomValue = Random.Range(0f, 1f);
        // Check if the random value is less than the follow-up probability
        if (randomValue < followUpProbability)
        {
            // Trigger the follow-up attack
            Debug.Log("Triggering follow-up attack");
            ChangeState(new roseFollowUp(this, player, animator));
        }
        else
        {
            // Maybe Dodge, maybe burst
            Debug.Log("No follow-up attack");
            //ChangeState(new ugtDodge(this, player, animator));
            animator.SetTrigger(animatorStrings.agroTrigger);
        }
    }

    public void OnAnimationComplete()
    {
        Debug.Log("Animation complete");
        animationFinished = true;
    }

    public void Thrust()
    {
        Debug.Log("Thrusting");
        preparedToThrust = true;
    }

    public void EndThrust()
    {
        Debug.Log("Thrust ended");
        thrustCooldown = true;
    }

    #endregion

    #region Direction & Movements

    public void FacePlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;

        if (directionToPlayer.x > 0 && !_isFacingRight)
        {
            // Face right
            Debug.Log("Facing right towards player");
            isFacingRight = true;
        }
        else if (directionToPlayer.x < 0 && _isFacingRight)
        {
            // Face left
            Debug.Log("Facing left towards player");
            isFacingRight = false;
        }
    }

    //public float thrustSpeed = 10f;

    #endregion

    #region Collisions & Jumps

    //REVISAR VARIABLES INUTILES


    private void checkCollisions()
    {
        onGround = colliderTouch.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        onWall = colliderTouch.Cast(Vector2.left, castFilter, wallHits, wallDistance) > 0 ||
                colliderTouch.Cast(Vector2.right, castFilter, wallHits, wallDistance) > 0;
    }

    // Handle gravity �?

    private void handleJump()
    {

    }
    #endregion

    public float timeToNextAttack = 5f;

    IEnumerator bossCooldown()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(timeToNextAttack);
        _finishedMainAttack = true;
    }
}

