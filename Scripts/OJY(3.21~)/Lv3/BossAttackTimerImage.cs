using System;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using static RootMotion.Demos.Turret;

public class BossAttackTimerImage : MonoBehaviour
{
    Transform origin;                       //이동 목표
    SpriteRenderer spriteRenderer;  
    public GameObject linePrefab;           //연결선 프리펩
    [SerializeField] Sprite correctImage;
    //연결선 설정용 변수----------
    GameObject lineObject;       
    LineRenderer lineRenderer;
    //--------------------------
    Lv3BossShot bossShot;   

    float moveTime;         //이동할 시간
    float ratio;            //이동한 거리의 비율

    GameObject ChildImage;      //과녁에 맞춰질 자식 오브젝트
    float randomSize = 5;       //랜덤으로 지정될 크기
    Vector3 scale = Vector3.one;//크기가 저장될 벡터


    private void Awake()
    {
        bossShot = GameObject.Find("BossAimingVCam").GetComponent<Lv3BossShot>();
        origin = transform.parent;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            SpriteRenderer parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            if (parentSpriteRenderer != null)
            {
                spriteRenderer.color = parentSpriteRenderer.color;
            }
        }



        ChildImage = transform.GetChild(0).gameObject;    
        moveTime = randomSize - 1f;                       //크기가 1이 될 떄까지의 시간 
        scale = ChildImage.GetComponent<RectTransform>().localScale = Vector3.one * randomSize;  //줄어들 자식 오브젝트의 크기 설정
    }
   
   
}
