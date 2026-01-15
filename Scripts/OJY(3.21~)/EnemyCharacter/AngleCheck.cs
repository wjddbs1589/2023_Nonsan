using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class AngleCheck : MonoBehaviour
{
    [HideInInspector] public bool find = false;
    Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction.normalized * 30f);
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction*30, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 오브젝트가 나의 콜라이더인 경우
            if (hit.collider == myCollider)
            {
                find = true;
            }
            else
            {
                find = false;
            }
        }
        else
        {
            find = false;
        }
    }
}


