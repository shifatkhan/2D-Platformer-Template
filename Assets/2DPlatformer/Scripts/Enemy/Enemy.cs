using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class holds basic data for an Enemy object.
 * @author ShifatKhan
 */
public class Enemy : Movement2D
{
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private string enemyName = "Enemy";
    [SerializeField]
    private int attackDamage = 1;
}
