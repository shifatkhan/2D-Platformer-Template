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
        if(other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if(enemy != null)
            {
                enemy.GetComponent<Enemy>().SetCurrentState(EnemyState.stagger);

                // Get Direction (by normalizing) and multiply the attack with thrust power.
                Vector2 direction = enemy.transform.position - transform.position;
                direction = direction.normalized * thrust;

                enemy.GetComponent<Enemy>().ApplyForce(direction);

                StartCoroutine(KnockCo(enemy));
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.GetComponent<Enemy>().ApplyForce(Vector3.zero);
            enemy.GetComponent<Enemy>().SetCurrentState(EnemyState.idle);
        }
    }
}
