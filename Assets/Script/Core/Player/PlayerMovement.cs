using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Reference")]
   [SerializeField] InputReader inputReader;
   [SerializeField] Transform tankBodyTransform;
   [SerializeField] Rigidbody2D rb;
   [Header("Settings")]
   [SerializeField] float movementSpeed=4f;
   [SerializeField] float turningRate=270f;

   private Vector2 priviousMovementInput;


    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        inputReader.MoveEvent+=HandleMove; //subcribe to Move Event
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.MoveEvent-=HandleMove; //UnSubcribe To Move Event
    }



    void Update()
    {
        if(!IsOwner) return;

        tankBodyTransform.Rotate(0f,0f,(priviousMovementInput.x*-turningRate*Time.deltaTime));
    }
    void FixedUpdate()
    {
        if(!IsOwner) return;
        rb.velocity=(Vector2)tankBodyTransform.up*movementSpeed*priviousMovementInput.y;
    }
    void HandleMove(Vector2 movementInput)
    {
        priviousMovementInput=movementInput;
    }
}
