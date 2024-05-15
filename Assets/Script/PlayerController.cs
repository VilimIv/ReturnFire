using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 3f;

    void Start()
    {
        // Get the Rigidbody component if not assigned in the inspector
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Freeze rotation to prevent unwanted rotation
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            // Get input only for the local player
            Vector3 moveDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDir = transform.forward;
            if (Input.GetKey(KeyCode.S)) moveDir = -transform.forward;
            if (Input.GetKey(KeyCode.A)) moveDir = -transform.right;
            if (Input.GetKey(KeyCode.D)) moveDir = transform.right;

            // Send the movement command to the server
            MovePlayerServerRpc(moveDir);
        }
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector3 moveDirection)
    {
        // Call a method to apply movement on the server
        MovePlayer(moveDirection);
        // Send the movement command to all clients
        MovePlayerClientRpc(moveDirection);
    }

    [ClientRpc]
    private void MovePlayerClientRpc(Vector3 moveDirection)
    {
        // Call a method to apply movement on all clients
        MovePlayer(moveDirection);
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        // Apply force to the Rigidbody in the moveDirection
        rb.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);
    }
}
