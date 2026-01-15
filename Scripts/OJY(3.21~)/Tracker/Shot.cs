using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EnemyAI;
using UnityEngine.Pool;
using UnityEngine.UI;
using Valve.VR;
using UnityEditor;
using TMPro;
using AmplifyImpostors;
using static Shot;
using UnityEngine.Events;
using Unity.VisualScripting;

public class Shot : MonoBehaviour
{
	public Transform shotOrigin, drawShotOrigin;
	public LayerMask enemyMask;
	public LayerMask covorMask;
	public int RPM = 600;

    AudioSource audioSource;

    //트래커,컨트롤러
    public SteamVR_Input_Sources inputSource;
    public SteamVR_Action_Boolean actionBoolean;

    //트래커 샷
    public Transform pointImg;

    public float duration;
    public float amount;
	GameObject player;
    public int PlayerNumber;
    //---------------------------------------------------------------------------------------------------

    // Unity에서 ScriptableObject, MonoBehaviour 등과 같은 컴포넌트 클래스 내에 다른 클래스를 정의할 때,
    // 해당 클래스를 인스펙터에서 직렬화하여 표시할 수 있도록 지정하는 특성(Attribute)
    [System.Serializable]
    public class ImpactInfo //이 클래스는 총알 충돌 시 이펙트 종류와 충돌한 물체의 재질 종류를 저장
    {
        public LayerType.LayerTypeEnum LayerType;
        public GameObject ImpactEffect;
        public AudioClip ImpactSound;
    }
    //ImpactInfo 클래스의 배열로, 총알 충돌 시 이펙트를 처리할 때 이 ImpactInfo를 참조하여 해당하는 이펙트를 가져옴
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    //---------------------------------------------------------------------------------------------------
    //총알
    [HideInInspector]public int maxBullet = 20;
    [HideInInspector]public int currentBullet = 20;

    public UnityEvent OnEmpty;

    public delegate void EmptyBullet();
    public EmptyBullet OnEmptyBullet;
    //---------------------------------------------------------------------------------------------------
    //총알 이펙트
    public GameObject bulletHoles;
	public GameObject hitEffect;
	[Tooltip("총알 이펙트 유지시간")]
	public float impactDuration = 3.0f;

	float bulletDamage = 250f;
	public bool canShot=true;

	private WaitForSeconds halfShotDuration;
    //---------------------------------------------------------------------------------------------------
    //public NewRay newRay;

    private void Awake()
    {
        maxBullet = 20;
        currentBullet = 20;
    }
    void Start()
	{
		float waitTime = 60f / RPM;
		halfShotDuration = new WaitForSeconds(waitTime / 2);
        audioSource = GetComponent<AudioSource>();                
    }

    void Update()
    {        
        if (SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
        {
            return;
        }
        //죽지 않음, 사격가능한 상황, 총알이 한발이상 있음 => 사격 가능
        if ( canShot && (currentBullet > 0))
        {
            //카메라의 위치에서 조준 UI의 방향벡터
            Vector3 direction = pointImg.transform.position - Camera.main.transform.position;// 방향 벡터 계산
            Ray ray = new Ray(Camera.main.transform.position, direction.normalized * 30f);   //레이 생성, 방향을 정규화한 뒤 30의 거리로 설정
            RaycastHit hits;

            if (Physics.Raycast(ray, out hits, Mathf.Infinity, enemyMask))
            {
                if (hits.collider)
                {
                    //마우스
                    if (canShot &&  (Input.GetMouseButtonDown(0) ||  actionBoolean.GetStateDown(inputSource)) )                    
                    {
                        //적 캐릭터 
                        Health health = hits.collider.GetComponentInParent<Health>();
                        //드론
                        DroneHealthAuto droneHealthAuto = hits.collider.GetComponentInParent<DroneHealthAuto>();
                        //차량 
                        CarHealth carHealth = hits.collider.GetComponent<CarHealth>();
                        //차량 탑승 적군 
                        CarEnemyHealth carEnemyHealth = hits.collider.GetComponentInParent<CarEnemyHealth>();

                        //폭발물 
                        ExplosionEffect drumHealth = hits.collider.GetComponent<ExplosionEffect>();

                        //사격더미 
                        TargetHuman targetHuman = hits.collider.GetComponentInParent<TargetHuman>();
                        //보스UI 
                        QTEImage qte = hits.collider.GetComponentInParent<QTEImage>();
                        

                        if (health != null)
                        {
                            health.HitCallback(new HealthManager.DamageInfo(hits.point, shotOrigin.forward, bulletDamage, hits.collider, player, PlayerNumber));
                        }
                        else if (carHealth != null)
                        {
                            carHealth.HitCallback(new HealthManager.DamageInfo(hits.point, shotOrigin.forward, bulletDamage, hits.collider, player, PlayerNumber));
                        }
                        else if (droneHealthAuto != null)
                        {
                            droneHealthAuto.HitCallback(new HealthManager.DamageInfo(hits.point, shotOrigin.forward, bulletDamage, hits.collider, player, PlayerNumber));
                        }
                        else if (drumHealth != null)
                        {
                            drumHealth.HitCallback(new HealthManager.DamageInfo(hits.point, shotOrigin.forward, bulletDamage, hits.collider, player, PlayerNumber));
                        }
                        else if (carEnemyHealth != null)
                        {
                            carEnemyHealth.HitCallback(new HealthManager.DamageInfo(hits.point, shotOrigin.forward, bulletDamage, hits.collider, player, PlayerNumber));
                        }
                        else if (targetHuman != null)
                        {
                            targetHuman.Hit();
                        }
                        else if (qte != null)
                        {
                            qte.TargetHit(PlayerNumber);
                        }

                        Shoot();
                        ShotEffect2(ray.direction, hits.point, hits.normal,hits);

                        //적중시 layer에 따른 이펙트 생성
                        var effect = GetImpactEffect(hits.transform.gameObject);
                        if (effect == null)
                            return;
                        var effectIstance = Instantiate(effect, hits.point, new Quaternion()) as GameObject;
                        effectIstance.transform.LookAt(hits.point + hits.normal);
                        Destroy(effectIstance, .5f);

                        GameObject bulletHole = Instantiate(bulletHoles, hits.point, Quaternion.identity);
                        bulletHole.transform.LookAt(hits.point + hits.normal);

                        pointImg.DOScale(1.5f, .8f);
                    }
                    else
                    {
                        pointImg.DOScale(1f, .8f);
                    }
                }
            }
            else if (Physics.Raycast(ray, out hits, Mathf.Infinity, covorMask)) //장애물 및 타겟이 아닌 레이어
            {
                //트래커 + 마우스
                if (Input.GetMouseButtonDown(0) ||  actionBoolean.GetStateDown(inputSource) && canShot)
                {
                    ShotEffect2(ray.direction, hits.point, hits.normal, hits);

                    var effect = GetImpactEffect(hits.transform.gameObject);
                    if (effect == null)
                        return;
                    var effectIstance = Instantiate(effect, hits.point, new Quaternion()) as GameObject;
                    effectIstance.transform.LookAt(hits.point + hits.normal);
                    Destroy(effectIstance, .5f);
                }
            }
            else
            {
                if (canShot && (Input.GetMouseButtonDown(0) || actionBoolean.GetStateDown(inputSource)))
                {
                    BulletChange();
                }
            }
        }
    }

    void BulletChange()
    {
        currentBullet--;
        //OnEmptyBullet?.Invoke();
        GameManager.Inst.UImanager.BulletTextChange(PlayerNumber, currentBullet);
        if (currentBullet == 0)
        {
            OnEmpty?.Invoke();
            StartCoroutine(Reloading());
        }
    }
    /// <summary>
    /// 총알 재장전
    /// </summary>
    /// <returns></returns>
    IEnumerator Reloading()
    {
        canShot = false;
        yield return new WaitForSeconds(2);
        canShot = true;
        currentBullet = maxBullet;
    }
	void Shoot()
    {
        StartCoroutine(ShotEffect());
	}

	private IEnumerator ShotEffect()
	{
		canShot = false;

		yield return halfShotDuration;

		yield return halfShotDuration;

        yield return halfShotDuration;
        yield return halfShotDuration;

        canShot = true;
	}

    //총알 자국 생성
    private void ShotEffect2(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit)
    {
        audioSource.Play(); 
        BulletChange();
        GameObject bulletHole = Instantiate(bulletHoles);
        GameObject effect = Instantiate(hitEffect);
        effect.transform.position = hitPoint;
        bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
        bulletHole.transform.position = hitPoint + 0.01f * hitNormal;
        bulletHole.transform.SetParent(hit.transform);
    }

    //---------------------------------------------------------------------------------------------------
    /// <summary>
    /// 충돌한 물체의 재질 종류를 판별하고 이펙트 종류를 가져옴, 재질에 맞는 소리 재생
    /// 만약 충돌한 물체가 재질을 가지고 있지 않으면, null 값을 반환
    /// </summary>
    /// <param name="impactedGameObject">충돌한 물체</param>
    /// <returns>충돌한 물체의 재질 종류</returns>
    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var layerType = impactedGameObject.GetComponentInParent<LayerType>();
		if (layerType == null)
			return null;

		foreach (var impactInfo in ImpactElemets)
		{
			if (impactInfo.LayerType == layerType.TypeOfLayer)
			{
                AudioSource.PlayClipAtPoint(impactInfo.ImpactSound, transform.position, 1); //재질에 일치하는 소리 재생
				return impactInfo.ImpactEffect;
			}
		}

		return null;
	}
    //---------------------------------------------------------------------------------------------------    
    
}


