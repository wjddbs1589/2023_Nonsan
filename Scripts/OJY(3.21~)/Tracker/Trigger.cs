using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Valve.VR;
using ViveTrackerSerialManager;
using System.Linq;

public class Trigger : MonoBehaviour
{
    [Range(10, 1000)]
    public long LagMillis = 100;

    private Dictionary<int, string> trackerInfo;
    private List<TrackerData> trackerList = new List<TrackerData>();

    [SerializeField]
    private List<Inputsystemtest> inputsystemtests;

    void Start()
    {
        trackerList = TrackerManager.GetTrackers().ToList();
        ListDevices();
    }

    public void ListDevices()
    {
        trackerInfo = new Dictionary<int, string>();

        for (int i = 0; i < SteamVR.connected.Length; ++i)
        {
            ETrackedPropertyError error = new ETrackedPropertyError();
            StringBuilder sb = new StringBuilder();

            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_SerialNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
            string serialNumber = sb.ToString();

            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, OpenVR.k_unMaxPropertyStringSize, ref error);
            string modelNumber = sb.ToString();

            if (!string.IsNullOrEmpty(serialNumber) && modelNumber.Contains("Tracker"))
            {
                trackerInfo[i] = serialNumber;
                Debug.Log($"Tracker found: Index {i} | Serial: {serialNumber}");
            }
        }

        Arrangeposition();
    }

    void Arrangeposition()
    {
        foreach (var item in inputsystemtests)
        {
            item.SetEIndex(-1);
        }

        for (int i = 0; i < inputsystemtests.Count && i < trackerList.Count; i++)
        {
            foreach (var localTracker in trackerInfo)
            {
                // 대소문자 무시하고 공백 제거 후 비교
                if (localTracker.Value.Trim().ToLower() == trackerList[i].serialNumber.Trim().ToLower())
                {
                    inputsystemtests[i].SetEIndex(localTracker.Key);
                    Debug.Log($"Mapped Inputsystem {i} to tracker index {localTracker.Key} ({localTracker.Value})");
                    break;
                }
            }

            if ((int)inputsystemtests[i].index == -1)
            {
                Debug.LogWarning($"Tracker match failed for Inputsystem {i}. Serial: {trackerList[i].serialNumber}");
            }
        }
    }
}
