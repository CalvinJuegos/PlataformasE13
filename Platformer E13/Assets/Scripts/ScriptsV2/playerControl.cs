using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(touchingDirections))] 


public class playerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpImpulse = 10f;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

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
        MovementStart();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Check if alive when hp implemented
        if (context.started && touching_directions.IsGrounded && CanMove)
        {
            animator.SetTrigger(animatorStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
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

    private void MovementStart()
    {
        // FUnci�n debe ser cambiada a no void y que devuelva un array que se actualice en FixedUpdate en vez de aqu�
        // Si se quiere a�adir otros tipos de movimiento tambi�n
        if (CanMove)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            animator.SetFloat(animatorStrings.yvelocity, rb.velocity.y);
        }
    }

}
