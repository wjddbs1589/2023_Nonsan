using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllyArriveCheck : MonoBehaviour
{
    CinemachineDollyCart cart;
    CinemachineSmoothPath path;
    float distance;

    Animator anim;
    private void Awake()
    {
        cart = GetComponent<CinemachineDollyCart>();
        path = cart.m_Path.GetComponent<CinemachineSmoothPath>();
        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        distance = (path.PathLength - cart.m_Position) / cart.m_Speed;
        if (distance <= 0)
        {
            anim.SetBool("Run",false);
        }
    }

}
