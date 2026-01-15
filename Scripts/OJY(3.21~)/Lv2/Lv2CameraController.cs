using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;

public class Lv2CameraController : MonoBehaviour
{
    public Transform SkyCamera;          //드론전투시 카메라 타겟
    public Transform GroundCamera;       //정면 카메라 타겟
    public bool vsFlyingEnemy = false;   //드론전투 시작 여부

    CinemachineDollyCart thisCart;  //차량
    CinemachineSmoothPath path;     //차량 경로
    public GameObject target;       //도착이후 카메라 타겟
    public GameObject helicopterEnemy;
    public GameObject cameraTarget;
    private void Awake()
    {
        thisCart = transform.parent.GetComponent<CinemachineDollyCart>();
        path = GameObject.Find("Path_Player").GetComponent<CinemachineSmoothPath>();
        target = GameObject.Find("MainEnemyCar");
        cameraTarget = GroundCamera.transform.gameObject;
    }

    private void Update()
    {
        transform.DOLookAt(cameraTarget.transform.position, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start"))
        {
            vsFlyingEnemy = true;
            cameraTarget = SkyCamera.transform.gameObject;
            SkyCamera.transform.GetComponent<CinemachineDollyCart>().m_Speed = 20.0f;
        }
        if (other.CompareTag("End"))
        {
            cameraTarget = GroundCamera.transform.gameObject;
            Time.timeScale = 1.0f;
        }
    }
    IEnumerator LookHelicopter()
    {
        //helicopterEnemy.transform.position -= transform.up * 2;
        cameraTarget = helicopterEnemy;
        yield return new WaitForSeconds(15);
        cameraTarget = GroundCamera.transform.gameObject;
    }
}
