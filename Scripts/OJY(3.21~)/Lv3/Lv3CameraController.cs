using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UIElements;
//using UnityEditor.PackageManager;

public class Lv3CameraController : MonoBehaviour
{
    [SerializeField] Transform ForwardTarget;
    [SerializeField] Transform BackTarget;

    //float CameraTurnSpeed = 10f;    
    public bool vsBack = false;

    // 카메라 흔들기에 사용될 값
    [Tooltip("지속시간")]
    public float shakeDuration = 0.3f;
    [Tooltip("강도")]
    public float shakeAmount = 0.5f;
    Vector3 originalPosition = new Vector3(0,2,0);
    BoxCollider box;

    private void Awake()
    {
        ForwardTarget = GameObject.Find("ForwardTarget").transform;
        box = transform.GetComponent<BoxCollider>();
        box.size = new Vector3(0.25f,0.25f, shakeAmount);
    }
    private void FixedUpdate()
    {
        if (vsBack)
        {
            transform.DOLookAt(BackTarget.position + (BackTarget.transform.up * 2), 1f);
        }
        else
        {
            transform.DOLookAt(ForwardTarget.position, .1f);
        }
    }
    
    public void Shake(GameObject crashTarget)
    {
        originalPosition = transform.localPosition;
        StartCoroutine(ShakeCoroutine(crashTarget.transform));
    }
    IEnumerator ShakeCoroutine(Transform crashPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 size = box.size;
            Vector3 randomPosition = new Vector3(Random.Range(-size.x / 2, size.x / 2),
                                                 0,//Random.Range(-size.y / 2, size.y / 2),
                                                 Random.Range(-size.z / 2, size.z / 2));
            transform.localPosition += randomPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
            transform.localPosition = originalPosition;
        }

        transform.localPosition = originalPosition;
    }

}
