using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using EnemyAI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MoonflowerCarnivore.ShurikenSalvo.ParticleHomingMultiTarget;
using UnityEngine.UI;

public class Lv3MoveCarHealth : HealthManager
{
    public GameObject explostionPrefab;
    public GameObject smokePrefab;
    GameObject smoke;
    
    float HpPercentForSmoke = 0.5f;

    [HideInInspector] public CinemachineDollyCart cart;
    CinemachineSmoothPath path;

    [Tooltip("현재 NPC체력.")]
    public float health = 500f;
    private float totalHealth;
    public GameObject gun;

    bool crashPartStart = false;
    [HideInInspector] public Lv3CameraController PlayerCamera;
    public LayerMask enemyMask;

    Animator anim;

    public AudioClip crashSound;
    UDPcrash UdpCrash;

    bool crash = false; //차량이 충돌햇는지 판별하는 변수
    bool useCrashVoice = false;
    AudioSource crashVoice;
    private void Awake()
    {
        totalHealth = health;
        cart = transform.parent.GetComponent<CinemachineDollyCart>();
        path = cart.m_Path.GetComponent<CinemachineSmoothPath>();
        PlayerCamera = GameObject.Find("MainCamera").GetComponent<Lv3CameraController>();
        UdpCrash = GetComponent<UDPcrash>();
        crashVoice = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    private void Update()
    {
        if (!dead)
        {
            float remainDistance = (path.PathLength - cart.m_Position) / path.PathLength;
            if (smoke == null && remainDistance <= 0.15f)
            {
                smoke = Instantiate(smokePrefab);
            }
            if(remainDistance <= 0)
            {
                transform.parent = transform.parent.parent;
                anim.enabled = true;
            }
        }
        if (smoke != null)
        {
            smoke.transform.position = transform.position + (transform.forward * 2);
        }
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        if (!dead)
        {
            GameManager.Inst.TotalScore += 100;
            health -= damage;
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
            crashPartStart = true;
            cart.m_Speed = 22;
        }
        if (other.CompareTag("Player"))
        {            
            if (crashPartStart)
            {
                if (!useCrashVoice)
                {
                    crashVoice.Play();
                    useCrashVoice = true;
                }
                if (!crash)
                {
                    StartCoroutine(chaseCo());
                    PlayerCamera.Shake(gameObject);
                    PlayerCamera.vsBack = true;
                }
                
            }
        }
        if (other.CompareTag("DestroyPoint"))
        {
            cart.m_Speed = 13.0f;
        }
    }

    IEnumerator chaseCo()
    {
        UdpCrash.Crash();
        AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position, 1);
        crash = true;
        cart.m_Speed = 17;
        yield return new WaitForSeconds(2.0f);
        cart.m_Speed = 20;
        crash = false;
    }
    void explosion()
    {
        if (!dead)
        {
            gun.SetActive(false);
            dead = true;
            crashPartStart = false;
            StartCoroutine(timeCo());
            GameObject explosion = Instantiate(explostionPrefab);
            explosion.transform.position = transform.position;
            explosion.transform.localScale *= 3f;
            explosion.transform.parent = null;
        }
    }
    IEnumerator FireCo(RaycastHit hitObj)
    {
        EnemyHealth health = hitObj.collider.GetComponentInParent<EnemyHealth>();
        if (health != null && !health.burning)
        {
            health.fireEffect = Instantiate(explostionPrefab);
            health.burning = true;
            yield return new WaitForSeconds(2.0f);
            health.burning = false;
            Destroy(health.fireEffect);
        }
    }
    IEnumerator timeCo()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1.0f;
    }

}