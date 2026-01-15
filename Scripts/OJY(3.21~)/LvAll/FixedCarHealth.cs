using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FixedCarHealth : HealthManager
{
    // 이 클래스를 가진 오브젝트(차량)가 충돌 경로를 생성하여 움직임
    [Tooltip("차량 폭파시 발생할 폭발 이펙트 프리펩")]
    public GameObject explostionPrefab;
    [Tooltip("차량 피격시 발생할 연기 이펙트 프리펩")]
    public GameObject smokePrefab;
    GameObject smoke;
    float HpPercentForSmoke = 0.5f;

    [Tooltip("현재 NPC체력.")]
    public float health = 500f;
    private float totalHealth;

    private Rigidbody rigid;

    private void Awake()
    {
        totalHealth = health;
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (health <= totalHealth * HpPercentForSmoke)
        {
            smoke.transform.position = transform.position;
        }
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        if (!dead)
        {
            health -= damage;
            if(smoke==null && health <= totalHealth * HpPercentForSmoke)
            {
                smoke = Instantiate(smokePrefab);
            }
            if (!dead)
            {
                GameManager.Inst.Score[playerNum]++;
                Kill();
            }
        }        
    }

    private void Kill()
    {
        dead = true;
        StartCoroutine(explosionCo());
    }
    
    IEnumerator explosionCo()
    {
        GameObject explosion = Instantiate(explostionPrefab, transform);
        explosion.transform.localScale *= 1.2f;
        yield return new WaitForSeconds(5.0f);
        Destroy(smoke);
        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Vector3 CrashDir = transform.position - other.transform.position.normalized;
    //        transform.GetComponent<Rigidbody>().AddForce(new Vector3(-2,1,-1) * 5f, ForceMode.Impulse);
    //    }
    //}
}
