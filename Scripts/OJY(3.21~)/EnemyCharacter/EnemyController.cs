using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using DG.Tweening;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    public Transform targetDestination; // 목적지, 
    
    NavMeshAgent agent;

    //이동 및 사격 애니메이션 포함
    Animator animator;
    
    public bool sit = false; //기본적으로 서있는 상태
    public bool arrive = false;     //웨이포인트 도착 여부
    public bool dead = false;//사망여부

    public int bulletCount = 30;
    int bulletMaxCount = 30;
    public float attackChance = 0.01f;

    Transform player; //플레이어 위치 감지
    Transform chest;
    public Transform muzzle;
    public Vector3 offset;
    public ParticleSystem muzzleFlash;
    public IObjectPool<FlashPool> _flashPool;
    public GameObject tracer;
    AudioSource shotSound; //사격 사운드
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        _flashPool = new ObjectPool<FlashPool>(FlashCreate, FlashGetPool, FlashReleasePool, FlashDestroyPool, maxSize: 5);
        shotSound = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        //플레이어 찾기
        if (GameObject.FindGameObjectWithTag("MainCamera") != null)
            player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        offset = new Vector3(10, 135, -65);    //상반신 회전 조정
        if (targetDestination != null)
        {
            agent.SetDestination(targetDestination.position);
            animator.SetBool("Running", true);
        }
    }

    public float remainDistance;
    private void Update()
    {
        //거리가 가깝거나 dead 가 false이고 arrive가 true일때, 안죽고 목적지에 도착했을 때
        if (!dead && arrive) // distance <= 15f || (!dead && arrive)
        {
            StartCoroutine(LookAt());
        }
        else if (!dead && !arrive)
        {
            //도착 했을 때 실행
            //* !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance 
            //* !agent.pathPending => 도착여부를 true, false로 저장. 기본 false에 도착시 true로 변경됨
           
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                arrive = true;
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 앉기 여부 결정
    /// </summary>
    void SitOrNot()
    {
        if (RandomInt(0, 2) == 0)
        {
            sit = true;
            offset = new Vector3(10, 135, -50);  
        }
        else
        {
            sit = false;
        }

        animator.SetBool("Sit", sit);
        animator.SetBool("Aim", !sit);
        animator.SetBool("Shoot",true);
    }

    /// <summary>
    /// 총알을 다 썻는지 확인, 애니메이션 이벤트 함수
    /// </summary>
    /// <returns>총알을 전부 소모했는지 여부.true = 전부 소모</returns>
    void RemainBulletCheck()
    {
        shotSound.Play();

        float randNum = Random.Range(0.0f, 1.0f);
        if (randNum <= attackChance) //1퍼센트 확률
        {
            GameManager.Inst.UImanager.DecreaseHP();
        }

        GameObject bulletEffect = Instantiate(tracer);
        bulletEffect.transform.position = muzzle.transform.position;
        bulletEffect.transform.LookAt(player.transform.position);
        bulletCount--;
        //총알을 전부 썻을때 true 반환
        if (bulletCount < 0)
        {
            animator.SetTrigger("Reload");
            bulletCount = bulletMaxCount;
        }
    }

    IEnumerator LookAt()
    {
        SitOrNot();

        while (true)
        {
            // 플레이어 방향 구하기
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            }
            chest.LookAt(player);
            chest.rotation = chest.rotation * Quaternion.Euler(offset);
            yield return new WaitForSeconds(0.1f);
        }
    }

    

    /// <summary>
    /// 랜덤 실수 뽑기
    /// </summary>
    /// <param name="a">최소값</param>
    /// <param name="b">최대값</param>
    /// <returns>a이상 b미만의 랜덤값</returns>
    private float RandomFloat(float a, float b)
    {
        float num = Random.Range(a, b);
        return num;
    }
    /// <summary>
    /// 랜덤 정수 뽑기
    /// </summary>
    /// <param name="a">최소값</param>
    /// <param name="b">최대값</param>
    /// <returns>a이상 b미만의 랜덤값</returns>
    private int RandomInt(int a, int b)
    {
        int num = Random.Range(a, b);
        return num;
    }

    private void Gunfire()
    {
        _flashPool.Get();
       //_flashPool.Release();
    }

    #region Flash Pool
    private FlashPool FlashCreate()
    {

        FlashPool _flash = Instantiate(muzzleFlash,muzzle).GetComponent<FlashPool>();
        _flash.SetManagedFlash(_flashPool);
        return _flash;
    }
    private void FlashGetPool(FlashPool _flash)
    {
        _flash.gameObject.SetActive(true);

    }
    private void FlashReleasePool(FlashPool _flash)
    {
        _flash.gameObject.SetActive(false);
    }

    private void FlashDestroyPool(FlashPool _flash)
    {
        Destroy(_flash.gameObject);
    }
    #endregion
}
