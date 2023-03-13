using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float moveSpeed = 5f;        // Base movement speed
    public float sprintSpeed = 8f;      // Sprinting speed
    public float jumpHeight = 2f;       // Jump height
    public float dodgeDistance = 5f;    // Distance to dodge
    public float groundCheckDistance = 1.1f; // Ground check distance

    private Rigidbody rb;               // Rigidbody component reference
    private Camera mainCamera;          // Main camera component reference
    private bool isGrounded = true;     // Grounded flag
    private bool isSprinting = false;   // Sprinting flag
    private bool hasJumped = false;     // Jump flag

    void Start()
    {
        // Get references to Rigidbody and Main Camera components
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if player is grounded or in mid-air
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance) && rb.velocity.y <= 0.01f;

        // Movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Sprint input
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;

        // Dodge input
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Dodge();

        // Calculate movement direction based on camera rotation
        Vector3 direction = mainCamera.transform.forward * vertical + mainCamera.transform.right * horizontal;
        direction.y = 0f;
        direction.Normalize();

        // Move the player
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        rb.velocity = direction * speed + Vector3.up * rb.velocity.y;

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !hasJumped)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            hasJumped = true;
        }

        // Reset jump flag if player lands on ground or hits a wall
        if (isGrounded || Physics.Raycast(transform.position, Vector3.forward, 0.5f) || Physics.Raycast(transform.position, Vector3.back, 0.5f)
            || Physics.Raycast(transform.position, Vector3.left, 0.5f) || Physics.Raycast(transform.position, Vector3.right, 0.5f))
        {
            hasJumped = false;
        }
    }

    // Dodge function
    void Dodge()
    {
        Vector3 dodgeDirection = mainCamera.transform.forward;
        dodgeDirection.y = 0f;
        dodgeDirection.Normalize();

        rb.AddForce(dodgeDirection * dodgeDistance, ForceMode.Impulse);
    }
}