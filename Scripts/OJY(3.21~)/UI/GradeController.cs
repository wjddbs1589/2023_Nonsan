using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GradeController : MonoBehaviour
{
    int[] allScore = new int[4];         //플레이어들의 점수 저장
    int[] highScorePlayer = { 0,1,2,3 }; //점수 순서대로 정렬할 플레이어 번호 배열

    RectTransform[] PlayerUI;       //대상 UI 엘리먼트들
    float[] targetYPositions;       //각 UI 엘리먼트의 목표 Y 좌표
    float[] startPositions;         //각 시작 Y 좌표
    float interpolationTime = 1.0f; //보간 시간 (2초)
    float startTime;

    private void Awake()
    {
        PlayerUI = new RectTransform[4];
        targetYPositions = new float[4];
        startPositions = new float[4];

        for (int i = 0; i < GameManager.Inst.playerCount; i++)
        {
            PlayerUI[i] = transform.GetChild(i).GetComponent<RectTransform>();
            PlayerUI[i].transform.gameObject.SetActive(true);
            targetYPositions[i] = i * -180f;
        }
    }
    private void OnEnable()
    {
        //플레이어가 두명 이상이면 정렬후 위치 변환
        if (GameManager.Inst.playerCount > 1)
        {
            ResetHighScore();
        }
    }

    /// <summary>
    /// 점수에 따라 높은 점수와 플레이어 번호를 앞으로 정렬 
    /// </summary>
    void ResetHighScore()
    {
        startTime = Time.time;

        //플레이어별 점수 받아옴
        for (int i = 0; i < GameManager.Inst.playerCount; i++)
        {
            allScore[i] = GameManager.Inst.Score[i];
            highScorePlayer[i] = i; //플레이어 번호를 순서대로 설정
        }

        for (int i = 0; i < GameManager.Inst.playerCount - 1; i++)
        {
            for (int j = 0; j < GameManager.Inst.playerCount - i - 1; j++)
            {
                if (allScore[j] < allScore[j + 1])
                {
                    int temp = allScore[j];
                    allScore[j] = allScore[j + 1];
                    allScore[j + 1] = temp;

                    int temp2 = highScorePlayer[j];
                    highScorePlayer[j] = highScorePlayer[j + 1];
                    highScorePlayer[j + 1] = temp2;
                }
            }
        }
        MoveUIElements();
    }

    /// <summary>
    /// 높은점수 번호부터 윗자리로 이동
    /// </summary>
    private void MoveUIElements()
    {
        for (int i = 0; i < GameManager.Inst.playerCount; i++)
        {
            startPositions[i] = PlayerUI[i].anchoredPosition.y;
        }

        float elapsedTime = Time.time - startTime;

        // 각 UI 엘리먼트의 위치 업데이트
        for (int i = 0; i < GameManager.Inst.playerCount; i++)
        {
            float t = Mathf.Clamp01(elapsedTime / interpolationTime);
            float newY = Mathf.Lerp(startPositions[highScorePlayer[i]], targetYPositions[i], t);

            Vector2 newPosition = PlayerUI[highScorePlayer[i]].anchoredPosition;
            newPosition.y = newY;
            PlayerUI[highScorePlayer[i]].anchoredPosition = newPosition;
        }

        // 보간이 끝나지 않았으면 계속 호출
        if (elapsedTime < interpolationTime)
        {
            Invoke("MoveUIElements", 0);
        }
    }
}
