using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static RootMotion.FinalIK.VRIKCalibrator;

/// <summary>
/// 개별 트래커의 회전 보정 데이터를 저장하기 위한 클래스
/// </summary>
[Serializable]
public class CalibrationData
{
    public string TrackerDevice = string.Empty; // 트래커 이름 또는 종류 (예: LeftFoot, RightHand 등)
    public float rotX = 0.0f; // X축 회전 값 (오일러 각도)
    public float rotY = 0.0f; // Y축 회전 값 (오일러 각도)
    public float rotZ = 0.0f; // Z축 회전 값 (오일러 각도)
}

/// <summary>
/// 트래커 보정 데이터를 저장, 불러오기, 관리하는 정적 클래스
/// JSON 파일을 통해 데이터를 영구적으로 저장함
/// </summary>
public static class TrackerJsonManager
{
    // 보정 데이터를 저장할 폴더 경로
    private static string dirPath = "C:\\NonsanAimData";

    // JSON 파일 경로 (C:\AimSet.json)
    private static string filePath = Path.Combine(dirPath, "AimSetting.json");

    private static string trackerJson = string.Empty; // JSON 문자열 (디버깅용으로 사용 가능)
    private static List<CalibrationData> calibrationDatas = new List<CalibrationData>(); // 메모리에 보관되는 보정 데이터 리스트

    /// <summary>
    /// 보정 데이터를 리스트에 추가
    /// </summary>
    public static void AddCalbrationData(CalibrationData calibrationData)
    {
        //동일한 TrackerDevice가 있는지 확인
        for (int i = 0; i < calibrationDatas.Count; i++)
        {
            if (calibrationDatas[i].TrackerDevice == calibrationData.TrackerDevice)
            {
                // 수치 갱신
                calibrationDatas[i].rotX = calibrationData.rotX;
                calibrationDatas[i].rotY = calibrationData.rotY;
                calibrationDatas[i].rotZ = calibrationData.rotZ;
                return; // 기존 값 수정 후 함수 종료
            }
        }
    }
    /// <summary>
    /// 특정 트래커 이름에 해당하는 보정 데이터를 반환
    /// </summary>
    public static CalibrationData GetCalbrationData(string deviceName)
    {
        foreach (var item in calibrationDatas)
        {
            if (item.TrackerDevice == deviceName)
            {
                return item; // 일치하는 데이터 반환
            }
        }

        return null; // 없으면 null 반환
    }

    /// <summary>
    /// 현재 메모리에 저장된 보정 데이터를 JSON 형식으로 파일에 저장
    /// </summary>
    public static void SaveJsonData()
    {
        // 디렉토리가 존재하지 않으면 생성
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        Debug.Log(filePath); // 저장 경로 로그 출력

        trackerJson = JsonConvert.SerializeObject(calibrationDatas); // JSON 문자열로 직렬화
        File.WriteAllText(filePath, trackerJson); // 파일에 저장
        Debug.Log(trackerJson); // 저장된 내용 출력
    }

    /// <summary>
    /// 파일로부터 JSON 데이터를 읽고, 메모리에 보정 데이터 리스트를 복원
    /// </summary>
    public static void LoadJsonData()
    {
        Debug.Log(filePath); // 경로 출력

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); // 파일에서 JSON 문자열 읽기

            // JSON 문자열을 리스트로 역직렬화
            calibrationDatas = JsonConvert.DeserializeObject<List<CalibrationData>>(json);

            Debug.Log("Loaded " + calibrationDatas.Count + " calibration entries.");

            // 각 항목 로그 출력
            foreach (var data in calibrationDatas)
            {
                Debug.Log($"Device: {data.TrackerDevice}, Rot: {data.rotX}, {data.rotY}, {data.rotZ}");
            }
        }
        else
        {
            Debug.LogWarning("Calibration file not found at: " + filePath);
            calibrationDatas = new List<CalibrationData>(); // 새 리스트 생성
        }
    }
}
