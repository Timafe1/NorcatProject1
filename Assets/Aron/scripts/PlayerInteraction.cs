using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    public GameObject interactionTextPanel;
    private bool interacting;
    private GameObject currentInteractableObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interacting)
            {
                // If already interacting, stop interaction
                interacting = false;
                interactionTextPanel.SetActive(false);
            }
            else if (currentInteractableObject != null)
            {
                // Start interaction
                interacting = true;
                interactionTextPanel.SetActive(true);
            }
        }

        if (interacting)
        {
            // Update the position of the text panel to follow the current interactable object
            Vector3 position = Camera.main.WorldToScreenPoint(currentInteractableObject.transform.position);
            interactionTextPanel.transform.position = position;
        }
    }

    void FixedUpdate()
    {
        // Check for interactable objects within range
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (Collider collider in interactables)
        {
            if (collider.gameObject.CompareTag("Character"))
            {
                currentInteractableObject = collider.gameObject;
                return;
            }
        }
        currentInteractableObject = null;
    }
}
