using Cinemachine;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    public GameObject explostionPrefab;
    public GameObject smokePrefab;
    public GameObject firePrefab;

    CinemachineDollyCart cart;
    public GameObject gun;

    [SerializeField] GameObject EnemyfirePrefab; //폭발할때 적에게 붙일 프리펩
    float explosionDamage = 1000.0f; //폭발 데미지
    public float explosionRange = 5.0f;
    public AudioClip explosionSound;
    public LayerMask enemyMask;

    private void Awake()
    {
        cart = transform.GetComponent<CinemachineDollyCart>();
    }
    void Explosion()
    {
        AudioSource.PlayClipAtPoint(explosionSound,Camera.main.transform.position, 1);
        if (Application.isPlaying)
        {
            GameObject explosion = Instantiate(explostionPrefab);
            explosion.transform.position = transform.position + (transform.forward * 2);
            explosion.transform.localScale *= 1.5f;

            GameObject fire = Instantiate(smokePrefab);
            fire.transform.position = explosion.transform.position;
            explosion.transform.localScale *= 1.5f;

            GameObject smoke = Instantiate(smokePrefab);
            smoke.transform.position = explosion.transform.position;
            explosion.transform.localScale *= 1.5f;

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, explosionRange, Vector3.up, 0f, enemyMask);
            foreach (RaycastHit hits in rayHits)
            {
                Health health = hits.collider.GetComponentInParent<Health>();
                CarHealth carHealth = hits.collider.GetComponent<CarHealth>();
                CarEnemyHealth carEnemyHealth = hits.collider.GetComponentInParent<CarEnemyHealth>();
                ExplosionEffect drumHealth = hits.collider.GetComponent<ExplosionEffect>();

                if (health != null)
                {
                    health.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, 4));
                }
                if (carHealth != null)
                {
                    carHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, 4));
                }
                if (drumHealth != null)
                {
                    drumHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, 4));
                }
                if (carEnemyHealth != null)
                {
                    carEnemyHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, 4));
                }
                StartCoroutine(FireCo(hits));
            }
        }        
    }
    IEnumerator FireCo(RaycastHit hitObj)
    {
        EnemyHealth health = hitObj.collider.GetComponentInParent<EnemyHealth>();
        if (health != null && !health.burning)
        {
            Debug.Log(hitObj.transform.name);
            health.fireEffect = Instantiate(EnemyfirePrefab);
            health.burning = true;
            yield return new WaitForSeconds(2.0f);
            health.burning = false;
            Destroy(health.fireEffect);
        }
    }
    //폭발 범위 표시용
    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;//원의 중심 위치        
        Gizmos.color = Color.red;           //원을 그리는 색상
        Gizmos.DrawWireSphere(center, explosionRange);
    }
}
