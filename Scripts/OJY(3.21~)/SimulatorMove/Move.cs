using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Move : MonoBehaviour
{
    float speed = 3f; // Ω∫««µÂ
    float maxAngle = 5f; //√÷¥Î ∞¢µµ
    float angleX = 0f; //∞¢µµ
    float angleZ = 0f; //∞¢µµ

    private List<float> rotationListX = new List<float>();
    private List<float> rotationListZ= new List<float>();
    Vector3 rot;
    public string savePathX = "Assets/RecordedRotationX.txt";
    public string savePathZ = "Assets/RecordedRotationZ.txt";
    private bool isRecording = false;

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Vertical");
        float inputZ = Input.GetAxis("Horizontal");

        angleX = inputX * maxAngle;
        angleZ = -inputZ * maxAngle;


        float currentAngleX = Mathf.LerpAngle(transform.rotation.eulerAngles.x, angleX, speed * Time.deltaTime);
        float currentAngleZ = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angleZ, speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentAngleX, transform.rotation.eulerAngles.y, currentAngleZ);


        if (Input.GetKeyDown(KeyCode.R)) // ≥Ï»≠ Ω√¿€
        {
            rotationListX.Clear();
            rotationListZ.Clear();
            isRecording = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && isRecording) // ≥Ï»≠ ∏ÿ√„
        {
            isRecording = false;
            SaveRecordedRotations();
        }

        if (isRecording)
        {
            if (transform.rotation.eulerAngles.x > 180)
            {
                rot.x = transform.rotation.eulerAngles.x - 360;

            }
            else
            {
                rot.x = transform.rotation.eulerAngles.x;
            }
            if (transform.rotation.eulerAngles.z > 180)
            {

                rot.z = transform.rotation.eulerAngles.z - 360;
            }
            else
            {

                rot.z = transform.rotation.eulerAngles.z;
            }
            rotationListX.Add(rot.x);
            rotationListZ.Add(rot.z);
        }

    }

    void SaveRecordedRotations()
    {
        
        string[] rotationLineX = new string[rotationListX.Count];
        string[] rotationLineZ = new string[rotationListX.Count];
        for (int i = 0; i < rotationListX.Count; i++)
        {
            rotationLineX[i] = rotationListX[i].ToString();
        }

        for (int i = 0; i < rotationListZ.Count; i++)
        {
            rotationLineZ[i] = rotationListZ[i].ToString();
        }
        File.WriteAllLines(savePathX, rotationLineX);
        File.WriteAllLines(savePathZ, rotationLineZ);

        Debug.Log("Recorded rotations saved to: " + savePathX);
    }
}
