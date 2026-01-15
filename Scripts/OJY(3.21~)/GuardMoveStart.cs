using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMoveStart : MonoBehaviour
{
    GameObject[] GuardCar;

    [Header("출발속도 설정")]
    public float SetSpeed = 15.0f;

    private void Awake()
    {
        GuardCar = new GameObject[transform.childCount];
        for (int i = 0; i < GuardCar.Length; i++)
        {
            GuardCar[i] = transform.GetChild(i).gameObject;
        }       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject car in GuardCar)
            {
                CinemachineDollyCart cart = car.gameObject.transform.GetComponent<CinemachineDollyCart>();
                if (cart != null)
                {
                    car.gameObject.transform.GetComponent<CinemachineDollyCart>().m_Speed = SetSpeed;
                }                
            }
        }
    }

}
