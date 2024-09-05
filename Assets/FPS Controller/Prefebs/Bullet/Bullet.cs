using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float Speed;
    public ParticleSystem OnHitAffect;
    Rigidbody RB;
    public float Damage;
<<<<<<< HEAD
    public FPSCharacterManager[] NetObjects;
    public NetworkVariable<int> OwnerID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public AudioClip FireSound;

    void Awake()
    {
        RB = GetComponent<Rigidbody>();
        if (FireSound != null)
        {
            AudioSource.PlayClipAtPoint(FireSound, transform.position);
        }
=======
    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
    }

    void Start()
    {
<<<<<<< HEAD
        if (IsServer)
        {
            SetInitialVelocityServerRpc(transform.forward, Speed);
        }
    }

    [ServerRpc]
    private void SetInitialVelocityServerRpc(Vector3 direction, float speed)
    {
        // Apply velocity on the server
        RB.velocity = direction * speed;
        // Synchronize the velocity with clients
        SetInitialVelocityClientRpc(direction, speed);
    }

    [ClientRpc]
    private void SetInitialVelocityClientRpc(Vector3 direction, float speed)
    {
        // Apply velocity on the clients
        RB.velocity = direction * speed;
=======
        RB.velocity = transform.forward * Speed;
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
    }

    private void OnCollisionEnter(Collision collision)
    {
<<<<<<< HEAD
        print("Hit to : " + collision.transform.name);

        if (collision.transform.tag == "Bullet")
            print("<color=red>Caution: Hit to A Bullet!!!");

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if (IsServer)
            {
                print("Damage");
                collision.gameObject.GetComponent<Health>().TakeDamageServerRpc(Damage);

                if (collision.gameObject.GetComponent<Health>().CurrentHealth.Value <= 0 && !collision.gameObject.GetComponent<Health>().isDeadCounted)
                {
                    print("Eliminated!!");
                    collision.gameObject.GetComponent<Health>().isDeadCounted = true;
                    AddEliminationServerRpc(OwnerID.Value);
                }
=======

        print("Hit to : " + collision.transform.name);

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if (IsOwner)
            {
                print("Damage");
                collision.gameObject.GetComponent<Health>().TakeDamageServerRpc(Damage);
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
            }
        }
        else
        {
            print("Health Didn't found");
        }

<<<<<<< HEAD
        if (OnHitAffect != null)
=======
        if(OnHitAffect != null)
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
        {
            Instantiate(OnHitAffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

<<<<<<< HEAD


=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
        if (IsHost)
        {
            DestroyBulletClientRpc();
            Destroy(this.gameObject);
        }
    }

<<<<<<< HEAD
    [ServerRpc]
    private void AddEliminationServerRpc(int ownerId)
    {
        FPSCharacterManager[] objs = FindObjectsOfType<FPSCharacterManager>();
        foreach (var obj in objs)
        {
            if (obj.OwnerClientId == (ulong)ownerId)
            {
                obj.AddEliminations(1);
            }
        }
    }

=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
    [ClientRpc]
    private void DestroyBulletClientRpc()
    {
        Destroy(this.gameObject);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
