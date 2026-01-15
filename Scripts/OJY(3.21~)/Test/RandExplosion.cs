using Cinemachine.Utility;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandExplosion : MonoBehaviour
{
    [Tooltip("폭발 이펙트 프리펩 저장 배열")]
    public GameObject[] explosionPrefab = new GameObject[15];
    GameObject[] explosion;
    int explosionNum;
    public int explosionCount = 3;
    float explosionScale = 1.0f;

    Vector3 center;
    BoxCollider boxCollider;
    float minX;
    float maxX;
    float minY;
    float maxY;
    float minZ;
    float maxZ;

    CarHealth health;
    private void Awake()
    {
        explosion = new GameObject[explosionPrefab.Length];
        boxCollider = transform.parent.GetComponent<BoxCollider>();

        // 직사각형의 중심 위치
        center = new Vector3(0, 0, 0);

        // 직사각형의 x, y, z 축 길이
        float xLength = boxCollider.size.x;
        float yLength = boxCollider.size.y;
        float zLength = boxCollider.size.z;

        // 직사각형의 범위 계산
        minX = center.x - (xLength / 2f);
        maxX = center.x + (xLength / 2f);
        minY = center.y - (yLength / 2f);
        maxY = center.y + (yLength / 2f);
        minZ = center.z - (zLength / 2f);
        maxZ = center.z + (zLength / 2f);

        health = transform.parent.GetComponent<CarHealth>();
    }
    public void explosionFunc(float health)
    {
        if (health <= 0)
        {
            StartCoroutine(explosionCo());
        }
    }
    IEnumerator explosionCo()
    {
        for (int i = 0; i < explosionCount; i++)
        {
            // 무작위 위치 생성
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomZ = Random.Range(minZ, maxZ);

            // 무작위 위치 설정
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
        
            if(i == explosionCount - 1)
            {
                //마지막 폭발은 가장 큰것으로 설정
                explosionNum = i;
                explosionScale = 2f;
            }
            else
            {
                explosionNum = Random.Range(0, explosionPrefab.Length);
                explosionScale = 1f;
            }
            explosion[i] = Instantiate(explosionPrefab[explosionNum]);
            explosion[i].transform.localScale *= explosionScale;
            
            // 폭발 위치 조정. 차량위치 + 차량 크기범위 내의 랜덤위치
            explosion[i].transform.position = transform.parent.position + randomPosition;
            yield return new WaitForSeconds(0.1f);
        }       
    }
}
