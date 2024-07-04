using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float Speed;
    public ParticleSystem OnHitAffect;
    Rigidbody RB;
    public float Damage;
    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        RB.velocity = transform.forward * Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        print("Hit to : " + collision.transform.name);

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if (IsOwner)
            {
                print("Damage");
                collision.gameObject.GetComponent<Health>().TakeDamageServerRpc(Damage);
            }
        }
        else
        {
            print("Health Didn't found");
        }

        if(OnHitAffect != null)
        {
            Instantiate(OnHitAffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

        if (IsHost)
        {
            DestroyBulletClientRpc();
            Destroy(this.gameObject);
        }
    }

    [ClientRpc]
    private void DestroyBulletClientRpc()
    {
        Destroy(this.gameObject);
    }
}