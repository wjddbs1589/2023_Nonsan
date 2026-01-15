using Cinemachine;
using DG.Tweening;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionEffect : HealthManager
{
    public float health;

    public GameObject explosionPrefab;
    [SerializeField] GameObject EnemyfirePrefab;
    [SerializeField] GameObject firePrefab;

    public float damage = 100f;
    public float explosionRange = 5.0f;

    public LayerMask enemyMask;
    AudioSource audioClip;
    Transform clipPosition;

    int score = 100;

    //타게팅 UI
    private GameObject player;
    float dist = 40;
    public GameObject UIrenderer;
    public SpriteRenderer sprite;
    private void Awake()
    {
        UIrenderer = transform.GetChild(0).gameObject;
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < dist && !dead)
        {
            UIrenderer.transform.position = (player.transform.position + (transform.position + new Vector3(0, .5f, 0))) * 0.5f;
            UIrenderer.transform.DOLookAt(player.transform.position, 0.3f);

            sprite.DOFade(1, 1f);
            UIrenderer.transform.DOScale(.5f, 1f);


        }
        else if (dead)
        {
            sprite.DOFade(0, 0.1f);
        }
        else if (distance > dist && !dead)
        {
            sprite.DOFade(0, 0.1f);
        }
    }


    private void Start()
    {
        dead = false;

        player = Camera.main.gameObject;
        audioClip = GetComponent<AudioSource>();
        clipPosition = Camera.main.transform;
        if (Time.timeScale < 1)
        {
            var main = explosionPrefab.GetComponent<ParticleSystem>().main;
            main.simulationSpeed = 4.0f;
        }
    }

    //공격 받았을 때
    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        health -= damage;
        if (health <= 0)
        {           
            if (!dead)
            {
                GameManager.Inst.UImanager.ScoreTextChage(playerNum, score);
                Kill(playerNum);
            }
        }
    }
    //죽을때
    public void Kill(int playerNum)
    {
        dead = true;
        if (Application.isPlaying)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform); //애는 이펙트 끝나면 알아서 사라짐
            explosion.transform.parent = null;

            GameObject fire = Instantiate(firePrefab, transform);
            fire.transform.position = transform.position;
            StartCoroutine(DestroyEffect(fire)); // 2초후에 불 이펙트 제거


            audioClip.Play();

            //주변 피해-----------------------------------------------------
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, explosionRange, Vector3.up, 0f, enemyMask);
            foreach (RaycastHit hits in rayHits)
            {
                Health health = hits.collider.GetComponentInParent<Health>();
                CarHealth carHealth = hits.collider.GetComponent<CarHealth>();
                CarEnemyHealth carEnemyHealth = hits.collider.GetComponentInParent<CarEnemyHealth>();
                ExplosionEffect drumHealth = hits.collider.GetComponent<ExplosionEffect>();

                if (health != null)
                {
                    health.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, damage, hits.collider, playerNum));
                }
                if (carHealth != null)
                {
                    carHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, damage, hits.collider, playerNum));
                }
                if (drumHealth != null)
                {
                    drumHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, damage, hits.collider, playerNum));
                }
                if (carEnemyHealth != null)
                {
                    carEnemyHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, damage, hits.collider, playerNum));
                }

                StartCoroutine(EnemyHasFireCo());
            }
        }        
    }

    //2초후 제거
    IEnumerator EnemyHasFireCo()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    /// <summary>
    /// 일정 시간 이후 이펙트 오브젝트 제거
    /// </summary>
    /// <param name="DestroyTarget">이펙트 게임오브젝트</param>
    /// <param name="time">이펙트 유지시간. 기본 2초. 이후 제거</param>
    /// <returns></returns>
    IEnumerator DestroyEffect(GameObject DestroyTarget, float time = 2)
    {
        yield return new WaitForSeconds(time);
        Destroy(DestroyTarget);
    }

    //폭발 범위 표시용
    private void OnDrawGizmos()
    {        
        Vector3 center = transform.position;//원의 중심 위치        
        Gizmos.color = Color.red;           //원을 그리는 색상
        Gizmos.DrawWireSphere(center, explosionRange);
    }
}


