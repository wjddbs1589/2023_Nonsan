using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPosition : MonoBehaviour
{
    public GameObject explosionEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //GameObject explo = Instantiate(explosionEffect, transform);
            //explo.transform.parent = null;            
            //Destroy(gameObject);
        }
    }
}
