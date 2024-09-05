using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Mathematics;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
<<<<<<< HEAD
using static UnityEngine.UI.GridLayoutGroup;
=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb

public class Health : NetworkBehaviour
{
    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(100f);
    [HideInInspector] public bool isDead;
<<<<<<< HEAD
    public bool isDeadCounted;
=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb

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
<<<<<<< HEAD
        else
        {
            isDeadCounted = false;
        }
=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb

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
<<<<<<< HEAD
        {
            //GetComponent<ReSpawnHandler>().CountdownTimer = GetComponent<ReSpawnHandler>().RespwanTime;
            GetComponent<ReSpawnHandler>().RespawnInProcess = true;
        }
=======
            GetComponent<ReSpawnHandler>().RespawnInProcess = true;
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
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