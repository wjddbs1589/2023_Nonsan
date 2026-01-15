using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelOfGuard : MonoBehaviour
{
    GameObject[] Wheels = new GameObject[4];
    public CinemachineDollyCart cart;
    CinemachineSmoothPath path;
    private void Awake()
    {
        if (cart ==null)
        {
            cart = transform.parent.parent.GetComponent<CinemachineDollyCart>();
        }
        path = cart.m_Path.GetComponent<CinemachineSmoothPath>();
        for (int i = 0; i < Wheels.Length; i++)
        {
            Wheels[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        for (int i = 0; i < Wheels.Length; i++)
        {
            if (Wheels[i] != null)
            {
                Wheels[i].transform.Rotate(Vector3.right * 720f * Time.deltaTime);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrashArea") || other.CompareTag("StopPoint"))
        {
            cart.m_Speed = 0.0f;
        }
    }

}
