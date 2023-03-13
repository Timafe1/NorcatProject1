using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float damage = 2f;
    [SerializeField] Transform playerTransform;
    private Rigidbody rb;
    Transform target;
    Vector3 moveDirection;
    bool canTurn = true;
    bool playerInRange = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target != null && playerInRange)
        {
            // Calculate the direction to the player
            Vector3 direction = playerTransform.position - transform.position;

            // Set the rotation of the enemy to face the player
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            if (canTurn)
            {
                rb.rotation = Quaternion.Euler(0, angle, 0);
            }

            // Calculate the move direction
            Vector3 moveDirection = direction.normalized;

            rb.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy2 detected player!");

            playerTransform = other.transform;

            // Check if the player is in sight using a raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f,
                (other.transform.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f),
                out hit, detectionRadius, ~LayerMask.GetMask("Obstacle", "Ground")))
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.5f,
                    (other.transform.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f),
                    Color.green);

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Enemy2 has line of sight to player!");
                    target = other.transform;
                    playerInRange = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy2 lost player!");
            target = null;
            playerTransform = null;
            playerInRange = false;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}