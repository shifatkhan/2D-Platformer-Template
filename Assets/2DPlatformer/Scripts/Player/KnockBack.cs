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
                // Make rigidbody dynamic so we can add a knock back force.
                enemy.isKinematic = false;

                // Get Direction (by normalizing) and add force to attack (thrust)
                Vector2 direction = enemy.transform.position - transform.position;
                direction = direction.normalized * thrust;
                enemy.constraints = RigidbodyConstraints2D.FreezePositionY; // so enemy doesn't fall through platform.
                enemy.AddForce(direction, ForceMode2D.Impulse);

                StartCoroutine(KnockCo(enemy));
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
        }
    }
}
