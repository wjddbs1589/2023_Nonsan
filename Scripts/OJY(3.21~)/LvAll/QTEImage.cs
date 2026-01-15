using EnemyAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class QTEImage : MonoBehaviour
{
    Image image;                  //시간에 따라 양을 조절할 타이머 이미지
    public float duration = 1.0f; //보간에 걸리는 전체 시간
    public float totalTime = 0.0f;//경과한 시간
    BodySmoke bodySmoke;
    public float recovery = 0.01f;//초당 회복량
    public bool useRotate = true; //플레이어 바라보기 사용 유무 선택
    public MachineGun machinegun;
    private void Start()
    {
        image = GetComponent<Image>();
        bodySmoke = GetComponent<BodySmoke>();
    }
    //float ratio = 0;
    float totalAmount = 1;
    void Update()
    {
        if (useRotate)
        {
            transform.DOLookAt(Camera.main.transform.position, 0.1f);
        }

        //초당 체력 회복
        if (image.fillAmount <= 1.0f)
        {
            totalAmount = image.fillAmount + (recovery * Time.deltaTime);
            image.fillAmount = Mathf.Lerp(image.fillAmount, totalAmount, 1);   
        }

        //게이지가 전부 닳았을 때
        if (image.fillAmount <= 0)
        {
            //중간보스(헬기)의 몸체에만 적용
            if (bodySmoke != null)
            {
                bodySmoke.SmokeActive();
            }

            //머신건 든 적에 컴포넌트로 들어 있을때 적용
            if (machinegun!=null)
            {
                machinegun.PlayDeadAnimation();
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 타이머가 있는 조준점을 사격했을때 실행되는 함수, 타이머의 수치를 감소 시킴
    /// </summary>
    public void TargetHit(int PlayerNumber)
    {
        GameManager.Inst.UImanager.ScoreTextChage(PlayerNumber, 10);

        //시간에 따라 줄어들지 않고 적중시에만 수치가 줄어들게 설정 할 때
        image.fillAmount -= 0.05f;
        totalAmount -= 0.05f;
    }

}
