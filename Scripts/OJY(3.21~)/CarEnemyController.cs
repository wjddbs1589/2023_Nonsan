using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CarEnemyController : MonoBehaviour
{
    [SerializeField] private GameObject gun;           //본인
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject ForwardTarget;
    [HideInInspector]public AngleCheck angleCheck;
    public float rotateSpeed = 3f;
    float distance = 0;
    float maxDistance = 22.5f;
    public float angle = 0;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("MainCamera").gameObject;
        }

        foreach (Transform child in transform.parent)
        {
            angleCheck = child.GetComponent<AngleCheck>();            
        }
    }

    float nowTime = 0;
    private void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime >= 0.5f)
        {
            target = Camera.main.gameObject;
            nowTime = 0;
        }

        distance = Vector3.Distance(transform.position, target.transform.position);

        // 대상 오브젝트와 현재 오브젝트 사이의 방향 벡터를 구합니다.
        Vector3 direction = target.transform.position - transform.position;
        // 대상 오브젝트의 반대 방향 벡터를 구합니다.
        Vector3 oppositeDirection = -direction;

        if (distance <= maxDistance)
        {
            if (angleCheck.find)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(oppositeDirection), Time.deltaTime * rotateSpeed);
            }
        }
        else
        {
            LookForward(direction, oppositeDirection);
        }
    }

    void LookForward(Vector3 direction, Vector3 oppositeDirection)
    {
        direction = ForwardTarget.transform.position - transform.position;
        oppositeDirection = -direction;
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.LookRotation(oppositeDirection), Time.deltaTime * rotateSpeed); //정면응시
    }
}
