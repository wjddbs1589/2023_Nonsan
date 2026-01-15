using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAnimationController : MonoBehaviour
{
    int allyCount;
    Animator[] anims;
    CinemachineDollyCart[] carts;
    private void Awake()
    {
        allyCount = transform.childCount;
        anims = new Animator[allyCount];
        carts = new CinemachineDollyCart[allyCount];
        for (int i = 0;i<allyCount;i++)
        {
            anims[i] = transform.GetChild(i).GetComponent<Animator>();
            carts[i] = transform.GetChild(i).GetComponent<CinemachineDollyCart>();
        }
    }

    void OnRunAnim()
    {
        for (int i = 0;i<allyCount;i++)
        {
            anims[i].SetBool("Run", true);
            carts[i].m_Speed = 1f;
        }
    }
}
