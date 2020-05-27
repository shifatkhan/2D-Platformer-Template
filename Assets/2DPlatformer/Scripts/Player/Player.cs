using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class that handles player physics.
 * @author ShifatKhan
 * @Special thanks to Sebastian Lague
 */
public class Player : Movement2D
{
    [SerializeField] private bool wallJumpingEnabled = true; // Enable/Disable the ability to walljump.
    [SerializeField] private bool wallSliding;
    private int wallDirX;

    [SerializeField] private Vector2 wallJumpClimb = new Vector2(7.5f, 16); // For climbing a wall (small jumps)
    [SerializeField] private Vector2 wallJumpOff = new Vector2(8.5f, 7); // For getting off a wall (small jump off the wall to the ground)
    [SerializeField] private Vector2 wallLeap = new Vector2(18, 17); // For jumping from wall to wall (big/long jump)

    [SerializeField] private float wallSlideSpeedMax = 3; // Velocity at which we will descend a wall slide.
    [SerializeField] private float wallStickTime = .1f; // Time after which player gets off the wall when no jump inputs were given (instead just getting off)
    [SerializeField] private float timeToWallUnstick;

    [SerializeField] private float attackSpeed = 0.5f;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (wallJumpingEnabled)
        {
            HandleWallSliding();
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

    public IEnumerator Attack1Co()
    {
        if(currentState != State.stagger && currentState != State.attack)
        {
            animator.SetTrigger("attack1");
            SetCurrentState(State.attack);

            yield return new WaitForSeconds(attackSpeed);

            SetCurrentState(State.idle);
        }
    }

    public override void UpdateState()
    {
        if (currentState != State.attack)
        {
            base.UpdateState();
        }
    }
}
