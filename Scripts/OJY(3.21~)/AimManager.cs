using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AimManager : MonoBehaviour
{
    [SerializeField] static AimManager instance = null;
    public static AimManager Inst => instance;
    //---------------------------------------------
    GameManager gameManager;
    //---------------------------------------------
    public Transform[] aimImage;   //캔버스가 가지고 있는 조준점 이미지
    public RectTransform[] uiRectTransforms;
    GameObject[] controller = new GameObject[4];//트래커를 플레이어 수에 맞춰 활성화
    //----------------------------------------------
    public enum GameMode
    {
        City,      //시가전
        Car,       //차량전
        Helicopter //헬기전
    }
    public GameMode ModeSelect = GameMode.Car;
    //---------------------------------------------- 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instantiate();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //매 씬마다 적용될 함수
    public void instantiate()
    {
        gameManager = GameManager.Inst;
        for (int i = 0; i < 4; i++)
        {
            controller[i] = gameManager.controllers[i];
        }
        PlayerAimSetting();
    }

    /// <summary>
    /// 플레이어 수에 맞게 에임이미지 및 에임컨트롤러 활성화
    /// </summary>
    public void PlayerAimSetting()
    {
        if(gameManager==null)
        {
            gameManager = GameManager.Inst;
        }
        for (int i = 0; i < gameManager.ShotScript.Length; i++)
        {
            if (gameManager.playerCount <= i)
            {
                controller[i].gameObject.SetActive(false);
                aimImage[i].gameObject.SetActive(false);
            }
            else
            {
                controller[i].gameObject.SetActive(true);
                aimImage[i].gameObject.SetActive(true);
            }
        }
    }
}
