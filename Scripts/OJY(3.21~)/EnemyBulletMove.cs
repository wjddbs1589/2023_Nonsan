using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyBulletMove : MonoBehaviour
{
    public GameObject mainCamera;       //총알 타겟
    public float speed = 10.0f;  //총알속도

    float offsetX; //거리 범위 값
    float offsetY; //높낮이 범위 값
    float offsetZ; //폭 범위 값
    Vector3 offset = Vector3.zero;
    private void Awake()
    {
        mainCamera = GameObject.Find("MainCamera");
        offsetX = Random.Range(4f, 5f);     //화면에 딱 맞는 범위 : 3 ~ 4
        offsetY = Random.Range(-.5f,0.5f);  //화면에 딱 맞는 범위 : ~0.5 ~ 0.5
        offsetZ = Random.Range(-2f, 0f);    //화면에 딱 맞는 범위 : -2 ~ 0
        StartCoroutine(DestroyCo());
        offset = new Vector3(offsetX, offsetY, offsetZ);
    }
    private void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, mainCamera.transform.position + new Vector3(offsetX, offsetY, offsetZ), speed * Time.deltaTime); // + offset, speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position + offset);
        if (distance >= 0.1)
        {
            transform.LookAt(mainCamera.transform.position + offset);
        }
        else
        {
            Destroy(gameObject);
        }        
    }
    IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
