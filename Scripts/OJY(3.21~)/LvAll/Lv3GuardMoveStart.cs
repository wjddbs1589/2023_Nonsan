using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Lv3GuardMoveStart : MonoBehaviour
{
    GameObject[] GuardCar = new GameObject[2]; //이동시킬 차량의 개수

    [Header("출발속도 설정")]
    public float SetSpeed = 18.0f;
    //--------------------
    float speed = 0;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            StartEffect?.Invoke();
        }
    }
    public Action StartEffect;
    //--------------------
    private void Awake()
    {
        for (int i = 0; i < GuardCar.Length; i++)
        {
            GuardCar[i] = transform.GetChild(i).gameObject;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject car in GuardCar)
            {
                Speed = SetSpeed;
                car.gameObject.transform.GetComponent<CinemachineDollyCart>().m_Speed = Speed;
            }
        }
    }

}
