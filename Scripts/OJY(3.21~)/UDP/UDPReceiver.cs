using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UDP_Car;
using UnityEngine.Playables;
using System.Collections;

public class UDPReceiver : MonoBehaviour
{
    [SerializeField] static UDPReceiver instance;
    public static UDPReceiver Inst => instance;

    private UdpClient m_Client;
    //public int Player;
    //public string m_Ip;
    public int m_Port;
    public ToServerPacket m_SendPacket = new ToServerPacket();
    public ToClientPacket m_ReceivePacket = new ToClientPacket();
    public ToPlayerPacket m_PlayerPacket = new ToPlayerPacket();
    private IPEndPoint m_RemoteIpEndPoint;

    public int playerCount = 0;
    public GameObject Intro;
    public GameObject waitImage;
    public PlayableDirector timeLine;
    public Trigger trigger;
    private void Awake()
    {
        InitClient();
    }
    void Update()
    {
        Receive();
    }

    void OnApplicationQuit()
    {
        CloseClient();
    }
    
    void InitClient()
    {
        if (instance == null)
        {
            instance = this;
            m_Client = new UdpClient(m_Port);
            m_Client.Client.Blocking = false;
            m_RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Receive()
    {
        try
        {
            byte[] bytes = m_Client.Receive(ref m_RemoteIpEndPoint);
            Debug.Log($"[Receive] Remote IpEndPoint : {m_RemoteIpEndPoint.ToString()} Size : {bytes.Length} byte");
            m_PlayerPacket = ByteArrayToStruct<ToPlayerPacket>(bytes);
            DoReceivePacket(m_PlayerPacket); // 받은 값 처리
        }
        catch (Exception ex)
        {
            //Debug.Log(ex.ToString());
        }

    }

    
    /// <summary>
    /// UDP프로그램에서 게임실행 버튼을 눌렀을 때
    /// </summary>
    /// <param name="packet"></param>
    void DoReceivePacket(ToPlayerPacket packet)
    {
        if (GameManager.Inst.gameEnd)
        {            
            playerCount = packet.playerCount; //플레이어 수 저장
            Intro.SetActive(true);            //인트로 재생
            StartCoroutine(ImageOff());       //인트로 영상재생까지 딜레이가 약간 있으므로 이미지를 잠깐 지연후 비활성화 
            GameManager.Inst.useRecord = packet.useRecord;
            GameManager.Inst.ResetPlayerSetting(playerCount); //플레이어 수에 맞게 리셋
            timeLine = FindObjectOfType<PlayableDirector>();  //타임라인 찾아서
            timeLine.enabled = true;                          //재생시킴 
            GameManager.Inst.gameEnd = false;                 //게임 종료여부 변경
            trigger.ListDevices();            //플레이어 수 만큼 트리거 정렬

            GameManager.Inst.GameReset();     //게임 내 값 초기화
        }
    }   
    
    IEnumerator ImageOff()
    {
        yield return new WaitForSeconds(0.3f);
        waitImage.SetActive(false);
    }
    void CloseClient()
    {
        m_Client.Close();
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
}
