using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class playerControl : MonoBehaviour
{
    #region Control
    //[Header("CONTROL")]
    
    Rigidbody2D rb;
    Animator animator;
    public CapsuleCollider2D colliderTouch;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.05f;

    [SerializeField]
    private bool _onGround;
    [SerializeField]
    private bool _onWall;

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
    private bool _isJumping = false;
    public bool IsJumping { get 
        {
            return _isJumping;
        } private set {
            _isJumping = value;
            animator.SetBool(animatorStrings.jump, _isJumping);
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        {
            return _isMoving;
        } private set {
            _isMoving = value;
            animator.SetBool(animatorStrings.isMoving, _isMoving);
        }
    }

    [SerializeField]
    private bool _isDashing = false;
    public bool IsDashing
    {
        get
        {
            _isDashing = animator.GetBool(animatorStrings.dash);
            return _isDashing;
        } private set {
            _isDashing = value;
            animator.SetBool(animatorStrings.isDashing, _isDashing);
        }
    }

    private bool _isFacingRight = true;
    public bool isFacingRight { get{ return _isFacingRight; } private set{ 
        if (_isFacingRight != value)
            {
                // Dar la vuelta a la escala local
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }}

    [SerializeField]
    public bool CanMove { get 
        {
            return animator.GetBool(animatorStrings.canMove);
        } }

    [SerializeField]
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(animatorStrings.isAlive);
        }
    }

    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    if (IsDashing)
                    {
                        return 0;
                    }
                    else
                    {
                        return moveSpeed;
                    }                 
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }  
        } }
    
    #endregion
    
    #region Main
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderTouch = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Coyote time counter
        if (onGround)
        {
            coyoteTimeCounter = CoyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Decrement jump buffer counter
        jumpBufferCounter -= Time.deltaTime;

        // Call handleJump to manage the jump logic
        handleJump();
    }

    
    private void FixedUpdate()
    {
        handleMovement();
        checkCollisions();

        if (onWall & !onGround)
        {
            Debug.Log("is on wall");
            handleWall();
        }

        animator.SetFloat(animatorStrings.yvelocity, rb.velocity.y);

        handleGravity();

    }
    #endregion

    #region Movement
    //[Header("MOVEMENT")]
    public float moveSpeed;
    public float speedDiff;
    public float movement;
    public float accelRate;
    public float decelRate;

    Vector2 moveInput;

    private void SetDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            //Mirar a la derecha
            isFacingRight = true;

        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            //Mirar a la izquierda
            isFacingRight = false;
        }
    }

    public void OnMove(InputAction.CallbackContext move)
    {
        moveInput = move.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void handleMovement()
    {
        float moveInputX = moveInput.x;
        if (CanMove && IsAlive){
            if (Mathf.Abs(moveInputX) > 0.1f) // Apply force when there is input
            {

                float targetSpeed = moveInputX * moveSpeed;           
                float speedDiff = targetSpeed - rb.velocity.x;
                float movement = speedDiff * accelRate;

                // Clamp the force to prevent excessive acceleration
                movement = Mathf.Clamp(movement, -moveSpeed * accelRate, moveSpeed * accelRate);

                rb.AddForce(new Vector2(movement, 0f));
            
            }
            else // Apply deceleration force when there is no input
            {
                if (Mathf.Abs(rb.velocity.x) > 0.1f) // Check if the player is moving
                {
                    float decelerationForce = rb.velocity.x * decelRate;
                    rb.AddForce(new Vector2(-decelerationForce, 0f));
                }
                else // Stop the player completely if the velocity is very low
                {

                    rb.velocity = new Vector2(0f, rb.velocity.y);
                }
            }
        }
    }
    #endregion

    #region Walls & Collision
    private void checkCollisions()
    {
        onGround = colliderTouch.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        onWall = colliderTouch.Cast(Vector2.left, castFilter, wallHits, wallDistance) > 0 ||
                colliderTouch.Cast(Vector2.right, castFilter, wallHits, wallDistance) > 0;
    }

    public float slideSpeed;
    
    private void handleWall()
    {
        rb.velocity = new Vector2(rb.velocity.x, - slideSpeed);

        float horizontalForce = moveInput.x * slideSpeed * 0.5f;
        rb.AddForce(new Vector2(horizontalForce, 0f));
    }
    #endregion

    #region Jumping
    //[Header("JUMP")]
    //REVISAR VARIABLES INUTILES
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float JumpPower = 10f;
    public float MaxFallSpeed = 40;
    public float FallAcceleration = 110;
    public float JumpEndEarlyGravityModifier = 3;
    public float CoyoteTime = .15f;
    public float JumpBuffer = .2f;

    private bool jumpInputReleased = false;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private void handleGravity()
    {
        // Apply additional gravity when falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Apply gravity modifier when jump is released early
        else if (rb.velocity.y > 0 && jumpInputReleased)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (JumpEndEarlyGravityModifier - 1) * Time.fixedDeltaTime;
        }

        // Clamp vertical speed to prevent exceeding max fall speed
        if (rb.velocity.y < -MaxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -MaxFallSpeed);
        }

        // Reset jumping flag when on the ground
        if (onGround)
        {
            IsJumping = false;
        }
    }

    private void handleJump()
    {
    // Handle jump buffering and coyote time
    if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && CanMove && IsAlive)
        {
            Debug.Log("Jumping Started");
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            jumpBufferCounter = 0;
            IsJumping = true;
            jumpInputReleased = false;
        }
    }

    public void OnJump(InputAction.CallbackContext jump)
    {
            if (jump.started)
        {
            jumpBufferCounter = JumpBuffer;
        }
        else if (jump.canceled)
        {
            jumpInputReleased = true;
        }
    }

    #endregion

    #region Dashing
    //[Header("DASH")]
    public bool canDash = true;
    public float dashingPower = 4f;
    public float dashSpeed = 10f;
    public float dashCooldown = 3f;
    public float dashDuration = 3f;
    //public LayerMask[] ignoreCollisionLayers;
    public LayerMask ignoreCollisionLayers;

    public void OnDash(InputAction.CallbackContext context)
    {   
        // A�adir IFrames y check health
        if (context.started && onGround && CanMove && canDash && IsAlive)
        {
            Debug.Log("Dashing started");
            StartCoroutine(HandleDash());
        }          
    }
    private IEnumerator HandleDash()
    {
        IsDashing = true;
        animator.SetTrigger(animatorStrings.dash);

        // Disable collision with specified layers
        // NO FUNCIONA CON EL BOSS
        int playerLayer = gameObject.layer;
        Debug.Log("Dashing!!!");

        /*
        foreach (LayerMask mask in ignoreCollisionLayers)
        {
            int layerIndex = (int)Mathf.Log(mask.value, 2);
            Debug.Log("Ignoring layer: " + layerIndex);
            Physics2D.IgnoreLayerCollision(playerLayer, layerIndex, true);
        }
        */
        Debug.Log("Ignoring layer: " + ignoreCollisionLayers);
        int layerIndex = (int)Mathf.Log(ignoreCollisionLayers.value, 2);
        Physics2D.IgnoreLayerCollision(playerLayer, layerIndex, true);

        // Set the velocity for the dash
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

        // Wait for the dash duration
        yield return new WaitForSeconds(dashDuration);

        /*
        foreach (LayerMask mask in ignoreCollisionLayers)
        {
            int layerIndex = (int)Mathf.Log(mask.value, 2);
            Physics2D.IgnoreLayerCollision(playerLayer, layerIndex, false);
        }
        IsDashing = false;
        */
        Physics2D.IgnoreLayerCollision(playerLayer, layerIndex, false);
        IsDashing = false;

        // Start the dash cooldown
        StartCoroutine(DashingCooldown());
    }

    private IEnumerator DashingCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion

    #region Attack
    //[Header("ATTACK")]

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackDamage;
    public float attackPosieDamage;
    public LayerMask enemyLayers;

    public void OnAttack(InputAction.CallbackContext attack)
    {
        if (attack.started)
        {
            animator.SetTrigger(animatorStrings.attack);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            Debug.Log("These enemies are hit:" + hitEnemies);

            foreach(Collider2D enemy in hitEnemies)
            {

                // Get the Damagable component from the enemy
                damagable Damagable = enemy.GetComponent<damagable>();

                // If the component exists, call the OnHit method
                if (Damagable != null)
                {
                    Debug.Log("Hit enemy");
                    Damagable.OnHit(attackDamage,attackPosieDamage);
                }
            }
        }
        
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    #endregion

}
