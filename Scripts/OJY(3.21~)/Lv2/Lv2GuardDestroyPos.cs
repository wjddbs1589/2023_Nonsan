using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Lv2GuardDestroyPos : MonoBehaviour
{
    //메인 카메라의 타겟 - 플레이어 카메라, 기름통
    //카메라 타겟 전환 시간(기름통이 터지고 원래 카메라로 돌아오는 쿨타임)
    //timeScale전환시 속도 변경

    public Lv2CameraController playerCamera;
    public GameObject targetObject;
    public float chamgeTime = 1.0f;
    public float SetSpeed = 0.2f;

    public float setSoundDelay = 0.3f;

    public GameObject[] sound;

    private void Awake()
    {
        foreach (GameObject obj in sound) 
        {
            obj.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SoundChange());
            playerCamera.cameraTarget = targetObject;
            transform.GetComponent<Collider>().enabled = false;
            Time.timeScale = SetSpeed;
        }
    }
    IEnumerator waitCo()
    {
        yield return new WaitForSeconds(chamgeTime);
        Time.timeScale = 1.0f; 
    }
    IEnumerator SoundChange()
    {
        sound[0].SetActive(true);
        yield return new WaitForSeconds(setSoundDelay);
        sound[0].SetActive(false);
        sound[1].SetActive(true);
    }

}
