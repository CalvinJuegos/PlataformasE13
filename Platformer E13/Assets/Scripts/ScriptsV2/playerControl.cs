using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class playerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
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

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetDirection(moveInput);
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

}
