using Cinemachine;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UDP_Car;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using ViveTrackerSerialManager;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] static GameManager instance = null;
    public static GameManager Inst => instance;
    //---------------------------------------------
    //플레이어의 사격 가능 여부를 조절
    Shot[] shotScript = new Shot[4];                     
    public Shot[] ShotScript => shotScript;
    //----------------------------------------------
    //Shot스크립트가 들어있는 플레이어 게임오브젝트를 저장 *현재 사용되지 않음
    GameObject[] playerObject = new GameObject[4];   //플레이어 오브젝트 저장
    public GameObject[] PlayerObject => playerObject;//PlayerObject통해 접근

    //----------------------------------------------
    //인원수
    public int playerCount;
    int totalSceneCount = 3;
    public int sceneNum = 0;
    //----------------------------------------------
    //소리 관리
    public GameObject bgm;

    float playerShotVolume = 1f;
    public float PlayerShotVolume => playerShotVolume;

    float enemyShotVolume = .4f;
    public float EnemyShotVolume => enemyShotVolume;
    //----------------------------------------------
    PlayerSetting playerSetting;
    public PlayerSetting PlayerSetting => playerSetting;
    //----------------------------------------------
    public GameObject[] controllers = new GameObject[4];
    //----------------------------------------------
    //게임 시작 관리
    [HideInInspector] public bool gameEnd = true;
    [HideInInspector] public string scenename;
    [HideInInspector] public bool gameStart = false;
    [HideInInspector] public UDPReceiver udpReceiver;
    [HideInInspector] public UDPSender udpSender;
    //----------------------------------------------
    //시뮬레이션 녹화 관리
    [HideInInspector] public bool SceneStart = false;
    [HideInInspector] public bool useRecord = false;
    //----------------------------------------------
    //점수관리 => 사람 = 50, 물건(차량 및 폭발물) = 100, 중간보스 = 300, 최종보스 = 500 => 점수판 , 현재점수 표시
     public int TotalkillCount = 0;
     public int TotalScore = 0;

     public int[] killCount = {0,0,0,0};
     public int[] Score = {0,0,0,0};

    //----------------------------------------------
    //UI 관리
    public ProgressBar progressBar;
    public GameObject UICanvas;
    [HideInInspector] public UIManager UImanager;
    //----------------------------------------------
    private void Awake()
    {
        //udp연결 함수 가져옴
        udpReceiver = FindObjectOfType<UDPReceiver>();
        udpSender = FindObjectOfType<UDPSender>();
        playerSetting = FindObjectOfType<PlayerSetting>();   //총기사용 세팅 정보 가져옴
        UImanager = FindObjectOfType<UIManager>();        

        killCount = new int[4];
        Score = new int[4];

        if (instance == null)
        {
            instance = this;
            instantiate();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        //게임 시작시 점수판 초기화
        for (int i = 0; i < 4; i++)
        {
            killCount[i] = 0;
            Score[i] = 0;
        }
    }

    bool stop = false;
    private void Update()
    {
        //씬 넘기기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneChanger();
        }

        //일시정지
        if (Input.GetMouseButtonDown(1))
        {
            if (stop)
            {
                stop = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                stop = true;
                Time.timeScale = 0.0f;
            }
            
        }

        //프로그램 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// 라운드 마다 초기화 진행
    /// </summary>
    public void instantiate()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex; //현재 씬 번호 가져옴
        shotScript = FindObjectsOfType<Shot>();              //사격할 플레이어 정보 가져옴
        PlayerSet(shotScript);
    }

    /// <summary>
    /// 게임 시작시 선택한 인원에 따라 세팅 리셋
    /// </summary>
    /// <param name="Count"></param>
    public void ResetPlayerSetting(int Count)
    {
        playerCount = Count;

        //플레이어 숫자만큼 세팅        
        //PlayerSet(shotScript);              //플레이어 활성화
        AimManager.Inst.PlayerAimSetting(); //에임 이미지 및 컨트롤러 활성화
    }

    //---------------------------------------------------------------------
    //씬이 로드된 후 호출되는 이벤트 핸들러 등록
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //이벤트 핸들러 등록 해제
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        instantiate();
    }
    
    //---------------------------------------------------------------------
    /// <summary>
    /// 다음 씬으로 넘기고 마지막 씬이면 다시 튜토리얼로 이동
    /// </summary>
    public void SceneChanger()
    {
        if (!sceneChange)
        {
            sceneNum++;        //씬 번호 변경
            if (sceneNum > totalSceneCount)
            {
                sceneNum = 0;
                GameManager.Inst.gameEnd = true; //게임 종료여부 변경                    
            }
            scenename = "NonsanChasing_Car_Lv" + sceneNum;

            playerSetting.StartGame();
        }
    }

    [HideInInspector] public bool sceneChange = false;
    //---------------------------------------------------------------------
    /// <summary>
    /// 사격 가능여부 및 에임, UI 숨기기 여부 변경. 컷씬등 사격불가 장면에서 사용
    /// </summary>
    /// <param name="able"></param>
    public void ActiveSetting(bool able)
    {
        if(sceneNum != 0)
        {
            progressBar.gameObject.SetActive(able);
        }
        
        for (int i = 0; i < playerCount; i++)
        {
            AimManager.Inst.aimImage[i].gameObject.SetActive(able);
            UImanager.UIobject[i].SetActive(able);
            ShotScript[i].canShot = able;
        } 
    }

    /// <summary>
    /// 플레이어 순서정렬 및 인원수만큼 게임오브젝트 활성화
    /// </summary>
    /// <param name="shot">shot Script의 배열</param>
    void PlayerSet(Shot[] shot)
    {
        Shot[] temps = new Shot[shot.Length]; 
        //플레이어 순서 정렬
        for (int i = 0; i < shot.Length; i++)
        {
            string name = shot[i].name;
            if (name.Contains("1"))
            {
                temps[0] = shot[i];
            }
            else if (name.Contains("2"))
            {
                temps[1] = shot[i];
            }
            else if(name.Contains("3"))
            {
                temps[2] = shot[i];
            }
            else if(name.Contains("4"))
            {
                temps[3] = shot[i];
            }
        }

        //정렬된 순서대로 저장
        for (int i = 0; i < shot.Length; i++)
        {
            shot[i] = temps[i];
            shot[i].PlayerNumber = i;             //플레이어 번호 지정
            shot[i].currentBullet = shot[i].maxBullet;

            playerObject[i] = shot[i].gameObject; //shot클래스를 가진 대상의 게임오브젝트 정보 저장
        }

        if (sceneNum!=0)
        {
            PlayerActive();
        }
    }

    /// <summary>
    /// 플레이어 수에 맞게 플레이어 및 UI 활성화
    /// </summary>
    public void PlayerActive()
    {
        for (int i = 0; i < ShotScript.Length; i++)
        {
            if (playerCount <= i)
            {
                shotScript[i].gameObject.SetActive(false);
                UImanager.UIobject[i].SetActive(false);
            }
            else
            {
                shotScript[i].gameObject.SetActive(true);
                UImanager.UIobject[i].SetActive(true);
            }
        }
    }

    
    public void UIoff()
    {
        for (int i = 0; i < 4; i++)
        {
            shotScript[i].canShot = false;
            UImanager.UIobject[i].SetActive(false);
        }
        progressBar.gameObject.SetActive(false);
    }

    //-----------초당 프레임을 계산하여 좌측 상단에 표시해주는 GUI-----------
    private void OnGUI()
    {
        int frameRate = (int)(1 / Time.deltaTime); // 현재 프레임 계산

        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + frameRate, style); // 프레임 텍스트 표시
    }

    public void GameReset()
    {
        //전체 점수및 킬수 초기화
        TotalkillCount = 0;
        TotalScore = 0; 

        //개인점수 및 킬수 초기화
        for (int i = 0; i < 4; i++)
        {
            killCount[i] = 0;
            Score[i] = 0;
        }

        //UI텍스트 초기화
        UImanager.Instantiate();
    }
}
