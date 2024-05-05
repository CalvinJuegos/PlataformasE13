using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    private Animator animator; // Reference to the Animator component

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }

        if (isDashing)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = maxJumpVelocity;
        }

        if (Input.GetButtonUp("Jump") && velocity.y > 0f)
        {
            velocity.y = minJumpVelocity;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        velocity.x = horizontal * moveSpeed;
    }

    private bool IsGrounded()
    {
        // Implement your own grounded check here using Controller2D collisions
        return controller.collisions.below;
    }

    void HandleWallSliding()
    {
        // Implement wall sliding logic based on Controller2D collisions
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        // Implement your jump logic based on Controller2D collisions
    }

    public void OnJumpInputUp()
    {
        // Implement your jump release logic if needed
    }

    private IEnumerator Dash()
    {
        if (!IsGrounded()) // Check if the player is grounded before dashing
            yield break;

        canDash = false;
        isDashing = true;
        float originalGravity = gravity;
        gravity = 0f;

        // Determine the direction of the dash based on the input
        float dashDirection = Input.GetAxisRaw("Horizontal");

        // Set the velocity accordingly
        velocity.x = dashDirection * dashingPower;

        // Set the IsDashing parameter in the Animator Controller to true
        animator.SetBool("isDashing", true);

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;

        gravity = originalGravity;
        isDashing = false;

        // Set the IsDashing parameter in the Animator Controller to false
        animator.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Flip()
    {
        // Implement your flip logic here
    }

}
