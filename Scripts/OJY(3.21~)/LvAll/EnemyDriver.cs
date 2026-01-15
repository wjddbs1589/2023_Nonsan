using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDriver : MonoBehaviour
{
    CarHealth carHealth;
    private void Awake()
    {
        carHealth = transform.parent.GetComponent<CarHealth>();
    }
    private void Update()
    {

        if (carHealth.dead)
        {
            Destroy(gameObject);
        }
    }
}
