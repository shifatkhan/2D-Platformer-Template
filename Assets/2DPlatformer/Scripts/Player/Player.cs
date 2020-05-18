using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class that handles player physics.
 * @author ShifatKhan
 * @Special thanks to Sebastian Lague
 */
[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{
    // TODO: Change jumpHeight and timeToJumpApex to 'protected'
    public float maxJumpHeight = 4; // Max height a jump can attain.
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; // How long (seconds) before reaching jumpHeight.
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public bool wallJumpingEnabled = true; // Enable/Disable the ability to walljump.
    bool wallSliding;
    int wallDirX;

    public Vector2 wallJumpClimb; // For climbing a wall (small jumps)
    public Vector2 wallJumpOff; // For getting off a wall (small jump off the wall to the ground)
    public Vector2 wallLeap; // For jumping from wall to wall (big/long jump)

    public float wallSlideSpeedMax = 3; // Velocity at which we will descend a wall slide.
    public float wallStickTime = .25f; // Time after which player gets off the wall when no jump inputs were given (instead just getting off)
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;
    
    Controller2D controller;
    Vector2 directionalInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calculate gravity.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // TODO: Put physics in FixedUpdate()
    private void Update()
    {
        CalculateVelocity();
        UpdateAnimator();

        if (wallJumpingEnabled)
        {
            HandleWallSliding();
        }

        controller.Move(velocity * Time.deltaTime, directionalInput);

        // Set velocity.y to 0 if player is on ground or hit ceiling.
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    /** Takes in player input and assigns it.
    */
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    /** Handle player jump velocity (takes into account for all 3 types of wall jumps)
     */
    public void OnJumpInputDown()
    {
        animator.SetTrigger("takeoff");
        if (wallJumpingEnabled && wallSliding)
        {
            if (wallDirX == directionalInput.x) // For wall climbing (moving at same direction as where the wall is)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0) // For jumping off the wall to the ground.
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else // For leaping from the wall with a big jump.
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (directionalInput.y != -1) // For when we want to fall through platform.
            {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    /** Handle player velocity when jump is released.
     */
    public void OnJumpInputUp()
    {
        // If we let go off jump button, we want to fall quicker (with min jump velocity)
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    /** Calculate and apply X and Y velocity to player depending on player inputs.
     */
    void CalculateVelocity()
    {
        // Smooth out change in directionX (when changing direction)
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Apply gravity to player's velocity.
        velocity.y += gravity * Time.deltaTime;
    }

    /** Check wallsliding state and apply jumping physics to player.
     */
    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;

        wallSliding = false;
        // We can wall-slide if player is pressing left or right. 
        if (velocity.x != 0 && (controller.collisions.left || controller.collisions.right) 
            && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            // Slowdown descent speed.
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            // Time after which player gets off the wall 
            // when no jump inputs were given (instead just getting off)
            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    public IEnumerator AttackCo()
    {
        animator.SetBool("attacking1", true);
        yield return null; // Wait 1 frame
        animator.SetBool("attacking1", false);
        //yield return null; // Wait 1 frame

    }

    /** Updates the player's animation.
     */
    void UpdateAnimator()
    {
        if(animator != null)
        {
            animator.SetBool("isRunning", directionalInput.x != 0);

            animator.SetBool("isAirborne", !controller.collisions.below);
        }

        if(spriteRenderer != null)
        {
            if(directionalInput.x > 0) // Facing right
            {
                spriteRenderer.flipX = false;
            }
            else if (directionalInput.x < 0) // Facing left
            {
                spriteRenderer.flipX = true;
            }

            // TODO: Change approach since we might want some children not to change.
            // Flip child gameObjects according to where the Player is facing.
            for (int i = 0; i < transform.childCount; i++)
            {
                Quaternion rotation = transform.GetChild(i).localRotation;
                rotation.y = spriteRenderer.flipX ? 180 : 0;
                transform.GetChild(i).localRotation = rotation;
            }
        }
    }
}
