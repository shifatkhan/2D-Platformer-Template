using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class holds basic data for an Enemy object.
 * @author ShifatKhan
 */
public enum EnemyState
{
    idle,
    move, // Indicates state of when enemy is moving, which includes flying, running, walking, etc.
    attack,
    stagger
}

public class Enemy : Movement2D
{
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private string enemyName = "Enemy";
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField] // TODO: remove serialized
    protected EnemyState currentState;

    public override void Start()
    {
        base.Start();
        SetCurrentState(EnemyState.idle);
    }

    public void SetCurrentState(EnemyState newState)
    {
        //if (currentState != newState)
            currentState = newState;
    }
}
