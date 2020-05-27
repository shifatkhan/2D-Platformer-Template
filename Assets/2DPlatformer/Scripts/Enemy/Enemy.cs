using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class holds basic data for an Enemy object.
 * @author ShifatKhan
 */
public class Enemy : Movement2D
{
    [SerializeField] protected int health = 3;
    [SerializeField] protected string enemyName = "Enemy";
    [SerializeField] protected int attackDamage = 1;
    [SerializeField] protected float attackSpeed = 0f;
}
