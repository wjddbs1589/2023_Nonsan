using DG.Tweening;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using static Car;

public class CarEnemyHealth : HealthManager
{    
    public float health = 1000; //체력
    private float totalHealth; //최대체력
    public GameObject bloodSample;
    public Transform weapon;                                   
    Animator anim;                
  
    GameObject player;

    public GameObject bulletPrefab;
    float attackDuration = 3.0f;      //현재 공격 지속 시간
    float attackDurationOrigin = 3.0f;//전체 공격 지속 시간
    float reloadTime = 2.0f;          //재장전까지 남은 현재 시간
    float reloadTimeOrigin = 2.0f;    //재장전까지 남은 전체 시간
    bool canShot = true;              //사격 가능 여부
    [SerializeField]MeshRenderer fixedGun; //차량에 기본 장착된 총. 죽으면 활성화
    [SerializeField]MeshRenderer rotateGun;//적과 함께 회전할 총. 죽으면 비 활성화
    
    AudioClip audioClip;
    ExplosionCarEnemy carEnemy;
    public AngleCheck angleCheck;

    int score = 100;
    private void Awake()
    {
        foreach (Transform child in transform.parent.transform.parent)
        {
            angleCheck = child.GetComponent<AngleCheck>();
        }

        carEnemy = GetComponent<ExplosionCarEnemy>();
        audioClip = GetComponent<AudioSource>().clip;
        fixedGun.enabled = false;
        player = GameObject.Find("PlayerCar").gameObject;
        totalHealth = health;
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (!dead)
        {            
            if (angleCheck.find)
            {
                //사격 가능여부 판별 
                //1.쏠수 있는 상태일 때
                if (canShot)
                {
                    //사격 지속시간이 남아있으면 사격
                    if (attackDuration >= 0)
                    {
                        attackDuration -= Time.deltaTime; //사격가능 시간 감소
                        anim.SetBool("FindPlayer", true); //사격 애니메이션 활성화
                    }
                    else
                    {
                        canShot = false;                  //사격가능 여부 변경  
                        anim.SetBool("FindPlayer", false);//사격 애니메이션 비활성화
                    }
                }
                //2.쏠수 없는 상태일 때
                else
                {
                    anim.SetBool("FindPlayer", false);//사격 애니메이션 비활성화
                    reloadTime -= Time.deltaTime;     //재장전시간 감소

                    //0보다 작아졌을 때
                    if (reloadTime <= 0)
                    {
                        canShot = true;                         //사격 가능여부 변경
                        reloadTime = reloadTimeOrigin;          //재장전 시간 초기화
                        attackDuration = attackDurationOrigin;  //사격 가능시간 초기화
                    }
                }
            }
            else
            {                
                anim.SetBool("FindPlayer", false); //사격 애니메이션 비활성화  
            }
        }
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        if (!dead)
        {
            Object.Instantiate<GameObject>(bloodSample, location, Quaternion.LookRotation(-direction), this.transform);
            health -= damage;
            if (health <= 0)
            {
                if (!dead)
                {
                    GameManager.Inst.UImanager.ScoreTextChage(playerNum, score, 1);
                    Kill();
                }
            }
        }        
    }

    public void Kill()
    {
        
        if (carEnemy == null)
        {
            dead = true;            
        }

        canShot = false;
        fixedGun.enabled = true;
        rotateGun.enabled = false;
        anim.SetBool("Dead", true);
        RemoveAllForces();
        RemoveAllColliders();
        //transform.GetComponent<Collider>().enabled = false;
        foreach (Rigidbody mb in this.GetComponents<Rigidbody>())
        {
            if (this != mb)
            {
                mb.useGravity = true;
                mb.isKinematic = false;
            }
        }
        foreach (MonoBehaviour mb in this.GetComponents<MonoBehaviour>())
        {
            if (this != mb)
                Destroy(mb);
        }

    }
    private void RemoveAllColliders()
    {
        Collider[] colliders = GetComponents<Collider>();  

        for (int i = 0; i < colliders.Length; i++)
        {
            Destroy(colliders[i]);  
        }
    }
    void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    //// Update health bar HUD to current NPC health.
    //private void UpdateHealthBar()
    //{
    //    float scaleFactor = health / totalHealth;
    //    //healthBar.sizeDelta = new Vector2(scaleFactor * originalBarScale, healthBar.sizeDelta.y);
    //}

    // Remove existing forces and set ragdoll parts as not kinematic to interact with physics.
    private void RemoveAllForces()
    {
        foreach (Rigidbody member in GetComponentsInChildren<Rigidbody>())
        {
            member.isKinematic = false;
            member.velocity = Vector3.zero;
        }
    }

    void spawnBullet()
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position, GameManager.Inst.EnemyShotVolume);
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.transform.position = weapon.transform.position;
    }

    List<AudioSource> audioSources = new List<AudioSource>();
    // 중첩된 소리 재생
    void PlayMultipleSounds(AudioClip clip, float volume)
    {
        // 오디오 소스 생성
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        // 오디오 소스 재생
        audioSource.Play();
        // 오디오 소스를 리스트에 추가
        audioSources.Add(audioSource);
    }

    // 모든 중첩된 소리 중지
    void StopAllSounds()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            Destroy(audioSource);
        }
        audioSources.Clear();
    }
}
