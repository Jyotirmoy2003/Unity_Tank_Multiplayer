using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId) //setting the owner so the projectile don't hit its owner
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody == null) { return; }

        if (col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (ownerClientId == netObj.OwnerClientId) //if owner then ignore
            {
                return;
            }
        }

        if (col.attachedRigidbody.TryGetComponent<Health>(out Health health)) //if damageable then damage
        {
            health.TakeDamage(damage);
        }
    }

}
