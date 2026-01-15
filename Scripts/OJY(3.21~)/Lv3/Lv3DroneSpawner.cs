using Cinemachine;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3DroneSpawner : MonoBehaviour
{
    public GameObject DronePrefab; //스폰될 드론 프리펩
    public int MaxSpawn = 0;      //최대 소환 개수

    GameObject pathObj;
    CinemachineSmoothPath[] path;  //드론이 스폰될 위치
    CinemachineDollyCart cart;
    public float speed = 23.0f;

    List<int> spawnNumList;
    int randPos;

    private void Awake()
    {
        pathObj = GameObject.Find("Path_Drone").gameObject;
        path = new CinemachineSmoothPath[pathObj.transform.childCount];
        MaxSpawn = pathObj.transform.childCount;
        for (int i = 0; i < pathObj.transform.childCount; i++)
        {
            path[i] = pathObj.transform.GetChild(i).GetComponent<CinemachineSmoothPath>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetComponent<Collider>().enabled = false;
            Spawn();
        }
    }
    
    void Spawn()
    {
        spawnNumList = new List<int>();
        for (int i = 0; i < MaxSpawn; i++) 
        {
            GameObject drone = Instantiate(DronePrefab);
            cart = drone.transform.GetComponent<CinemachineDollyCart>();

            do
            {
                randPos = Random.Range(0, MaxSpawn);
            }
            while (spawnNumList.Contains(randPos));
            spawnNumList.Add(randPos);

            cart.m_Path = path[randPos];
            cart.m_Speed = speed;
        }
    }

}
