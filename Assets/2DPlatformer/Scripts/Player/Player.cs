using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Class that handles player physics.
 * @author ShifatKhan
 * @Special thanks to Sebastian Lague
 */
public class Player : Movement2D
{
    public bool wallJumpingEnabled = true; // Enable/Disable the ability to walljump.
    bool wallSliding;
    int wallDirX;

    public Vector2 wallJumpClimb; // For climbing a wall (small jumps)
    public Vector2 wallJumpOff; // For getting off a wall (small jump off the wall to the ground)
    public Vector2 wallLeap; // For jumping from wall to wall (big/long jump)

    public float wallSlideSpeedMax = 3; // Velocity at which we will descend a wall slide.
    public float wallStickTime = .25f; // Time after which player gets off the wall when no jump inputs were given (instead just getting off)
    float timeToWallUnstick;
    
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

    public IEnumerator AttackCo()
    {
        if(currentState != State.stagger && currentState != State.attack)
        {
            animator.SetBool("attacking1", true);
            yield return null; // Wait 1 frame
            animator.SetBool("attacking1", false);
            //yield return null; // Wait 1 frame
        }
    }
}
