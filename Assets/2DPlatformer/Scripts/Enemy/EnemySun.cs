using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class takes care of the AI for Enemy Sun.
 * @author ShifatKhan
 */
public class EnemySun : Enemy
{
    [SerializeField]
    private float chaseDistance = 6; // Distance where enemy will chase target.
    [SerializeField]
    private float attackDistance = 0.8f; // Distance where enemy will attack target.

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 homePosition; // Place to return to if player is gone.

    public override void Start()
    {
        // Set default target to be the Player.
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        homePosition = gameObject.transform.position;
    }

    public override void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        // If target is inside chase radius and outside attack radius.
        if(Vector2.Distance(target.position, transform.position) <= chaseDistance
            && Vector2.Distance(target.position, transform.position) > attackDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
