using System;
using UnityEngine;
using Valve.VR;

/// <summary>
/// 특정 트래커(예: 왼발, 오른손 등)의 회전 보정 데이터를 저장하고 로드하며 적용하는 클래스
/// </summary>
public class TrackerCal : MonoBehaviour
{
    [SerializeField] private string strJson = string.Empty;   // (사용하지 않음) JSON 문자열 저장용 (디버그용으로 추정)
    [SerializeField] private CalibrationData calibrationData; // 현재 보정 데이터

    [SerializeField] private EIndex deviceType;               // 이 오브젝트에 연결된 트래커의 종류 (예: 왼발, 오른손 등)
    [SerializeField] private Quaternion calibrationRotation;  // 저장된 보정 회전값
    [SerializeField] private GameObject parentObj;            // 보정을 적용할 오브젝트 (부모 오브젝트)

    [SerializeField] bool IsPlaying;

    private void Start()
    {
        TrackerJsonManager.LoadJsonData();
        LoadTrackerCalibration();
    }

    private void Update()
    {
        if (!IsPlaying)
        {
            // W 키: 모든 보정 데이터를 JSON 파일로 저장
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SaveCal();
                TrackerJsonManager.SaveJsonData();
            }

            // E 키: JSON 파일로부터 모든 보정 데이터를 불러오기
            if (Input.GetKeyDown(KeyCode.E))
            {
                TrackerJsonManager.LoadJsonData();
                LoadTrackerCalibration();
            }
        }
    }

    #region 켈리브레이션 값 게산 및 저장
    /// <summary>
    /// 현재 트래커의 회전을 보정 데이터로 저장하고 JSON 매니저에 추가
    /// </summary>
    public void SaveCal()
    {
        // transform.localRotation을 직접 사용하는 것이 더 정확하나, 회전 축을 재배열하여 저장하고 있음
        calibrationRotation = new Quaternion(
            transform.localRotation.x,
            transform.localRotation.z, // y와 z가 서로 교환됨 (사용 의도 확인 필요)
            transform.localRotation.y,
            transform.localRotation.w
        );

        // 오일러 각도로 저장 (z와 y를 교환)
        calibrationData = new CalibrationData
        {
            TrackerDevice = deviceType.ToString(),
            rotX = calibrationRotation.eulerAngles.x,
            rotY = calibrationRotation.eulerAngles.z,
            rotZ = calibrationRotation.eulerAngles.y
        };

        //계산한 켈리브레이션 값을 JSON 데이터에 추가
        TrackerJsonManager.AddCalbrationData(calibrationData);
    }
    #endregion

    #region 켈리브레이션 값 호출
    /// <summary>
    /// JSON 데이터에서 해당 트래커의 보정 데이터를 불러와서 적용
    /// </summary>
    public void LoadTrackerCalibration()
    {
        calibrationData = TrackerJsonManager.GetCalbrationData(deviceType.ToString());

        if (calibrationData != null)
        {
            // 저장된 오일러 각도를 Quaternion으로 변환
            calibrationRotation = Quaternion.Euler(calibrationData.rotX, calibrationData.rotZ, calibrationData.rotY);

            Quaternion correction = Quaternion.Inverse(calibrationRotation); //보정을 위한 역회전
            parentObj.transform.localRotation = correction;                  //보정 적용
        }
        else
        {
            Debug.LogWarning("No calibration data found for " + deviceType.ToString());
        }
    }
    #endregion
}
