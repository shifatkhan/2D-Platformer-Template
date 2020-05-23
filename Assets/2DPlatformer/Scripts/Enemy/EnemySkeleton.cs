using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class EnemySkeleton : Enemy
{
    [SerializeField]
    private float moveSpeed = 1;
    [SerializeField]
    private float chaseDistance = 6; // Distance where enemy will chase target.
    [SerializeField]
    private float attackDistance = 0.8f; // Distance where enemy will attack target.

    // TODO: Make Player.cs and EnemySkeleton.cs inherit from a new 2Dmovement script that has the following.
    public float maxJumpHeight = 4; // Max height a jump can attain.
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; // How long (seconds) before reaching jumpHeight.
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    Vector2 direction;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 homePosition; // Place to return to if player is gone.

    private bool followTarget;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set default target to be the Player.
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        // Calculate gravity.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        homePosition = gameObject.transform.position;
    }

    void Update()
    {
        CalculateVelocity();
        UpdateAnimator();

        controller.Move(velocity * Time.deltaTime, direction);

        // Set velocity.y to 0 if player is on ground or hit ceiling.
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    void FixedUpdate()
    {
        
    }

    /** Calculate and apply X and Y velocity to player depending on player inputs.
     */
    void CalculateVelocity()
    {
        CheckDistance();

        // Smooth out change in directionX (when changing direction)
        float targetVelocityX = direction.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Apply gravity to player's velocity.
        velocity.y += gravity * Time.deltaTime;
    }

    void CheckDistance()
    {
        // If target is inside chase radius and outside attack radius.
        if (Vector2.Distance(target.position, transform.position) <= chaseDistance
            && Vector2.Distance(target.position, transform.position) > attackDistance)
        {
            direction = (target.position - transform.position).normalized;
        }
        else if ((Vector2.Distance(homePosition, transform.position) <= chaseDistance && Vector2.Distance(homePosition, transform.position) > attackDistance) 
            && Vector2.Distance(target.position, transform.position) >= chaseDistance)
        {
            // Target is gone.
            direction = (homePosition - transform.position).normalized;
        }
        else
        {
            direction.x = 0;
        }
    }

    public void Jump()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
    }

    public IEnumerator AttackCo()
    {
        animator.SetBool("attacking1", true);
        yield return null; // Wait 1 frame
        animator.SetBool("attacking1", false);
        //yield return null; // Wait 1 frame

    }

    /** Updates the enemy's animation.
     */
    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", direction.x != 0);
        }

        if (spriteRenderer != null)
        {
            if (direction.x > 0) // Facing right
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0) // Facing left
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
