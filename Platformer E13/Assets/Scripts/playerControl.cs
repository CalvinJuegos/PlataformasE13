using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class playerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float speedDiff;
    public float movement;
    public float accelRate = 1f;

    Vector2 moveInput;

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
        }
    }

    public bool _isFacingRight = true;
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

    public float slideSpeed;

    Rigidbody2D rb;
    Animator animator;
    public CapsuleCollider2D colliderTouch;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderTouch = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    private void FixedUpdate()
    {

        if (IsMoving)
        {
            handleMovement();
        }
        
        checkCollisions();

        if (onWall & !onGround)
        {
            Debug.Log("is on wall");
            handleWall();
        }

        animator.SetFloat(animatorStrings.yvelocity, rb.velocity.y);

    }
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
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        //speedDiff =moveInput.x*moveSpeed
        //speedDiff = moveSpeed - rb.velocity.x;
        //movement = speedDiff * accelRate;
        //rb.AddForce(movement * moveInput);
    }

    private void checkCollisions()
    {
        onGround = colliderTouch.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        onWall = colliderTouch.Cast(Vector2.left, castFilter, wallHits, wallDistance) > 0 ||
                colliderTouch.Cast(Vector2.right, castFilter, wallHits, wallDistance) > 0;

    }
    
    private void handleWall()
    {
        rb.velocity = new Vector2(rb.velocity.x, - slideSpeed);
    }

    #region Jumping
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Header("JUMP")]
    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 10f;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 40;

    [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    public float FallAcceleration = 110;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 3;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = .15f;

    [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = .2f;
    public void OnJump(InputAction.CallbackContext jump)
    {
        // Check if alive when hp implemented
        if (jump.performed && onGround && CanMove)
        {
            Debug.Log("Jumping Started");
            animator.SetTrigger(animatorStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            //rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            //rb.AddForce()
        }
        if (jump.canceled && rb.velocity.y > 0.5f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * lowJumpMultiplier);
        }
    }
    #endregion

    #region Dashing
    public bool canDash = true;
    public float dashingPower = 4f;
    public float dashSpeed = 10f;
    public float dashCooldown = 3f;
    public float dashDuration = 3f;
    public void OnDash(InputAction.CallbackContext context)
    {   
        // Añadir IFrames y check health
        if (context.started && onGround && CanMove && canDash && IsAlive)
        {
            Debug.Log("Dashing started");
            StartCoroutine(HandleDash());
        }          
    }
    private IEnumerator HandleDash()
    {
        animator.SetTrigger(animatorStrings.dash);
        // No acelera
        rb.velocity = new Vector2(moveInput.x * dashSpeed, 0f);
        while (!IsDashing)
        {
            yield return null;
        }

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
    public void OnAttack(InputAction.CallbackContext attack)
    {
        if (attack.started)
        {
            animator.SetTrigger(animatorStrings.attack);
        }
    }
    #endregion
    

    

    //private void MovementStart()
    //{
    // FUnción debe ser cambiada a no void y que devuelva un array que se actualice en FixedUpdate en vez de aquí
    // Si se quiere añadir otros tipos de movimiento también
    //   if (CanMove)
    //  {
    //     
    //    }
    //}

}
