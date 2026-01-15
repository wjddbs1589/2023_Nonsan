using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterSound : MonoBehaviour
{
    public Transform player; // 플레이어 Transform 참조
    public float maxDistance = 6f; // 최대 거리
    public float minDistance = 4f; // 최소 거리
    public float volumeStep = 0.3f; // 볼륨 변경 단위

    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Start()
    {
        player = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 플레이어와 게임 오브젝트 사이의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 볼륨 조정
        if (distance >= minDistance && distance <= maxDistance)
        {
            float volume = 1f - (distance - minDistance);
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
        }
        else
        {
            audioSource.volume = 0f;
        }
    }
}
