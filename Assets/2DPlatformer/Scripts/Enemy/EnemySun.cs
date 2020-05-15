using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySun : Enemy
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float chaseDistance; // Distance where enemy will chase target.
    [SerializeField]
    private float attackDistance; // Distance where enemy will attack target.

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform homePosition; // Place to return to if player is gone.

    void Start()
    {
        // Set default target to be the Player.
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }
    
    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if(Vector2.Distance(target.position, transform.position) <= chaseDistance
            && Vector2.Distance(target.position, transform.position) > attackDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
