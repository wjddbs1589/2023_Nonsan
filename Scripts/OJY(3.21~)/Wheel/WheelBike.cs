using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBike : MonoBehaviour
{
    GameObject[] Wheels = new GameObject[4];
    CinemachineDollyCart DollyCart;
    float speed = 15.0f;
    private void Awake()
    {
        DollyCart = transform.parent.GetComponent<CinemachineDollyCart>();
        for (int i = 0; i < Wheels.Length; i++)
        {
            Wheels[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        speed = DollyCart.m_Speed;
        if (speed > 0.0f)
        {
            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i].transform.Rotate(Vector3.right * 1800.0f * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "StopPoint")
        {
            DollyCart.m_Speed = 0.0f;
        }
    }
}
