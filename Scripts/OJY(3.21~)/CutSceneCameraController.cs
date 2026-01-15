using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutSceneCameraController : MonoBehaviour
{
    public GameObject mainCamera;     //정면 카메라
    public GameObject cutSceneCamera; //컷신동안 켜질 카메라
    public float playtime = 5.0f;     //카메라 전환 시간
    public Lv3CameraController Lv3Controller;
    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("MainCamera").gameObject;
        }
        if (cutSceneCamera == null)
        {
            cutSceneCamera = transform.GetChild(0).gameObject;
        }
        cutSceneCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DroneCutSceneStart();
        }
    }
    //컷신 카메라 전환------------------------
    public void DroneCutSceneStart()
    {
        cutSceneCamera.SetActive(true);
        RenderSettings.fog = false;
        StartCoroutine(changeCo());
    }
    IEnumerator changeCo()
    {
        Lv3Controller.vsBack = false;
        yield return new WaitForSeconds(playtime);
        RenderSettings.fog = true;
        cutSceneCamera.SetActive(false);
        transform.GetComponent<Collider>().enabled = false;
    }
    //--------------------------------------

}
