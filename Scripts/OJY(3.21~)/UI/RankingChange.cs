using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingChange : MonoBehaviour
{
    public RectTransform[] uiElements; // 대상 UI 엘리먼트들
    public float[] targetYPositions; // 각 UI 엘리먼트의 목표 Y 좌표
    public float interpolationTime = 2.0f; // 보간 시간 (2초)

    private float[] startPositions; // 각 UI 엘리먼트의 시작 Y 좌표
    private float startTime;

    private void Awake()
    {
        // 시작 시간 및 시작 Y 좌표 저장
        startTime = Time.time;
        startPositions = new float[uiElements.Length];
        for (int i = 0; i < uiElements.Length; i++)
        {
            startPositions[i] = uiElements[i].anchoredPosition.y;
        }

        MoveUIElements();
    }

    private void MoveUIElements()
    {
        float elapsedTime = Time.time - startTime;

        // 각 UI 엘리먼트의 위치 업데이트
        for (int i = 0; i < uiElements.Length; i++)
        {
            float t = Mathf.Clamp01(elapsedTime / interpolationTime);
            float newY = Mathf.Lerp(startPositions[i], targetYPositions[i], t);

            Vector2 newPosition = uiElements[i].anchoredPosition;
            newPosition.y = newY;
            uiElements[i].anchoredPosition = newPosition;
        }

        // 보간이 끝나지 않았으면 계속 호출
        if (elapsedTime < interpolationTime)
        {
            Invoke("MoveUIElements", 0);
        }
    }
}
