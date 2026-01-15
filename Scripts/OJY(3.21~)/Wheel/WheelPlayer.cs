using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelPlayer : MonoBehaviour
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
                Wheels[i].transform.Rotate(Vector3.right * 720.0f * Time.deltaTime);
            }
        }
    }
}
