using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float lifeTime=3f;
    void Start()
    {
        Destroy(this.gameObject,lifeTime);
    }

    
}
