using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1MoveCarHealth : HealthManager
{
    // 이 클래스를 가진 오브젝트(차량)가 충돌 경로를 생성하여 움직임
    [Tooltip("차량 폭파시 발생할 폭발 이펙트 프리펩")]
    public GameObject explostionPrefab;
    [Tooltip("차량 피격시 발생할 연기 이펙트 프리펩")]
    public GameObject smokePrefab;
    GameObject smoke;

    //public GameObject smokeTailPrefab;
    //GameObject smokeTail;

    float HpPercentForSmoke = 0.5f;

    [SerializeField]CinemachineDollyCart cart;
    [SerializeField]CinemachineSmoothPath path;
    float timer;

    [Tooltip("현재 NPC체력.")]
    public float health = 500f;
    private float totalHealth;

    float launchForce;
    private Rigidbody rigid;

    private void Awake()
    {
        totalHealth = health;
        cart = transform.GetComponent<CinemachineDollyCart>();
        path = cart.m_Path.GetComponent<CinemachineSmoothPath>();
        rigid = GetComponent<Rigidbody>();
        launchForce = rigid.mass * 10.0f;
    }

    private void Update()
    {
        if (!dead)
        {
            if(smoke == null && (path.PathLength - cart.m_Position) / path.PathLength <= 0.15f)
            {
                smoke = Instantiate(smokePrefab);
            }
        }
        //else
        //{
        //    smokeTail = Instantiate(smokeTailPrefab);
        //    smokeTail.transform.position = transform.position;
        //    smokeTail.transform.parent = null;
        //}

        if (smoke != null)
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
        }        
    }

    IEnumerator explosionCo()
    {
        dead = true;
        if (cart != null)
        {
            cart.enabled = false;
        }
        Time.timeScale = 0.1f;
        StartCoroutine(timeCo());
        GameObject explosion = Instantiate(explostionPrefab, transform);
        explosion.transform.localScale *= 1.5f;
        explosion.transform.parent = null;

        rigid.AddForce(new Vector3(-2f,1f,-2f) * launchForce, ForceMode.Impulse);

        float rotateTime=0;
        while (rotateTime < 2.0f)
        {
            rigid.AddTorque(new Vector3(15,0,0), ForceMode.Impulse);
            rotateTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);
        Destroy(smoke);
        Destroy(gameObject);
    }
    IEnumerator timeCo()
    {    
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    IEnumerator setSpeedCo(Collider other)
    {
        yield return new WaitForSeconds(2.0f);
        cart.m_Speed = other.transform.GetComponent<CinemachineDollyCart>().m_Speed + 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DroneEnd"))
        {
            cart.m_Speed = 20;
        }
        if (other.CompareTag("Player"))
        {
            cart.m_Speed = other.transform.GetComponent<CinemachineDollyCart>().m_Speed - 1;
            StartCoroutine(setSpeedCo(other));
        }
        if (other.CompareTag("DestroyPoint"))
        {
            StartCoroutine(explosionCo());
        }
    }
}
