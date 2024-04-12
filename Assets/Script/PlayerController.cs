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
        moveDir *= (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0f) ? 3*moveSpeed : moveSpeed; // Sprint only when W is pressed

        UpdateAnimationStateServerRpc(moveDir);

        // Send movement request to server (including move speed)
        MovementServerRpc(moveDir, moveSpeed);
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveDirection, float clientMoveSpeed)
    {
        // Authoritative movement logic on the server
        transform.position += moveDirection * clientMoveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void UpdateAnimationStateServerRpc(Vector3 moveDir)
    {
        if (moveDir.magnitude == 0f) // Idle
        {
            animator.SetFloat("Walk", 0f);
        }
        else
        {
            float forwardValue = Vector3.Dot(moveDir, transform.forward);
            float rightValue = Vector3.Dot(moveDir, transform.right);

            if (forwardValue < 0f) // Walking backwards
            {
                animator.SetFloat("Walk", -1f);
            }
            else if (forwardValue > 0f) // Walking forwards
            {
                if (Input.GetKey(KeyCode.LeftShift)) // Sprinting
                {
                    animator.SetFloat("Walk", 2f);
                }
                else // Walking normally
                {
                    animator.SetFloat("Walk", 1f);
                }
            }
            else if (Mathf.Abs(rightValue) > 0f) // Walking sideways
            {
                animator.SetFloat("Walk", 1f);
            }
        }
    }



}
