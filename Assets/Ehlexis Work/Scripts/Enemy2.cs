using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2 : MonoBehaviour
{
    public static event Action<Enemy2> OnEnemyKilled;

    [SerializeField] float health = 3f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float timeAheadOfPlayer = 1f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] Transform playerTransform;

    Rigidbody rb;
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
            // Calculate the direction to the predicted position
            Vector3 predictedPosition = playerTransform.position + (playerTransform.GetComponent<Rigidbody>().velocity * timeAheadOfPlayer);
            Vector3 direction = predictedPosition - transform.position;

            Debug.DrawLine(transform.position, predictedPosition, Color.blue); // Draw a line to the predicted position
            Debug.DrawRay(playerTransform.position, playerTransform.GetComponent<Rigidbody>().velocity, Color.green); // Draw a ray in the player's velocity direction

            // Set the rotation of the enemy to face the predicted position
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            if (canTurn)
            {
                rb.rotation = Quaternion.Euler(0, angle, 0);
            }

            // Calculate the intercept direction
            Vector3 interceptDirection = direction.normalized;

            Debug.DrawRay(transform.position, interceptDirection, Color.red); // Draw a ray in the intercept direction

            // Limit the turn rate to make the movement smoother
            float maxTurnRate = 180f; // degrees per second
            float deltaAngle = Mathf.MoveTowardsAngle(rb.rotation.eulerAngles.y, angle, maxTurnRate * Time.deltaTime);
            rb.rotation = Quaternion.Euler(0, deltaAngle, 0);

            moveDirection = interceptDirection;

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
            playerTransform = other.transform;

            // Check if the player is in sight using a raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f,
                (other.transform.position + Vector3.up * 0.5f) - (transform.position + Vector3.up * 0.5f),
                out hit, detectionRadius, ~LayerMask.GetMask("Obstacle", "Ground"))
                && hit.collider.gameObject.CompareTag("Player"))
            {
                target = other.transform;
                playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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
            OnEnemyKilled?.Invoke(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}