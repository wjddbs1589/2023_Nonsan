using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowSmoke : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject grenade;  //연막탄
    float throwPower = 8.0f;              //던지는 힘
    Lv3BossCutScenePlayer cutScenePlayer;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        cutScenePlayer = GameObject.Find("BossCutScene").GetComponent<Lv3BossCutScenePlayer>();
        grenade.SetActive(false);
    }
    private void Start()
    {
        cutScenePlayer.cutSceneStarted += SmokeCoStart; 
    }
    /// <summary>
    /// 타임라인 실행후 연막던지는 코루틴을 실행할 함수
    /// </summary>
    void SmokeCoStart()
    {
        StartCoroutine(TakeGrenadeCo());
    }
    /// <summary>
    /// 연막탄 던지는 애니메이션 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TakeGrenadeCo()
    {
        yield return new WaitForSeconds(4.05f);
        anim.SetBool("Throw", true);
        grenade.SetActive(true);
    }
    /// <summary>
    /// 애니메이션 종료시점에 애니메이션 파라미터 값 변경
    /// </summary>
    void EndGrenade()
    {
        anim.SetBool("Throw", false);
    }
    /// <summary>
    /// 연막탄을 던져 이동시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ThrowGrenade()
    {
        grenade.transform.parent = null;
        Rigidbody rb = grenade.transform.AddComponent<Rigidbody>();
        rb.AddForce((transform.forward + transform.up) * throwPower, ForceMode.Impulse);
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        yield return new WaitForSeconds(0.5f);
        grenade.transform.localScale *= 2.0f;
    }
}
