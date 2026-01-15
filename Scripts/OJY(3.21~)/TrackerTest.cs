using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//수정본
public class TrackerTest : MonoBehaviour
{
    [Header("Transforms")]
    [Tooltip("Vive Tracker의 Transform")]
    public Transform trackerTransform;

    [Tooltip("Controller의 Transform")]
    public Transform controllerTransform;

    [Header("Position Settings")]
    [Tooltip("위치 보간 속도")]
    public float positionInterpolationSpeed = 10f;

    [Header("Rotation Settings")]
    [Tooltip("회전 보간 속도")]
    public float rotationInterpolationSpeed = 10f;
    [Tooltip("회전 제한 값")]
    public Vector3 rotationLimits;
    [Tooltip("감도 조정 값")]
    public float sensitivity = 1f;
    [Tooltip("회전 가중치")]
    public float weight;

    private Quaternion initialRotation; // 초기 회전 값

    void Start()
    {
        // Vive Tracker의 초기 위치와 회전 값을 저장하여 초기 오프셋과 회전값을 계산합니다.
        initialRotation = Quaternion.Inverse(trackerTransform.rotation) * transform.rotation;
    }

    void Update()
    {
        transform.position = controllerTransform.position;

        // Vive Tracker의 회전과 초기 회전값을 조합하여 목표 회전값을 설정합니다.
        Quaternion targetRotation = trackerTransform.rotation * initialRotation;

        // 회전을 보간하여 부드럽게 회전시킵니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationInterpolationSpeed);

        // 회전 각도를 감도와 가중치에 따라 조정하고 지정된 회전 제한 값 범위 내로 클램핑합니다.
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;
        rotationEulerAngles *= sensitivity;
        rotationEulerAngles.x = Mathf.Clamp(rotationEulerAngles.x / weight, -rotationLimits.x, rotationLimits.x);
        rotationEulerAngles.y = Mathf.Clamp(rotationEulerAngles.y / weight, -rotationLimits.y, rotationLimits.y);
        rotationEulerAngles.z = Mathf.Clamp(rotationEulerAngles.z / weight, -rotationLimits.z, rotationLimits.z);
        transform.rotation = Quaternion.Euler(rotationEulerAngles);
    }

    public void PositionReset()
    {
        transform.position = trackerTransform.position;
    }
}