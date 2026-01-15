using Cinemachine;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLookatChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DroneHealth health = other.transform.GetComponentInParent<DroneHealth>();
        if (health != null)
        {
            health.see = true;
            health.seeTime = 0.5f;
            Destroy(gameObject, 20);
        }
        
    }
}
