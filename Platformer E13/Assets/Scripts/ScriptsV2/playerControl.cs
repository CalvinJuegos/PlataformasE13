using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(touchingDirections))] 


public class playerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpImpulse = 10f;
    public float dashingPower = 4f;
    public float dashSpeed = 10f;
    public float dashCooldown = 3f;
    public float dashDuration = 3f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public bool canDash = true;

    //public float maxJumpHeight = 4;
    //public float minJumpHeight = 1;
    //public float timeToJumpApex = .4f;
    //float accelerationTimeAirborne = .2f;
    //float accelerationTimeGrounded = .1f;

    //rivate float dashingPower = 24f;
    //public float dashingTime = 0.2f;
    //public float dashingCooldown = 1f;

    touchingDirections touching_directions;

    Vector2 moveInput;

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

    public bool CanMove { get 
        {
            return animator.GetBool(animatorStrings.canMove);
        } }
    
    // Need to implement animator

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

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touching_directions = GetComponent<touchingDirections>();
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(animatorStrings.yvelocity, rb.velocity.y);

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

    public void OnJump(InputAction.CallbackContext context)
    {
        // Check if alive when hp implemented
        if (context.started && touching_directions.IsGrounded && CanMove)
        {
            Debug.Log("Jumping Started");
            animator.SetTrigger(animatorStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            //rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            //rb.AddForce()
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {   
        // Añadir IFrames y check health
        if (context.started && touching_directions.IsGrounded && CanMove && canDash && IsAlive)
        {
            Debug.Log("Dashing started");
            StartCoroutine(HandleDash());
        }          
    }

    public void OnAttack(InputAction.CallbackContext attack)
    {
        if (attack.started)
        {
            animator.SetTrigger(animatorStrings.attack);
        }
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
