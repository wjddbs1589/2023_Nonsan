using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv2MoveCarHealth : HealthManager
{
    public GameObject explostionPrefab;
    public GameObject smokePrefab;
    GameObject smoke;

    float HpPercentForSmoke = 0.5f;

    CinemachineDollyCart cart;
    CinemachineSmoothPath path;

    [Tooltip("현재 NPC체력.")]
    public float health = 500f;
    private float totalHealth;

    float launchForce;
    private Rigidbody rigid;

    //bool canDead = false;
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
            if (smoke == null && (path.PathLength - cart.m_Position) / path.PathLength <= 0.15f)
            {
                smoke = Instantiate(smokePrefab);
            }
            else if ((path.PathLength - cart.m_Position) / path.PathLength <= 0)
            {
                StartCoroutine(explosionCo());
            }
        }

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
            if (health <= 0)
            {
                StartCoroutine(explosionCo());
            }
            if (smoke == null && health <= totalHealth * HpPercentForSmoke)
            {
                smoke = Instantiate(smokePrefab);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DroneEnd"))
        {
            cart.m_Speed = 20;
        }
        if (other.CompareTag("DestroyPoint"))
        {
            health = 2000;
        }
        if (other.CompareTag("Player"))
        {
            cart.enabled = false;
            health -= 500;

            if (health <= 0)
            {
                StartCoroutine(rotateCo());
            }
        }
    }

    IEnumerator rotateCo()
    {
        transform.GetComponent<Collider>().enabled = false;
        float rotateTime = 0;
        while (rotateTime < 1.0f)
        {
            rigid.AddTorque(new Vector3(45, 0, 0), ForceMode.Impulse);
            rotateTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        transform.GetComponent<Collider>().enabled = false;
        StartCoroutine(explosionCo());
    }
    IEnumerator explosionCo()
    {
        dead = true;
        if (cart != null)
        {
            cart.enabled = false;
        }

        cart.m_Speed = 0;
        GameObject explosion = Instantiate(explostionPrefab, transform);
        explosion.transform.localScale *= 1.5f;
        explosion.transform.parent = null;

        //RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        //foreach (RaycastHit hitObj in rayHits)
        //{

        //    if (hitObj.transform.GetComponent<HealthManager>() != null)
        //    {
        //        hitObj.collider.SendMessageUpwards("HitCallback", new DamageInfo(hitObj.point, transform.forward, 100, hitObj.collider),
        //            SendMessageOptions.DontRequireReceiver);

        //        if (hitObj.transform.GetComponent<Rigidbody>() != null && hitObj.collider.gameObject != this)
        //        {
        //            hitObj.transform.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 10, 3, ForceMode.Impulse);
        //        }
        //    }
        //}

        yield return new WaitForSeconds(3.0f);
        Destroy(smoke);
        Destroy(gameObject);
    }
}

