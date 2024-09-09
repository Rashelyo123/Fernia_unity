using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grab : MonoBehaviour
{
    public float raycastDistance = 2f; // The distance of the raycast in front of the character
    public Transform movableObject = null; // This will be set when a movable object is detected
    public KeyCode grabKey = KeyCode.X; // The key to grab the object
    private bool isGrabbing = false; // Whether the object is currently being grabbed
    private Vector3 grabOffset; // The initial offset when grabbing the object
    private Quaternion initialFacingDirection; // The facing direction when grabbing the object
    

    void Update()
    {
        if (Input.GetKeyDown(grabKey) && movableObject == null)
        {
            // Cast a ray forward from the character
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.forward * raycastDistance, Color.green);
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Movable")) // Check if it's a movable object (add a Movable tag to objects)
                {
                    movableObject = hit.collider.transform;
                    grabOffset = movableObject.position - transform.position; // Calculate offset
                    initialFacingDirection = transform.rotation; // Save the initial facing direction
                    isGrabbing = true;
                }
            }
        }

        if (Input.GetKeyUp(grabKey) && isGrabbing)
        {
            // Stop grabbing
            isGrabbing = false;
            movableObject = null;
        }

        if (isGrabbing && movableObject != null)
        {
            // Lock the character's facing direction
            transform.rotation = initialFacingDirection;

            // Move the object in front of the character based on the offset
            Vector3 targetPosition = transform.position + transform.forward * grabOffset.magnitude;
            movableObject.position = targetPosition;
        }
    }
}
