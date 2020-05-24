using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class takes care of calculating the gravity and movement of a 2d Platformer entity.
 * This is mostly used for beings that are grounded (the player, humans, skeletons, etc.)
 * Not for flying entities e.g.: birds, helicopters, etc.
 */
[RequireComponent(typeof(Controller2D))]
public class Movement2D : MonoBehaviour
{
    // TODO: Change jumpHeight and timeToJumpApex to 'protected'
    public float maxJumpHeight = 4; // Max height a jump can attain.
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; // How long (seconds) before reaching jumpHeight.
    protected float accelerationTimeAirborne = .2f;
    protected float accelerationTimeGrounded = .1f;

    public float moveSpeed = 6;

    protected float gravity;
    protected float maxJumpVelocity;
    protected float minJumpVelocity;
    public Vector3 velocity;
    protected float velocityXSmoothing;

    protected Controller2D controller;
    protected Vector2 directionalInput;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    
    public virtual void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calculate gravity.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
    
    public virtual void Update()
    {
        UpdateAnimator();
    }
    
    public virtual void FixedUpdate()
    {
        CalculateVelocity();

        Move();
    }

    /** Calculate and apply X and Y velocity to game object..
     */
    protected void CalculateVelocity()
    {
        // Smooth out change in directionX (when changing direction)
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Apply gravity to velocity.
        velocity.y += gravity * Time.deltaTime;
    }

    protected void Move()
    {
        controller.Move(velocity * Time.deltaTime, directionalInput);

        // Set velocity.y to 0 if gameObject is on ground or hit ceiling.
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    /** Updates animation.
     */
    protected void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", directionalInput.x != 0);

            animator.SetBool("isAirborne", !controller.collisions.below);
        }

        if (spriteRenderer != null)
        {
            if (directionalInput.x > 0) // Facing right
            {
                spriteRenderer.flipX = false;
            }
            else if (directionalInput.x < 0) // Facing left
            {
                spriteRenderer.flipX = true;
            }

            // TODO: Change approach since we might want some children not to change.
            // Flip CHILD gameObjects according to where the gameobject is facing.
            for (int i = 0; i < transform.childCount; i++)
            {
                Quaternion rotation = transform.GetChild(i).localRotation;
                rotation.y = spriteRenderer.flipX ? 180 : 0;
                transform.GetChild(i).localRotation = rotation;
            }
        }
    }
}
