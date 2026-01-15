using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewRay : MonoBehaviour
{
    //public GameObject RaycastRoom;
    public GameObject Renderer; //벽
    public RectTransform uiRectTransform; //UI의 커서
    public int PlayerNumber = 0;

    public LayerMask layerMask;
    public float maxDistance = 150f;
    public float screenX, screenY;

    public bool isCalibration = false;

    private void Update()
    {
        // Vector3 center = new Vector3(rayCamera.pixelWidth / 2, rayCamera.pixelHeight / 2);
        //float distanceFactor = Mathf.Clamp01(Vector3.Distance(transform.position, transform.position) / maxDistance);

        Ray ray = new Ray(transform.position, transform.forward);//rayCamera.ScreenPointToRay(transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        // Plane에 Ray 발사
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            // Plane과 크기 비율이 같은 카메라를 사용하기 때문에
            // 카메라의 높이와 Plane의 높이가 같으면 크기 비율이 같다.
            float planeHeight = Renderer.transform.localScale.z;
            float cameraHeight = Camera.main.pixelHeight / 100;//mainCam.orthographicSize * 2f;
            float ratio = cameraHeight / planeHeight;

            // Ray가 부딪힌 지점을 Plane 좌표계에서의 위치로 변환
            Vector3 localPosition = Renderer.transform.InverseTransformPoint(hit.point);

            // 변환된 좌표를 크기 비율이 같은 UI 좌표계에서의 위치로 변환
            Vector2 uiPosition = new Vector2((localPosition.x * ratio) * screenX, (localPosition.y * ratio) * screenY);
            uiRectTransform.anchoredPosition = uiPosition;
        }
    }

    //public void SetCalibration()
    //{
    //    Vector3 pos = transform.position + transform.forward * 5f;
    //    GameObject rederer = Instantiate(Renderer, pos, transform.rotation, RaycastRoom.transform);
    //    Renderer = rederer;
    //    isCalibration = true;
    //}

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //씬이 변경 되었을 때 컷신 UI 제거
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiRectTransform = AimManager.Inst.uiRectTransforms[PlayerNumber];
    }
}

