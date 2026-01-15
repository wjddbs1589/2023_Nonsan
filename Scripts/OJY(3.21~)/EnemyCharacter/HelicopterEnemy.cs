using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HelicopterEnemy : MonoBehaviour
{
    public GameObject target;
    public GameObject tracer;
    public GameObject flash;
    public GameObject HelicopterBarrel;
    bool attack = false;
    float attackCooltime = 1.0f;     //현재 쿨타임
    float attackCoolOrigin = 2.0f;   //전체 쿨타임
    public float attackDelay = 0.1f; //총알 발사 딜레이
    Transform[] muzzles;             //총알 생성 위치
    EnemyHelicopter enemyHelicopter;
    AudioSource gunSound;
    private void Awake()
    {
        muzzles = new Transform[HelicopterBarrel.transform.childCount];
        for(int i = 0;i<HelicopterBarrel.transform.childCount; i++)
        {
            muzzles[i] = HelicopterBarrel.transform.GetChild(i).transform;
        }
        target = Camera.main.gameObject;
        enemyHelicopter = transform.GetComponentInParent<EnemyHelicopter>();
        gunSound = GetComponent<AudioSource>();        
    }

    void Update()
    {
        transform.LookAt(target.transform.position);
        if (attack)
        {
            attackCooltime -= Time.deltaTime;
            if (attackCooltime <= 0)
            {
                attackCooltime = attackCoolOrigin;
                StartCoroutine(ShotCo());
            }
        }
    }

    /// <summary>
    /// 총알 발사 코루틴, 여러개의 총알 구멍중에서 하나를 랜덤으로 뽑아 총알(tracer)생성
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotCo()
    {
        for (int i = 0; i < muzzles.Length * 3; i++)
        {
            if (!enemyHelicopter.dead)
            {
                int RandNum = Random.Range(0, muzzles.Length);

                flash = muzzles[RandNum].transform.GetChild(0).gameObject;
                flash.SetActive(true);

                GameObject bullet = Instantiate(tracer);
                bullet.transform.position = muzzles[RandNum].transform.position;
                gunSound.Play();
                bullet.transform.LookAt(target.transform.position);

                yield return new WaitForSeconds(0.05f);
                flash.SetActive(false);
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }
    void AttackStart()
    {
        attack = true;
        attackCooltime = 0;
    }
    void AttackStop()
    {
        attack = false;
    }
}
