using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float thrust;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if(enemy != null)
            {
                // Make rigidbody dynamic so we can add a knock back force.
                enemy.isKinematic = false;

                Vector2 direction = enemy.transform.position - transform.position;
                direction = direction.normalized * thrust;
                enemy.AddForce(direction, ForceMode2D.Impulse);

                enemy.isKinematic = true;
            }
        }
    }
}
