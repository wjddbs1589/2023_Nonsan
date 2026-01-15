using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    TextMeshProUGUI displayText;
    float duration = 1f;
    public float Score = 0;

    float elapsedTime = 0;
    public int playerNum;

    void Awake()
    {
        elapsedTime = 0f;
        displayText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(InterpolateNumber());
    }

    IEnumerator InterpolateNumber()
    {
        Score = GameManager.Inst.Score[playerNum];

        float elapsedTime = 0f; // 이 부분을 추가하여 elapsedTime을 각 코루틴 호출마다 초기화

        while (elapsedTime < duration)
        {
            // 보간된 숫자 계산
            float currentValue = Mathf.Lerp(0, Score, elapsedTime / duration);

            // UI Text에 표시
            displayText.text = currentValue.ToString("F0"); // 소수점 이하 자리 없이 정수로 표시

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 보간이 완료된 후 최종 값을 표시
        displayText.text = Score.ToString("F0");
    }

}
