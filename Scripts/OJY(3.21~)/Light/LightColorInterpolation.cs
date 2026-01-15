using System.Collections;
using UnityEngine;

public class LightColorInterpolation : MonoBehaviour
{
    public Light targetLight; // 조명 오브젝트
    public Color startColor; // 시작 색상
    public Color middleColor; // 중간 색상 (파란색)
    public Color endColor;   // 끝 색상
    public float duration = 4.0f; // 각 보간에 걸리는 시간 (초)

    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(InterpolateColors(startColor, middleColor, duration));
            yield return StartCoroutine(InterpolateColors(middleColor, endColor, duration));
            yield return StartCoroutine(InterpolateColors(endColor, startColor, duration));
        }
    }

    private IEnumerator InterpolateColors(Color from, Color to, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            targetLight.color = Color.Lerp(from, to, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetLight.color = to;
    }
}
