using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using AmplifyImpostors;

public class Enemy : HealthManager
{
    //체력 , 세팅
    private float enemyHealth = 100f;
    public Vector3 offset;
    public GameObject hitEffect;

    //private bool dead;
    public Transform weapon;
    private Transform chest;

    //플레이어 감지 변수
    public Transform player;          //플레이어 위치 감지

    //장애물 레이어
    public LayerMask coverLayer;

    //첫 이동
    public bool runEnemy = true; // 이동 -> 도착시 false
    public Vector3 wayPoint;     //이동할 위치

    //이펙트


    private NavMeshAgent agent;
    Navigation navigation;
    private Animator animator;
    public bool lookat = false;

    public int bulletCount = 30;        //총알개수
    public bool sitattack, standattack; //공격 상태
    public GameObject Tracer;
    public Transform Muzzle;

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //weapon = weapon.parent;
        
    }
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        //lookatTarget = GetComponent<LookatTarget>();

        //타겟 찾기
        //if (GameObject.FindGameObjectWithTag("Target") != null)
        //    player = GameObject.FindGameObjectWithTag("Target").transform;
        player = Camera.main.transform;

        int runIndex = Random.Range(0, 3); //뛰는 모션 인덱스 랜덤설정 0~2
                                           //runEnemy일 경우 시작시 목적지로 뛰어간다.
        if (runEnemy)
        {
            //lookatTarget.enabled = false;
            transform.DOLookAt(wayPoint, 1f); //가는동안 목적지 방향으로 보기
            //agent.SetDestination(wayPoint); //목적지 설정
            animator.SetBool("Running", true);
            animator.SetInteger("RunIndex", runIndex);
        }

        //StartCoroutine(ArrivalConfirm()); //도착 확인 -> sit 또는 aim 설정
        InvokeRepeating("MoveShot", 20f, 15f);
    }

    private void LateUpdate()
    {
        if (runEnemy)
        {
            if (agent.remainingDistance <= agent.stoppingDistance) //목적지에 도착했는지 확인 navmeshagent에서 stoppinDistance 설정
            {
                lookat = true;

                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 11f, coverLayer); // 도착 후 주변에 장애물이 있는지 확인 
                if (hits.Length > 0)//장애물 발견시 일단 엄폐(sit)
                {
                    animator.SetBool("Sit", true);

                    if ((int)RandomNum(-0.49f, 1.49f) == 0) //0이면 앉아서 공격
                    {
                        sitattack = true;
                        StartCoroutine(SitAttackState());
                    }
                    else //1이면 서서공격
                    {
                        animator.SetBool("Sit", false);
                        animator.SetBool("Aim", true);
                        standattack = true;
                        StartCoroutine(AimAttackState());
                    }

                }
                else //장애물이 없으면 서서공격
                {
                    animator.SetBool("Aim", true);
                    standattack = true;
                    StartCoroutine(AimAttackState());
                }
                animator.SetBool("Running", false);
                runEnemy = false;
            }
        }
        

        if (lookat)
        {
            //transform.DOLookAt(player.position, 1f);
            chest.LookAt(player);
            chest.rotation = chest.rotation * Quaternion.Euler(offset);
        }
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        Object.Instantiate<GameObject>(hitEffect, location, Quaternion.LookRotation(-direction), this.transform);
        enemyHealth -= damage;

        if (!dead)
        {
            animator.SetTrigger("Hit");
        }
        // Time to die.
        if (enemyHealth <= 0)
        {
            // Kill the NPC?
            if (!dead)
            {
                GameManager.Inst.killCount[playerNum]++;
                GameManager.Inst.Score[playerNum]++;
                Kill();
            }

            bodyPart.GetComponent<Rigidbody>().AddForce(10f * direction.normalized, ForceMode.Impulse);
            int deathNum = Random.Range(0, 10);
            animator.SetInteger("Death", deathNum);
        }
    }

    public void Kill()
    {
        // Destroy all other MonoBehaviour scripts attached to the NPC.
        foreach (MonoBehaviour mb in this.GetComponents<MonoBehaviour>())
        {
            if (this != mb)
                Destroy(mb);
        }
        Destroy(this.GetComponent<NavMeshAgent>());
        RemoveAllForces();
        animator.enabled = false;
        Destroy(weapon.gameObject);
        dead = true;
    }


    private void RemoveAllForces()
    {
        foreach (Rigidbody member in GetComponentsInChildren<Rigidbody>())
        {
            member.isKinematic = false;
            member.velocity = Vector3.zero;
        }
    }

    //도착 확인
    IEnumerator ArrivalConfirm()
    {
        while (runEnemy)
        {            
            if (agent.remainingDistance <= agent.stoppingDistance) //목적지에 도착했는지 확인 navmeshagent에서 stoppinDistance 설정
            {
                lookat = true;

                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 11f, coverLayer); // 도착 후 주변에 장애물이 있는지 확인 
                if (hits.Length > 0)//장애물 발견시 일단 엄폐(sit)
                {
                    animator.SetBool("Sit", true);
                    yield return new WaitForSeconds(RandomNum(0, 3)); //앉아있을 시간 랜덤 0~2초

                    if ((int)RandomNum(-0.49f, 1.49f) == 0) //0이면 앉아서 공격
                    {
                        sitattack = true;
                        StartCoroutine(SitAttackState());
                    }
                    else //1이면 서서공격
                    {
                        animator.SetBool("Sit", false);
                        animator.SetBool("Aim", true);
                        standattack = true;
                        StartCoroutine(AimAttackState());
                    }

                }
                else //장애물이 없으면 서서공격
                {
                    animator.SetBool("Aim", true);
                    standattack = true;
                    StartCoroutine(AimAttackState());
                }
                animator.SetBool("Running", false);
                runEnemy = false;

            }

            yield return new WaitForSeconds(0.1f); //0.1초마다 도착 확인
        }
    }

    //서서 공격
    IEnumerator AimAttackState()
    {
        while (standattack)
        {
            animator.SetTrigger("Shoot");
            bulletCount -= 1; //총알개수 카운트
            yield return new WaitForSeconds(0.3f);
            if (bulletCount == 15)
            {
                int state = (int)RandomNum(-0.49f, 1.49f);
                // 15발 쏘고 난 뒤 상태 전환할지 체크
                if (state == 0)//앉아서
                {
                    standattack = false;
                    sitattack = true;
                    animator.SetBool("Sit", true);
                    animator.SetBool("Aim", false);
                    StopAllCoroutines();
                    StartCoroutine(SitAttackState());
                }
                else//서서
                {
                    break;
                }
            }
            else if (bulletCount <= 0) // 재장전 확인
            {
                animator.SetTrigger("Reload"); //총알이 0 이하 -> 재장전
                bulletCount = 30;
                yield return new WaitForSeconds(4.5f);

            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    //앉아서 공격
    IEnumerator SitAttackState()
    {
        while (sitattack)
        {
            animator.SetTrigger("Shoot");
            bulletCount -= 1;
            yield return new WaitForSeconds(0.3f);

            if (bulletCount == 15)
            {
                int state = (int)RandomNum(-0.49f, 1.49f);
                // 5발 쏘고 난 뒤 상태 전환할지 체크
                if (state == 0)
                {
                    sitattack = false;
                    standattack = true;
                    animator.SetBool("Sit", false);
                    animator.SetBool("Aim", true);
                    StopAllCoroutines();
                    StartCoroutine(AimAttackState());
                }
                else
                {
                    break;
                }
            }
            else if (bulletCount <= 0)
            {
                animator.SetTrigger("Reload");
                bulletCount = 30;
                yield return new WaitForSeconds(4.5f);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    void MoveShot()
    {
        //Debug.Log("MoveShot");
        StopAllCoroutines();
        animator.SetInteger("MoveShotIndex", 0);// (int)RandomNum(-0.49f, 3.49f));
        animator.SetTrigger("MoveShot");
        //if (transform.parent.gameObject.activeSelf)
        //{
        //    if (sitattack)
        //    {
        //        StartCoroutine(SitAttackState());
        //    }
        //    else
        //    {
        //        StartCoroutine(AimAttackState());
        //    }
        //}
        
    }
    void SpawnTracer()
    {
        GameObject tracer = Instantiate(Tracer);
        tracer.transform.position = Muzzle.transform.position;
    }

    private float RandomNum(float a, float b)
    {
        float num = Random.Range(a, b);
        return num;
    }
}
