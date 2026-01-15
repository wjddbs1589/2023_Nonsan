using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f;  // 흔들림의 강도 설정
    public float shakeDuration = 0.5f;// 흔들림이 지속되는 시간

    private Vector3 originalPosition; //카메라의 초기 위치 저장

    void Awake()
    {
        originalPosition = transform.localPosition; // 카메라의 초기위치를 저장
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float time = 0.0f;

        while (time < shakeDuration)
        {
            Vector3 pos =
                originalPosition          //기본 위치
                + Random.insideUnitSphere //(-1, -1, -1) ~ (1, 1, 1) 범위의 랜덤한 벡터를 반환
                * shakeAmount;            //shakeAmount 값과 곱해주어 카메라를 흔들림 효과
            transform.localPosition = pos;

            time += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Shake();
        }
    }
}
