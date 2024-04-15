using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAim : NetworkBehaviour
{
   [SerializeField] InputReader inputReader;
   [SerializeField] Transform tankTurretTransform;



   void LateUpdate()
   {
        if(!IsOwner) return;

        Vector2 aimPosition= inputReader.AimPositon;
        Vector2 aimWorldPosition= Camera.main.ScreenToWorldPoint(aimPosition);

        tankTurretTransform.up=new Vector2(
            aimWorldPosition.x-tankTurretTransform.position.x,
            aimWorldPosition.y-tankTurretTransform.position.y);
   }
}
