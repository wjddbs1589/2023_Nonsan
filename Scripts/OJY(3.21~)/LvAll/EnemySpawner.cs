using EasyRoads3Dv3;               
using EnemyAI;                     
using System.Collections;          
using System.Collections.Generic;  
using System.Linq;                 
using Unity.VisualScripting;       
using UnityEngine;                 
using UnityEngine.AI;              

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab = new GameObject[9]; //적군 프리펩 저장할 배열
    int spawnCount = 0;   //스폰할 유닛 수
    [SerializeField] int maxSpawnCount = 5;//스폰 가능한 유닛 수 

    public Transform wayPoints; //웨이포인트들 저장       
    Transform[] wayPoint;       //스포너 근처 웨이포인트 지정

    List<int> numberList;
    int[] selectedNum;
    
    private void Awake()
    {
        wayPoint = new Transform[wayPoints.childCount]; //배열 초기화

        //이동할 위치들을 배열에 저장
        wayPoint = new Transform[wayPoints.childCount];
        for (int i = 0;i<wayPoints.childCount;i++)           //웨이포인트
        {
            wayPoint[i] = wayPoints.GetChild(i).transform;
        }

        //리스트를 이용해 이동할 위치를 중복없이 랜덤으로 지정하기
        numberList = Enumerable.Range(0, wayPoints.childCount).ToList(); //0부터 웨이포인트 갯수만큼 리스트 생성
        selectedNum = new int[wayPoints.childCount];
        for (int i = 0; i < wayPoints.childCount; i++)
        {
            int index = Random.Range(0, numberList.Count);
            selectedNum[i] = numberList[index];
            numberList.RemoveAt(index);           
        }

        SpawnObject();
    }

    /// <summary>
    /// 적 유닛 스폰하는 함수
    /// </summary>
    private void SpawnObject()
    {
        while (spawnCount < maxSpawnCount)
        {
            //프리펩 0~8번중 뽑힌것을 생성
            int RandNum = Random.Range(0, enemyPrefab.Length);
            GameObject EnemyObject = Instantiate(enemyPrefab[RandNum], transform.position, Quaternion.identity);
            EnemyObject.transform.parent = transform;
            
            EnemyController enemyCon = EnemyObject.GetComponent<EnemyController>();//생성된 유닛의 웨이포인트와 도망지점 설정            
            enemyCon.targetDestination = wayPoint[selectedNum[spawnCount]];        //이동할 웨이포인트 설정

            NavMeshAgent nav = EnemyObject.GetComponent<NavMeshAgent>();
            nav.SetDestination(enemyCon.targetDestination.position); //네브메쉬의 목적지 설정
            spawnCount++;
        }        
    }
}
