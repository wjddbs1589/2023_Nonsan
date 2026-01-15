using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class PlayerSetting : MonoBehaviour
{
    private static PlayerSetting instance;
    public static PlayerSetting Inst => instance;

    //canvas 
    public CanvasGroup Fade_img;
    public GameObject Lv1to2CutVideo;
    public GameObject Lv2to3CutVideo;

    public GameObject LoadingBar;
    public Text loadingText;
    public Image loadingBar;
    public GameObject loadingImage;
    public GameObject Tip;
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //씬전환 & 게임 세팅 & 시작
    public void StartGame()
    {
        if (GameManager.Inst.sceneNum == 1 || GameManager.Inst.sceneNum == 0)
        {
            Fade_img.DOFade(1, .5f)
           .OnStart(() =>
           {
               GameManager.Inst.sceneChange = true;
               Fade_img.blocksRaycasts = true;
           })
           .OnComplete(() =>
           {
               loadingImage.SetActive(true);
               LoadingBar.SetActive(true);
               Tip.SetActive(true);

               StartCoroutine("LoadScene", GameManager.Inst.scenename);
           });
        }
        else
        {
            GameManager.Inst.sceneChange = true;
            Fade_img.blocksRaycasts = true;

            if (GameManager.Inst.sceneNum == 2)
            {
                Lv1to2CutVideo.SetActive(true);
            }
            if (GameManager.Inst.sceneNum == 3)
            {
                Lv2to3CutVideo.SetActive(true);
            }

            LoadingBar.SetActive(true);
            Tip.SetActive(true);

            StartCoroutine("LoadScene", GameManager.Inst.scenename);
        }
    }

    /// <summary>
    /// 로딩보다 영상이 먼저 끝나면 로딩화면 실행
    /// </summary>
    public void SetActiveLoadingImage()
    {
        loadingImage.SetActive(true);
    }

    /// <summary>
    /// 씬이 넘어가는동안 움직일 수 있는 로딩 화면
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;        

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += (Time.deltaTime / 2);

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true;                    
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90)
                {
                    past_time = 0;
                    Fade_img.DOFade(1, 1.25f);
                }
            }

            loadingText.text = percentage.ToString("   0") + "%";
            loadingBar.fillAmount = (percentage / 100);
        }
        
        GameManager.Inst.progressBar.gameObject.SetActive(true);        
    }

    void BulletReload()
    {
        for (int i = 0;i<4;i++)
        {
            GameManager.Inst.ShotScript[i].currentBullet = 20;
            GameManager.Inst.UImanager.bulletText[i].text = "20/20";
        }
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //씬이 변경 되었을 때 컷신 UI 제거
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Inst.sceneChange = false; //씬 변경 진행 여부 변경
        BulletReload();  //총알 재 장전

        //바뀐 씬이 0일때 초기화
        if (GameManager.Inst.sceneNum == 0)
        {
            GameManager.Inst.UIoff(); //UI 닫기
            GameManager.Inst.udpReceiver.waitImage.SetActive(true);  //대기 이미지 활성화
            GameManager.Inst.bgm.SetActive(false); //배경음악 종료
        }
        else
        {
            GameManager.Inst.ActiveSetting(true);
            StartCoroutine(GameManager.Inst.progressBar.TimerStart(GameManager.Inst.sceneNum)); // 타이머 시작
        }

        //바뀐 씬이 1번이면
        if (GameManager.Inst.sceneNum == 1)
        {
            GameManager.Inst.udpReceiver.waitImage.SetActive(false); //대기 이미지 비활성화
            GameManager.Inst.bgm.SetActive(true); //배경음악 재생
        }

        //라운드별 컷신 비 활성화
        Lv1to2CutVideo.SetActive(false);
        Lv2to3CutVideo.SetActive(false);

        loadingImage.SetActive(false); //로딩 이미지 비활성화
        LoadingBar.SetActive(false);   //로딩 게이지 비활성화
        Tip.SetActive(false);          //팁 텍스트 비활성화
        Fade_img.DOFade(0, 1f);        //페이드 인
    }
}

