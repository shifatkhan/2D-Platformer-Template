using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author ShifatKhan
 */
public class KnockBack : MonoBehaviour
{
    public float thrust;
    public float knockTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Knocked: " + other);
            // Get Direction (by normalizing) and multiply the attack with thrust power.
            Vector2 direction = other.transform.position - transform.position;
            direction = direction.normalized * thrust;
            
            other.GetComponent<Movement2D>().KnockBack(direction, knockTime);
        }
    }

    //private IEnumerator KnockCo(Rigidbody2D enemy)
    //{
    //    if (enemy != null)
    //    {
    //        yield return new WaitForSeconds(knockTime);
    //        enemy.GetComponent<Enemy>().ApplyForce(Vector3.zero);
    //        enemy.GetComponent<Enemy>().SetCurrentState(EnemyState.idle);
    //    }
    //}
}
