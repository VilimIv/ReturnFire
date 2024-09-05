using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Grenade : NetworkBehaviour
{
<<<<<<< HEAD
    public AudioClip ExoplosionSound;
    public ParticleSystem ExplosionAffect;

=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
    public float Range = 6f;
    public float Delay = 5f;
    public float Damage = 100f;

<<<<<<< HEAD
    public float Timer;

    public NetworkVariable<int> OwnerID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    void Start()
    {

    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if(Timer > Delay)
        {
            Explode();
            Timer = 0;
        }
    }
    void Explode()
    {
=======
    void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(Delay);
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
        Collider[] Colliders = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider collider in Colliders)
        {
            if(collider.transform.gameObject.GetComponent<Health>() != null)
            {
<<<<<<< HEAD
                if (IsOwner)
                {
                    collider.transform.gameObject.GetComponent<Health>().TakeDamageServerRpc(Damage);
                }

                if (collider.gameObject.GetComponent<Health>().CurrentHealth.Value <= 0 && !collider.gameObject.GetComponent<Health>().isDeadCounted)
                {
                    print("Eliminated!!");
                    FPSCharacterManager[] Objs = FindObjectsOfType<FPSCharacterManager>();
                    if (Objs.Length > 0)
                    {
                        for (int i = 0; i < Objs.Length; i++)
                        {
                            if (Objs[i].OwnerClientId.ToString() == OwnerID.Value.ToString())
                            {
                                Objs[i].gameObject.GetComponent<FPSCharacterManager>().AddEliminations(1);
                            }
                        }
                    }
                    collider.gameObject.GetComponent<Health>().isDeadCounted = true;
=======
                if (collider.transform.gameObject.GetComponent<Health>().IsOwner)
                {
                    collider.transform.gameObject.GetComponent<Health>().CurrentHealth.Value -= Damage;
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
                }
            }
        }

<<<<<<< HEAD
        if(ExplosionAffect != null)
        {
            Instantiate(ExplosionAffect, transform.position, Quaternion.identity);
        }

        if(ExoplosionSound != null)
        {
            AudioSource.PlayClipAtPoint(ExoplosionSound, transform.position);
        }

=======
>>>>>>> 789a03c02e18d62d16445d7bad6b050633dcd5fb
       gameObject.SetActive(false);
    }

    [ClientRpc]
    private void DestroyBulletClientRpc()
    {
        Destroy(this.gameObject);
    }
}