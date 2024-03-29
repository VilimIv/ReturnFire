using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    // Store movement direction locally
    private Vector3 moveDir = Vector3.zero;

    void Update()
    {
        if (!IsOwner) return;

        // Get input only on the owner
        moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = +1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = -1f;

        // Send movement request to server
        MovementServerRpc(moveDir);
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 moveDirection)
    {
            float moveSpeed = 3f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}