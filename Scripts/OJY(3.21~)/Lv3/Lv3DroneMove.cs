using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv3DroneMove : MonoBehaviour
{
    GameObject player;                       //드론이 바라볼 대상
    [SerializeField]GameObject Destination;  //목적지들을 가진 오브젝트
    [SerializeField]Transform[] Destinations;//목적지들이 있는 배열
    Lv3DroneSpawner Spawner;

    int positionCount;
    private void Awake()
    {
        player = GameObject.Find("PlayerCar");        //드론이 바라볼 대상
        Destination = GameObject.Find("Lv3DroneDestination");//드론의 목적지
        Spawner = GameObject.Find("Lv3DroneSpawner").GetComponent<Lv3DroneSpawner>();

        Destinations = new Transform[Destination.transform.childCount];
        for (int i = 0; i < Destinations.Length; i++)
        {
            Destinations[i] = Destination.transform.GetChild(i);
        }

        while (true)
        {
            positionCount = Random.Range(0, Destinations.Length);
            //if (Spawner.UsingPosition[positionCount]==false)
            //{
            //    Spawner.UsingPosition[positionCount] = true;
            //    break;
            //}
        }
    }

    private void Update()
    {
        if (Spawner == null)
        {            
            StartCoroutine(destroyCo());
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Destinations[positionCount].position, 40 * Time.deltaTime);
        }        
        transform.DOLookAt(player.transform.position, .5f);
    }

    public void destroy()
    {
        if (Spawner != null)
        {
            //Spawner.UsingPosition[positionCount] = false;
            //Spawner.respawn();
        }
        Destroy(gameObject);
    }
    IEnumerator destroyCo()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
