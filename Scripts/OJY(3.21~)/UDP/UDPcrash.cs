using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;

public class UDPcrash : MonoBehaviour
{
    private UdpClient m_Client;   // UDP 클라이언트 객체

    private IPEndPoint m_RemoteIpEndPoint;  // 원격 IP 주소와 포트 정보
    public string m_Ip;                    // 서버 IP 주소
    public int m_Port;                     // 서버 포트 번호

    // 패킷 정의
    public ToClientPacket m_ClientPacket = new ToClientPacket();

    void Start()
    {
        Application.targetFrameRate = 60;
        InitClient();   // 클라이언트 초기화
    }

    void OnApplicationQuit()
    {
        CloseClient();  // 애플리케이션 종료 시 클라이언트 닫기
    }

    /// <summary>
    /// 클라이언트 생성 및 초기화
    /// </summary>
    void InitClient()
    {
        m_Client = new UdpClient();
        m_Client.Client.Blocking = false;
        m_RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    // 데이터를 서버로 전송하는 함수
    public void Send()
    {
        m_ClientPacket.Crash_Dir = 7;

        try
        {
            //SetSendPacket();
            byte[] bytes = StructToByteArray(m_ClientPacket);
            m_Client.Send(bytes, bytes.Length, m_Ip, m_Port);
            Debug.Log($"[Send] {m_Ip}:{m_Port} Size : {bytes.Length} byte");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return;
        }


    }
    public void Crash()
    {
        Send();
    }

    // 클라이언트 닫기
    void CloseClient()
    {
        m_Client.Close();
    }

    // C# 구조체를 바이트 배열로 변환하는 함수
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

    // 바이트 배열을 C# 구조체로 변환하는 함수
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
}
