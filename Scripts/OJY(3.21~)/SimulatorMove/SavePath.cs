using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavePath : MonoBehaviour
{
    [SerializeField]
    public class Vector3Data
    {
        public float x;
        public float y;
        public float z;
    }

    [SerializeField]
    public class Levels
    {
        public Vector3Data Lv1;
        public Vector3Data Lv2;
        public Vector3Data Lv3;

    }
    public bool isRecording = false;
    public string fileName = "JsonData.json";
    //public string filePath = Application.persistentDataPath + "C:/Users/User/AppData/LocalLow/yoyo/SimulatorData";
    public GameObject Cube;

    List<Vector3> s_data = new List<Vector3>();
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            s_data.Clear();
            isRecording = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
           SaveVector3ToJson(s_data);
        }

        if (isRecording)
        {
            Vector3 Pos;// = Cube.transform.rotation.eulerAngles;
            if (Cube.transform.rotation.eulerAngles.x > 180)
            {
                Pos.x = Cube.transform.rotation.eulerAngles.x - 360;

            }
            else
            {
                Pos.x = Cube.transform.rotation.eulerAngles.x;
            }
            if (Cube.transform.rotation.eulerAngles.z > 180)
            {

                Pos.z= Cube.transform.rotation.eulerAngles.z - 360;
            }
            else
            {

                Pos.z = Cube.transform.rotation.eulerAngles.z;
            }
            Pos.y = 0f;
            //SaveVector3ToJson(Pos);
            s_data.Add(Pos);
        }
    }

    private void SaveVector3ToJson(List<Vector3> vector3Value)
    {
        List<Vector3Data> dataList = new List<Vector3Data>();
       // Vector3Data data = new Vector3Data { };
        Vector3[] Pos = new Vector3[vector3Value.Count];

        //for (int i = 0; i < vector3Value.Count-1; i++)
        //{
        //    data.x = vector3Value[i].x;
        //    data.y = vector3Value[i].y;
        //    data.z = vector3Value[i].z;


        //    dataList.Add(data.x);
        //    Debug.Log(data.x);
        //}
        foreach (Vector3 datas in vector3Value)
        {
            Vector3Data data = new Vector3Data
            {
                x = datas.x > 5f ? 4.98f : datas.x,
                y = datas.y > 5f ? 4.98f : datas.y,
                z = datas.z > 5f ? 4.98f : datas.z
            };

            //Vector3Data data = new Vector3Data
            //{
            //    x = datas.x,
            //    y = datas.y,
            //    z = datas.z
            //};

            dataList.Add(data);
            Debug.Log(datas);
        }


        Levels levels = new Levels
        {
           // Lv1 = data
        };

        // JSON 파일에 저장
        //File.Create(filePath);
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(dataList);
        string path = Path.Combine(Application.persistentDataPath+ "/SimulatorData", fileName);
        File.WriteAllText(path, jsonData);
    }

    public void SaveData()
    {
        SaveVector3ToJson(s_data);
    }

}
