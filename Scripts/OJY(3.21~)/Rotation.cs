using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public EnemyHelicopter heli;
    public GameObject effectPrefab; // 이펙트 프리팹을 할당할 변수

    private void OnTriggerEnter(Collider other)
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Awake()
    {
        if (heli == null)
        {
            heli = GetComponentInParent<EnemyHelicopter>();
        }
    }
    void Update()
    {
        if (!heli.dead)
        {
            transform.Rotate(Vector3.up * 1800.0f * Time.deltaTime);
        }
    }
}
