using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class velocityTest : MonoBehaviour
{
    Rigidbody rb;
    //Vector3 velocity;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.forward * 50;

    }

    public float force = 3.0f;
    void OnCollisionEnter(Collision collision)
    {
        Vector3 dir = transform.position - collision.transform.position.normalized;
        Debug.Log(dir);
        rb.AddForce(dir * force, ForceMode.Impulse);
    }



    void MoveTarget(GameObject target, Vector3 direction)
    {
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
