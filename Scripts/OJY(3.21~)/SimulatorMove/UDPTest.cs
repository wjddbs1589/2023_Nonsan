using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;


public class UDPTest : MonoBehaviour
{
    private UdpClient m_Client;
    public string m_Ip = "192.168.0.10";
    public int m_Port = 61021;
    public ToServerPacket m_SendPacket = new ToServerPacket();
    public ToClientPacket m_ReceivePacket = new ToClientPacket();
    private IPEndPoint m_RemoteIpEndPoint;
    private List<float> recordedRotationX = new List<float>();
    private List<float> recordedRotationZ = new List<float>();
    public string filePathX = "Assets/RecordedRotationX.txt";
    public string filePathZ = "Assets/RecordedRotationZ.txt";
    public GameObject move;

    private bool indexing = false;
    private int index;
    void Start()
    {
        Application.targetFrameRate = 60;
        //ReadFile();
        InitClient();
        InvokeRepeating("Send", 0f, 0.02f);
        

        StartCoroutine(StartSimulator());

        //m_SendPacket.ControlCommand = 0;
        m_SendPacket.FrameCount = 0;
        m_SendPacket.Command = 0;
        m_SendPacket.Heave_Acc = 0;
        m_SendPacket.Roll_Pos = 0;
        m_SendPacket.Pitch_Pos = 0;
        m_SendPacket.Crash_Dir = 0;
        m_SendPacket.Crash_Amp = 0;
    }

    void Update()
    {
      //  Send();
        Receive();
        
        //m_SendPacket.Crash_Dir = 1;
        //m_SendPacket.Crash_Amp = 0;

    }

    void OnApplicationQuit()
    {
        CloseClient();
    }

    void InitClient()
    {
        m_Client = new UdpClient();
        m_Client.Client.Blocking = false;
        m_RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // SendPacket에 배열이 있으면 선언 해 주어야 함.
       // m_SendPacket.m_IntArray = new int[2];
    }

    void SetSendPacket()
    {
        

        if (m_SendPacket.FrameCount <= 2999)
        {
            m_SendPacket.FrameCount += 1;
        }
        else
        {
            m_SendPacket.FrameCount = 0;
        }
        

        if(indexing)
        {

            m_SendPacket.Roll_Pos = recordedRotationX[index];
            m_SendPacket.Pitch_Pos = recordedRotationZ[index];
            move.transform.rotation = Quaternion.Euler(recordedRotationX[index], 0, recordedRotationZ[index]);
            index += 1;
            Debug.Log(index);
            if(index >= recordedRotationX.Count-1)
            {
                indexing = false;
            }
        }
        else
        {
            m_SendPacket.Roll_Pos = 0;
            m_SendPacket.Pitch_Pos = 0;
        }
        


        //실시간 값 추가
        //m_SendPacket.Command = 1;
        //m_SendPacket.Heave_Acc = 1;
        //m_SendPacket.Roll_Pos = move.transform.rotation.eulerAngles.z;//Quaternion.Euler()//;
        //m_SendPacket.Pitch_Pos = move.transform.rotation.eulerAngles.x;

        //if (move.transform.rotation.eulerAngles.x > 180)
        //{

        //    m_SendPacket.Pitch_Pos = move.transform.rotation.eulerAngles.x - 360;
        //}
        //else
        //{

        //    m_SendPacket.Pitch_Pos = move.transform.rotation.eulerAngles.x;
        //}
        //if (move.transform.rotation.eulerAngles.z > 180)
        //{

        //    m_SendPacket.Roll_Pos = move.transform.rotation.eulerAngles.z - 360;
        //}
        //else
        //{

        //    m_SendPacket.Roll_Pos = move.transform.rotation.eulerAngles.z;
        //}

    }


    void Send()
    {
        try
        {
            SetSendPacket();
            byte[] bytes = StructToByteArray(m_SendPacket);
            m_Client.Send(bytes, bytes.Length, m_Ip, m_Port);
            Debug.Log($"[Send] {m_Ip}:{m_Port} Size : {bytes.Length} byte");
        }

        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return;
        }
    }

    void Receive()
    {
        try
        {
            byte[] bytes = m_Client.Receive(ref m_RemoteIpEndPoint);
            //Debug.Log($"[Receive] Remote IpEndPoint : {m_RemoteIpEndPoint.ToString()} Size : {bytes.Length} byte");
            m_ReceivePacket = ByteArrayToStruct<ToClientPacket>(bytes);
            DoReceivePacket(); // 받은 값 처리
        }

        catch (Exception ex)
        {
            Debug.Log($"UDPTest : " + ex);
            //Debug.Log(ex.ToString());
            return;
        }

    }

    void DoReceivePacket()
    {
        //Debug.LogFormat($"BoolVariable = {m_ReceivePacket.m_BoolVariable} " +
        //    $"IntlVariable = {m_ReceivePacket.m_IntVariable} " +
        //    $"m_IntArray[0] = {m_ReceivePacket.m_IntArray[0]} " +
        //    $"m_IntArray[1] = {m_ReceivePacket.m_IntArray[1] } " +
        //    $"FloatlVariable = {m_ReceivePacket.m_FloatlVariable} " +
        //    $"StringlVariable = {m_ReceivePacket.m_StringlVariable}");
        ////출력: BoolVariable = True IntlVariable = 13 m_IntArray[0] = 7 m_IntArray[1] = 47 FloatlVariable = 2020 StringlVariable = Coder Zero
    }

    void CloseClient()
    {
        //m_Client.Close();
    }

    byte[] StructToByteArray(object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    T ByteArrayToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return obj;
    }

    IEnumerator StartSimulator()
    {
        yield return new WaitForSeconds(5f);
        m_SendPacket.Command = 1;
        yield return new WaitForSeconds(5f);
        m_SendPacket.Command = 2;
        yield return new WaitForSeconds(15f);
        m_SendPacket.Command = 3;
        yield return new WaitForSeconds(15f);
        m_SendPacket.Command = 4;
        indexing = true;

    }

    void ReadFile()
    {
        string[] lineX = File.ReadAllLines(filePathX);
        string[] lineZ = File.ReadAllLines(filePathZ);

        float x = 0;
        float z = 0;

        foreach(string line in lineX)
        {
            if (float.TryParse(line, out x))
                recordedRotationX.Add(x);
        }
        foreach (string line in lineZ)
        {
            if (float.TryParse(line, out z))
                recordedRotationZ.Add(z);
        }

        Debug.Log("X : " + lineX[index] + "Z : " + lineZ[index]);
    }
}
