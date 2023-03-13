using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public GameObject impactPrefab; // object to be instantiated on impact
    public Transform impactPosition; // position where the object will be instantiated
    public float damage = 10f; // amount of damage dealt to enemy

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // reduce enemy health by damage amount
            Enemy2 enemy = collision.gameObject.GetComponent<Enemy2>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // instantiate impact effect at set position
            Instantiate(impactPrefab, impactPosition.position, impactPosition.rotation);

            
        }
    }
}