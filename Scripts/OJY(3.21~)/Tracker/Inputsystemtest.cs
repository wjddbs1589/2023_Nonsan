using UnityEngine;
using Valve.VR;

namespace Valve.VR
{
    public enum EIndex
    {
        None = -1,
        Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
        Device1,
        Device2,
        Device3,
        Device4,
        Device5,
        Device6,
        Device7,
        Device8,
        Device9,
        Device10,
        Device11,
        Device12,
        Device13,
        Device14,
        Device15,
        Device16
    }

    public class Inputsystemtest : MonoBehaviour
    {
        public float speed;
        private bool selectIndex = false;

        public EIndex index;

        [Tooltip("If not set, relative to parent")]
        public Transform origin;

        public bool isValid { get; private set; }

        private SteamVR_Events.Action newPosesAction;

        public Inputsystemtest()
        {
            newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
        }

        private void Awake()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            var render = SteamVR_Render.instance;
            if (render == null)
            {
                enabled = false;
                return;
            }

            newPosesAction.enabled = true;
        }

        private void OnDisable()
        {
            newPosesAction.enabled = false;
            isValid = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.W))
            {
                selectIndex = !selectIndex;
            }
        }

        private void OnGUI()
        {
            if (selectIndex)
            {
                GUILayout.Label("Select an Index:");
                string[] indexOptions = {
                    "Hmd", "Device1", "Device2", "Device3", "Device4", "Device5",
                    "Device6", "Device7", "Device8", "Device9", "Device10",
                    "Device11", "Device12", "Device13", "Device14", "Device15", "Device16"
                };

                index = (EIndex)GUILayout.SelectionGrid((int)index, indexOptions, 1);
                GUILayout.Label("Selected Index: " + index.ToString());
            }
        }

        public void SetEIndex(int idx)
        {
            if (System.Enum.IsDefined(typeof(EIndex), idx))
            {
                index = (EIndex)idx;
                Debug.Log($"{name}: Index set to {index}");
            }
            else
            {
                index = EIndex.None;
                Debug.LogWarning($"{name}: Invalid index assigned!");
            }
        }

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {
            if (index == EIndex.None)
            {
                Debug.Log($"{name}: Index is None, skipping pose update.");
                return;
            }

            int i = (int)index;

            isValid = false;
            if (poses.Length <= i)
            {
                Debug.LogWarning($"{name}: poses.Length <= index ({i})");
                return;
            }

            if (!poses[i].bDeviceIsConnected || !poses[i].bPoseIsValid)
            {
                Debug.LogWarning($"{name}: Device not connected or pose invalid.");
                return;
            }

            isValid = true;
            var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            // 회전만 적용
            if (origin != null)
            {
                transform.rotation = origin.rotation * pose.rot;
            }
            else
            {
                transform.rotation = pose.rot;
            }
        }
    }
}
