using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Mathematics;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class Health : NetworkBehaviour
{
    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(100f);
    [HideInInspector] public bool isDead;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CurrentHealth.OnValueChanged += GetComponent<FPSCharacterManager>().HealthValueChaged;
    }

    void HealthChecks()
    {
        if(CurrentHealth.Value <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (CurrentHealth.Value <= 0)
        {
            GetComponent<FPSCharacterManager>().DIsableAllWeapons();
        }

        if (!IsOwner)
            return;

        HealthChecks();
    }

    void Die()
    {
        isDead = true;
        if (GetComponent<FPSCharacterManager>() != null)
        {
            GetComponent<FPSCharacterManager>().Refrences.CharcaterAniamtor.SetInteger("WeaponType_int", 0);
            GetComponent<FPSCharacterManager>().Refrences.CharcaterAniamtor.SetBool("Death_b", true);
            GetComponent<FPSCharacterManager>().DIsableAllWeapons();
            HideCharacterServerRPC();
            GetComponent<FPSCharacterManager>().enabled = false;
        }

        if (GetComponent<CameraMovement>() != null)
            GetComponent<CameraMovement>().enabled = false;

        if (GetComponent<PlayerController>() != null)
        {
            GetComponent<PlayerController>().enabled = false;
        }

        if (GetComponent<ReSpawnHandler>() != null)
            GetComponent<ReSpawnHandler>().RespawnInProcess = true;
    }

    [ServerRpc(RequireOwnership = false)]
    void HideCharacterServerRPC()
    {
        GetComponent<FPSCharacterManager>().DIsableAllWeapons();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        CurrentHealth.Value -= damage;
        if (CurrentHealth.Value <= 0)
        {
            Die();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ShowCharacterServerRPC()
    {
        GetComponent<FPSCharacterManager>().GrabWeapon(0);
    }
}