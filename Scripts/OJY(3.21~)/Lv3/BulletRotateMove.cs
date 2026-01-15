using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletRotateMove : MonoBehaviour
{
    [SerializeField]GameObject bulletFire;    //총구 화염 위치
    [SerializeField]Transform bulletPosition; //총알 생성위치
    GameObject target;
    private void Start()
    {
        StartCoroutine(BulletMoveStart());
    }
    /// <summary>
    /// 총알이동
    /// </summary>
    public IEnumerator BulletMoveStart()
    {
        Time.timeScale = 0.2f;

        if (Application.isPlaying)
        {
            transform.position = bulletPosition.position;
            GameObject fire = Instantiate(bulletFire, transform);
            fire.transform.parent = null;
            fire.transform.localScale *= 0.5f;

            target = GameObject.Find("NS_Boss(Clone)");
            transform.position = fire.transform.position; 
            yield return new WaitForSeconds(1.35f);
            TimeRecovery();
        }
    }

    /// <summary>
    /// 게임속도 원상복구
    /// </summary>
    public void TimeRecovery()
    {
        Time.timeScale = 1;
        transform.localScale = Vector3.one * 2;
    }
}
