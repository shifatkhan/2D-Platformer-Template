using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float chaseDistance; // Distance where enemy will chase target.
    [SerializeField]
    private float attackDistance; // Distance where enemy will attack target.

    // TODO: Make Player.cs and EnemySkeleton.cs inherit from a new 2Dmovement script that has the following.
    public float maxJumpHeight = 4; // Max height a jump can attain.
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f; // How long (seconds) before reaching jumpHeight.

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform homePosition; // Place to return to if player is gone.

    void Start()
    {
        // Set default target to be the Player.
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        // Calculate gravity.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector2.Distance(target.position, transform.position) <= chaseDistance
            && Vector2.Distance(target.position, transform.position) > attackDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
