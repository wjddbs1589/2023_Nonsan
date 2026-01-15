using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Camera cameraToCheck; // 검사할 카메라
    public float maxDistance = 100f; // 최대 거리
    public int maxActiveLights = 8; // 최대 활성화되는 라이트 개수

    private Light[] lights; // 모든 라이트를 저장할 배열

    private void Update()
    {
        // Scene에서 모든 Light 컴포넌트를 찾아서 배열에 저장합니다.
        lights = FindObjectsOfType<Light>();

        List<Light> visibleLights = new List<Light>();

        // 카메라에 들어오는 조명들만을 대상으로 거리에 따라 정렬합니다.
        foreach (Light light in lights)
        {
            Vector3 viewportPos = cameraToCheck.WorldToViewportPoint(light.transform.position);

            // 카메라 시야에 들어온 조명들만 선택합니다.
            if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0 &&
                Vector3.Distance(cameraToCheck.transform.position, light.transform.position) <= maxDistance)
            {
                visibleLights.Add(light);
            }
        }

        // 가까운 순서대로 최대 maxActiveLights 개의 라이트를 활성화합니다.
        for (int i = 0; i < visibleLights.Count; i++)
        {
            if (i < maxActiveLights)
            {
                visibleLights[i].enabled = true;
            }
            else
            {
                visibleLights[i].enabled = false;
            }
        }
    }
}
