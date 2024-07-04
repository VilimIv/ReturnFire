using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Grenade : NetworkBehaviour
{
    public float Range = 6f;
    public float Delay = 5f;
    public float Damage = 100f;

    void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(Delay);
        Collider[] Colliders = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider collider in Colliders)
        {
            if(collider.transform.gameObject.GetComponent<Health>() != null)
            {
                if (collider.transform.gameObject.GetComponent<Health>().IsOwner)
                {
                    collider.transform.gameObject.GetComponent<Health>().CurrentHealth.Value -= Damage;
                }
            }
        }

       gameObject.SetActive(false);
    }

    [ClientRpc]
    private void DestroyBulletClientRpc()
    {
        Destroy(this.gameObject);
    }
}