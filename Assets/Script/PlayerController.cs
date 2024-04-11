using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEngine.UI.GridLayoutGroup;
using Unity.Netcode.Components;

public class PlayerNetwork : NetworkBehaviour
{
    // Store movement direction locally
    private Vector3 moveDir = Vector3.zero;
    private float moveSpeed = 3f;
    private Animator animator;

    void Awake()
    {
        // ... existing code
        animator = GetComponent<Animator>();
    }

    private void UpdateAnimationState(Vector3 moveDir)
    {
        if (moveDir.magnitude == 0f) // Idle
        {
            animator.SetFloat("Walk", 0f); // Set animation parameter for Idle (adjust value if needed)
        }
        else if (moveDir.magnitude > moveSpeed) // sprint
        {
            animator.SetFloat("Walk", 2f); // Set animation parameter for Idle (adjust value if needed)
        }
        else
        {
            // Set animation based on dominant movement direction (forward/backward or sideways)
            animator.SetFloat("Walk", Mathf.Abs(moveDir.z) > Mathf.Abs(moveDir.x) ? Mathf.Abs(moveDir.z) : Mathf.Abs(moveDir.x));
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        // Get input only on the owner
        moveDir = Vector3.zero;

        // Check for horizontal (A/D) and vertical (W/S) movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Handle diagonal movement with magnitude adjustment for a 45-degree angle
        if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0)
        {
            moveDir = (transform.forward * Mathf.Sign(verticalInput) + transform.right * Mathf.Sign(horizontalInput)) * Mathf.Sqrt(0.5f);
        }
        else // Handle single direction movement
        {
            moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        }


        // Apply sprint speed multiplier only when sprinting
        moveDir *= (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0f) ? 3f : 1f; // Sprint only when W is pressed

        UpdateAnimationState(moveDir);

        // Send movement request to server (including move speed)
        MovementServerRpc(moveDir, moveSpeed);
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveDirection, float clientMoveSpeed)
    {
        // Authoritative movement logic on the server
        transform.position += moveDirection * clientMoveSpeed * Time.deltaTime;
    }
}
