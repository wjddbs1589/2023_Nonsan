using Cinemachine;
using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace EnemyAI
{
    public class CarHealth : HealthManager
    {
        public float health = 500;        

        private float totalHealth;

        Rigidbody rigid;

        public float power;
        public float exp_power;

        public GameObject firePrefab;
        public GameObject smokePrefab;
        public GameObject bonenet;
        public GameObject door1, door2;
        public GameObject[] wheels;

        public GameObject[] explosionPrefab;         //생성될 폭발 프리펩 배열
        [SerializeField] GameObject EnemyfirePrefab; //폭발할때 적에게 붙일 프리펩

        public AudioClip explosionSound;
        public AudioClip crashSound;
        GameObject[] explosion;             //생성된 폭발 프리펩을 저장할 배열
        float explosionDamage = 250.0f;    //폭발 데미지
        public float explosionRange = 5.0f; //폭발범위
        int explosionNum;                   
        public int explosionCount = 3; //폭발 횟수
        float explosionScale = 1.0f;   //폭발 프리펩 크기
        public LayerMask enemyMask;

        Vector3 center;
        BoxCollider boxCollider;
        float minX;
        float maxX;
        float minY;
        float maxY;
        float minZ;
        float maxZ;
        Vector3 randomPosition;

        CinemachineDollyCart cart;
        Animator anim;

        int score = 100;

        private void Awake()
        {            
            totalHealth = health;

            explosion = new GameObject[explosionPrefab.Length];
            boxCollider = transform.GetComponent<BoxCollider>();

            // 중심 위치
            center = Vector3.zero;

            // x, y, z 축 길이
            float xLength = boxCollider.size.x;
            float yLength = boxCollider.size.y;
            float zLength = boxCollider.size.z;

            //콜라이더 범위 계산
            minX = center.x - (xLength / 2f);
            maxX = center.x + (xLength / 2f);
            minY = center.y - (yLength / 2f);
            maxY = center.y + (yLength / 2f);
            minZ = center.z - (zLength / 2f);
            maxZ = center.z + (zLength / 2f);

            cart = transform.GetComponent<CinemachineDollyCart>();
            rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            if(anim != null) anim.enabled = false;
        }

        public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
        {
            health -= damage;
            if (health <= 0)
            {
                // Take damage received from current health.
                // Time to die.
                if (!dead)
                {
                    GameManager.Inst.UImanager.ScoreTextChage(playerNum, score);
                    StartCoroutine(KillCo(playerNum));
                }
            }
        }

        public void Kill()
        {
            //StartCoroutine(KillCo());
        }
        IEnumerator KillCo(int playerNum)
        {
            dead = true; 

            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 1);
            GameObject[] firePrefabs = new GameObject[explosionCount];  //생성될 불 프리펩 저장할 배열
            GameObject[] smokePrefabs = new GameObject[explosionCount]; //불과 함께 생성될 연기 배열

            var position = transform.position;
            rigid.AddExplosionForce(exp_power, new Vector3(position.x + Random.Range(-2.5f, 2.5f), position.y + Random.Range(0, 0.8f), 
                position.z + Random.Range(-3.2f, 3.2f)), 3, 3, ForceMode.Impulse);

            if (bonenet != null)
            {
                bonenet.transform.rotation = Quaternion.Euler(-5, 0, 0);
                bonenet.transform.position = new Vector3(0, -1.4f, 0.4f);
            }
            if (door1 != null && door2 != null)
            {
                door1.AddComponent<Rigidbody>();
                door2.AddComponent<Rigidbody>();
                door1.AddComponent<BoxCollider>();
                door2.AddComponent<BoxCollider>();
                door1.GetComponent<Rigidbody>().AddForce(Vector3.right * power);
                door2.GetComponent<Rigidbody>().AddForce(Vector3.left * power);
            }

            rigid.velocity = new Vector3(rigid.velocity.x, 2, rigid.velocity.z);

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
                    health.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, playerNum));
                }
                if (carHealth != null)
                {
                    carHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, playerNum));
                }
                if (drumHealth != null)
                {
                    drumHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, playerNum));
                }
                if (carEnemyHealth != null)
                {
                    carEnemyHealth.HitCallback(new HealthManager.DamageInfo(hits.point, transform.position, explosionDamage, hits.collider, playerNum));
                }
                //hitObj.collider.SendMessageUpwards("HitCallback", new HealthManager.DamageInfo(hitObj.point, hitObj.transform.position - transform.position, explosionDamage,
                //        hitObj.collider, playerNum), SendMessageOptions.DontRequireReceiver);

                if (hits.transform.GetComponent<Rigidbody>() != null && hits.collider.gameObject != this)
                {
                    hits.transform.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 10, 3, ForceMode.Impulse);
                }

                StartCoroutine(FireCo(hits));
            }

            //무작위로 폭발 생성-----------------------------------------------------
            for (int i = 0; i < explosionCount; i++)
            {
                // 무작위 위치 생성
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                float randomZ = Random.Range(minZ, maxZ);
                // 무작위 위치 설정
                randomPosition = new Vector3(randomX, randomY, randomZ);

                if (i == explosionCount - 1)
                {
                    //마지막 폭발은 가장 크게 설정
                    explosionScale = 2f;
                }
                else
                {
                    explosionNum = Random.Range(0, explosionPrefab.Length - 1);
                    explosionScale = 1f;
                }

                explosion[i] = Instantiate(explosionPrefab[explosionNum], transform);
                explosion[i].transform.localPosition = center + randomPosition;
                explosion[i].transform.localScale *= explosionScale;

                // 폭발 위치 조정. 차량위치 + 차량 크기범위 내의 랜덤위치
                if (firePrefab != null)
                {
                    firePrefabs[i] = Instantiate(firePrefab, transform);
                    firePrefabs[i].transform.position = explosion[i].transform.position;
                }
                if (smokePrefab != null)
                {
                    smokePrefabs[i] = Instantiate(smokePrefab, transform);
                    smokePrefabs[i].transform.position = explosion[i].transform.position;
                }
                yield return new WaitForSeconds(0.1f);
            }

           
            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<Rigidbody>().isKinematic = true;

            yield return new WaitForSeconds(5.0f);

            for(int i = 0; i < explosionCount; i++)
            {
                Destroy(firePrefabs[i]);
                Destroy(smokePrefabs[i]);
            }
        }

        IEnumerator FireCo(RaycastHit hitObj)
        {
            EnemyHealth health = hitObj.collider.GetComponentInParent<EnemyHealth>();
            if (health != null && !health.burning)
            {
                health.fireEffect = Instantiate(EnemyfirePrefab);
                health.burning = true;
                yield return new WaitForSeconds(2.0f);
                health.burning = false;
                Destroy(health.fireEffect);
            }
        }

        private void OnTriggerEnter(Collider other)
        {            
            if (other.CompareTag("Player"))
            {
                transform.GetComponent<Collider>().enabled = false;
                if (cart != null)
                {
                    cart.enabled = false;
                    if (anim !=null)
                    {
                        AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position, 1);
                        anim.enabled = true;
                    }
                }
            }
        }

        //폭발 범위 표시용
        private void OnDrawGizmos()
        {
            Vector3 center = transform.position;//원의 중심 위치        
            Gizmos.color = Color.blue;           //원을 그리는 색상
            Gizmos.DrawWireSphere(center, explosionRange);
        }
    }
}