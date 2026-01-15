using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GasAreaExplosion : MonoBehaviour
{
    //public GameObject firePrefab;
    //public GameObject smokePrefab;

    //public GameObject[] explosionPrefab; //랜덤으로 터질 프리펩 넣을 배열
    //GameObject[] explosion;              //폭발의 위치조정을 위해 폭발 프리펩 저장할 배열

    //public int AttackCount = 20;//폭발 횟수
    //float explosionScale = 3f;  //폭발의 크기

    //float delay = 0.3f;//폭발간 시간
    //public float force = 8;   //물건을 날릴 파워
    //float range = 15;  //폭발 하여 물건을 날릴수 있는 범위


    ////랜덤위치를 계산하기 위한 변수
    //Vector3 center;
    //Vector3 randomPosition;
    //BoxCollider boxCollider;
    //Rigidbody rigidbody;
    //float minX;
    //float maxX;
    //float minY;
    //float maxY;
    //float minZ;
    //float maxZ;

    //private void Awake()
    //{
    //    explosion = new GameObject[AttackCount];
    //    boxCollider = transform.GetComponent<BoxCollider>();
    //    rigidbody = GetComponent<Rigidbody>();
    //    // 중심 위치
    //    center = new Vector3(0, 0, 0);

    //    // x, y, z 축 길이
    //    float xLength = boxCollider.size.x;
    //    float yLength = boxCollider.size.y;
    //    float zLength = boxCollider.size.z;
    //    // 범위 계산
    //    minX = center.x - (xLength / 2f);
    //    maxX = center.x + (xLength / 2f);
    //    minY = center.y - (yLength / 2f);
    //    maxY = center.y + (yLength / 2f);
    //    minZ = center.z - (zLength / 2f);
    //    maxZ = center.z + (zLength / 2f);
    //}

    //private void Start()
    //{
    //    StartCoroutine(explosionCo());
    //}
    //IEnumerator explosionCo()
    //{
    //    for (int i = 0; i < AttackCount; i++)
    //    {
    //        // 무작위 위치 생성
    //        float randomX = Random.Range(minX, maxX);
    //        float randomY = Random.Range(minY, maxY);
    //        float randomZ = Random.Range(minZ, maxZ);

    //        Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
    //        int explosionNum = Random.Range(0, explosionPrefab.Length-1);
    //        if (i == AttackCount - 1)
    //        {
    //            randomPosition = center;
    //            explosionScale = 10.0f;
    //            explosionNum = explosionPrefab.Length-1; //가장 마지막 큰 폭발 프리펩
    //            delay = 0.6f;
    //            force *= 5;
    //        }
    //        yield return new WaitForSeconds(delay);
    //        explosion[i] = Instantiate(explosionPrefab[explosionNum], transform);
    //        explosion[i].transform.localScale *= explosionScale;
    //        explosion[i].transform.position = boxCollider.transform.position + randomPosition;
            
    //        if (firePrefab != null)
    //        {
    //            Instantiate(firePrefab, explosion[i].transform);
    //        }
    //        if (smokePrefab != null)
    //        {
    //            Instantiate(smokePrefab, explosion[i].transform);                
    //        }

    //        RaycastHit[] rayHits = Physics.SphereCastAll(explosion[i].transform.position, 10, Vector3.up, 0, LayerMask.GetMask("Enemy"));
    //        foreach (RaycastHit hit in rayHits)
    //        {
    //            rigidbody = hit.collider.GetComponent<Rigidbody>();
    //            if (rigidbody != null)
    //            {
    //                rigidbody.AddExplosionForce(force, explosion[i].transform.position, 10, 3, ForceMode.Impulse);
    //            }
    //        }
    //    }
    //}
}
