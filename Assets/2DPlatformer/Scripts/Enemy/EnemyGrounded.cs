using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrounded : Movement2D
{
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private string enemyName = "Enemy";
    [SerializeField]
    private int attackDamage = 1;
}
