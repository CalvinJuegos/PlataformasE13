using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossUGTbehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    //Animator animator;
    public CapsuleCollider2D colliderTouch;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.05f;

    private ugtState currentState;

    private Animator animator;
    public GameObject player;
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
        ChangeState(new ugtIdle(this, player, animator)); // Initial state
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void ChangeState(ugtState newState)
    {
        currentState?.Exit();
        currentState = newState;
        //Debug.Log("Entering estate",currentState);
        currentState.Enter();
    }

    #region Triggers

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered range");
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(animatorStrings.agroRange, true);
            ChangeState(new ugtAgro(this, player, animator));
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(animatorStrings.agroRange, false);
            // Cycle through ranged attacks, starting with summon
            ChangeState(new ugtSummon(this, player, animator));
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
            ChangeState(new ugtFollowUp(this, player, animator));
        }
        else
        {
            Debug.Log("No follow-up attack");
            ChangeState(new ugtAgro(this, player, animator));
            animator.SetTrigger(animatorStrings.agroTrigger);
        }
    }
    public GameObject spawnPoint;
    public int projNum;
    public void spawnProjectiles()
    {
        Instantiate(projectilePrefab, spawnPoint.transform.position, Quaternion.identity);
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
