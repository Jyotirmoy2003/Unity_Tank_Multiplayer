using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;


public class ProjectileLuncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader inputReader;
    [SerializeField] Transform projectileSpawnPosition;
    [SerializeField] GameObject serverProjectile;
    [SerializeField] GameObject clientProjectile;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] private CoinWallet wallet;
  
    [Header("Settings")]
    [SerializeField] float projectileSpeed=2f;
    [SerializeField] float firerate=1f;
    [SerializeField] float muzzleFlashDuration=0.2f;
    [SerializeField] private int costToFire;


    private bool shouldFire=false;
    private float timer,muzzleFlashTimer;
    
    #region NetworkSpane
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){return;}
        inputReader.PrimaryFireEvent+=HandleInput;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner){ return; }
        inputReader.PrimaryFireEvent-=HandleInput;
    }
    #endregion



    void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }
      
        if(!IsOwner) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if(!shouldFire) return;

        if (timer > 0) { return; }

        if (wallet.TotalCoins.Value < costToFire) { return; }


        PrimaryfireServerRPC(projectileSpawnPosition.position,projectileSpawnPosition.up);
        SpawnDummyProjectile(projectileSpawnPosition.position,projectileSpawnPosition.up);

        timer = 1 / firerate;
    }



#region  RPC

    [ServerRpc]
    private void PrimaryfireServerRPC(Vector3 spawnPos,Vector3 direction)
    {
        if (wallet.TotalCoins.Value < costToFire) { return; }
        wallet.SpendCoins(costToFire);

        GameObject projectileGameObject=Instantiate(serverProjectile,spawnPos,Quaternion.identity);
       projectileGameObject.transform.up=direction;

       Physics2D.IgnoreCollision(playerCollider, projectileGameObject.GetComponent<Collider2D>());

       if (projectileGameObject.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }


       if (projectileGameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

       SpawnDummyProjectileClientRPC(spawnPos,direction);
    }
    [ClientRpc]
    private void SpawnDummyProjectileClientRPC(Vector3 spawnPos,Vector3 direction)
    {
        if(IsOwner) return;
        SpawnDummyProjectile(spawnPos,direction);
    }


#endregion





    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
       GameObject projectileGameObject=Instantiate(clientProjectile,spawnPos,Quaternion.identity);
       projectileGameObject.transform.up=direction;

        //muzzle flash 
       muzzleFlash.SetActive(true);
       muzzleFlashTimer=muzzleFlashDuration;

       Physics2D.IgnoreCollision(playerCollider, projectileGameObject.GetComponent<Collider2D>());

       if (projectileGameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

    }

    void HandleInput(bool val)
    {
        shouldFire=val;
    }
}
