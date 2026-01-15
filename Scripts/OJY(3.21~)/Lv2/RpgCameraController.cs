using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RpgCameraController : MonoBehaviour
{
    [SerializeField] GameObject RpgCamera;
    [SerializeField] GameObject PlayerCamera;

    Animator[] anims;
    RPGShoot[] shoots;

    private void Awake()
    {
        //if (RpgCamera==null)
        //{
        //    RpgCamera = GameObject.Find("RpgCamera_1").gameObject;
        //}
        //RpgCamera.SetActive(false);

        //if (PlayerCamera == null)
        //{
        //    PlayerCamera = GameObject.Find("MainCamera").gameObject;
        //}

        anims = new Animator[transform.childCount];
        shoots = new RPGShoot[transform.childCount];
        for (int i = 0;i< transform.childCount; i++) 
        {
            anims[i] = transform.GetChild(i).GetComponent<Animator>();
            shoots[i] = transform.GetChild(i).GetComponent<RPGShoot>();
        }
    }
    IEnumerator CameraChangeCo()
    {
        RpgCamera.SetActive(true);
        PlayerCamera.SetActive(false);
        yield return ShotCo();
        //StartCoroutine(ShotCo());
        yield return new WaitForSeconds(2f);
        RpgCamera.SetActive(false);
        PlayerCamera.SetActive(true);
    }
    IEnumerator ShotCo()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < transform.childCount; i++)
        {
            anims[i].enabled = true;
            shoots[i].Shot();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CameraChangeCo());
        }
    }

}
