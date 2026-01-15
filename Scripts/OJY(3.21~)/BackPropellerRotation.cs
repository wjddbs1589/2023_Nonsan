using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPropellerRotation : MonoBehaviour
{
    public EnemyHelicopter heli;
    float rotateSpeed = 1080;
    public GameObject effectPrefab; // 이펙트 프리팹을 할당할 변수

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 지점에 이펙트 프리팹 생성
        Instantiate(effectPrefab, transform.position, Quaternion.identity);
    }

    private void Awake()
    {
        heli = GetComponentInParent<EnemyHelicopter>();
    }
    void Update()
    {
        if (!heli.dead)
        {
            transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
        }
        else
        {
            rotateSpeed = Mathf.Lerp(1800, 0, 1.5f * Time.deltaTime);
        }
    }
}
