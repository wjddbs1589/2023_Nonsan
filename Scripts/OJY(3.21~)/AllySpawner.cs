using Cinemachine;
using System.Collections;
using System.Collections.Generic;
//00
//using TreeEditor;
using UnityEngine;

public class AllySpawner : MonoBehaviour
{
    CinemachineDollyCart[] cart = new CinemachineDollyCart[4];   //카트 정보
    CinemachineSmoothPath[] path = new CinemachineSmoothPath[4]; //경로 정보

    bool[] arrive = new bool[4];                  //모든 차량의 도착여부
    float remainLength = 0;                       //남은 거리 저장용 변수
    [HideInInspector]public int arriveCount = 0;  //도착한 차량의 숫자

    [SerializeField] GameObject rpgArmy;

    private void Awake()
    {
        for (int i = 0; i < 4; i++) 
        {
            cart[i] = transform.GetChild(i).GetComponent<CinemachineDollyCart>();
            path[i] = cart[i].m_Path.GetComponent<CinemachineSmoothPath>();
        }
        rpgArmy.SetActive(false);
    }
    private void Update()
    {
        for (int i = 0; i < cart.Length; i++)
        {
            if (!arrive[i])
            {
                remainLength = (path[i].PathLength - cart[i].m_Position) / cart[i].m_Speed;
                if (path[i].PathLength - cart[i].m_Position <= 0)
                {
                    arrive[i] = true;
                    arriveCount++;
                    if (arriveCount == 4)
                    {
                        StartCoroutine(SpawnRpgCo());
                        arriveCount = 0;
                    }
                }
            }
        }
    }
    IEnumerator SpawnRpgCo()
    {
        yield return new WaitForSeconds(1.0f);
        rpgArmy.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            transform.GetComponent<Collider>().enabled = false;
            foreach (CinemachineDollyCart dollycart in cart)
            {
                dollycart.m_Speed = 25.0f;
            }
        }
    }
}
