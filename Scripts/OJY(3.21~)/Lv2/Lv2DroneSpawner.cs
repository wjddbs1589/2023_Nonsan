using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lv2DroneSpawner : MonoBehaviour
{
    public GameObject DronePrefab; //스폰될 드론 프리펩
    int MaxSpawn = 10;              //최대 소환 개수
    int droneCount = 0;            //스폰된 드론의 개수

    GameObject pathParent;
    CinemachineSmoothPath[] Path;

    int randPath;

    private void Awake()
    {
        pathParent = GameObject.Find("Path_Drone");
        Path = new CinemachineSmoothPath[pathParent.transform.childCount];

        for(int i = 0; i < pathParent.transform.childCount; i++)
        {
            Path[i] = pathParent.transform.GetChild(i).GetComponent<CinemachineSmoothPath>();
            Debug.Log(Path[i].name);
        }

        randPath = Random.Range(0, transform.childCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start"))
        {
            StartCoroutine(initateCo());
        }
        if (other.CompareTag("End"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator initateCo()
    {
        yield return new WaitForSeconds(1.0f);
        while (droneCount < MaxSpawn)
        {
            GameObject drone = Instantiate(DronePrefab);
            droneCount++;
        }
    }
}
