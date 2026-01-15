using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("플레이어별 전체UI 게임오브젝트")]
    public GameObject[] UIobject = new GameObject[4];            //플레이어 UI 오브젝트

    [Header("점수UI")]
    public TextMeshProUGUI[] scoreText = new TextMeshProUGUI[4]; //점수 UI 오브젝트

    [Header("총알UI")]
    public TextMeshProUGUI[] bulletText = new TextMeshProUGUI[4];//총알 개수 UI 오브젝트    

    [Header("체력UI")]
    public TextMeshProUGUI[] hpText = new TextMeshProUGUI[4];    //플레이어 체령 UI
    int[] PlayerHPpercent = new int[4];                          //플레이어 체력 저장 변수
    Animator anim;                                               //피격 애니메이션 재생

    bool[] activeDecrease = new bool[4];      //이미 피격 당하여 체력이 깎이고 있는지 

    public TextMeshProUGUI killtext;

    //플레이어 선택
    int selectNum;

    //보간에 걸리는 시간
    float hpDuration = 1.0f; 
    float scoreDuration = 0.05f;

    private void Awake()
    {
        for (int i =0;i<4;i++)
        {
            anim = transform.GetComponent<Animator>();
            activeDecrease[i] = false;
            PlayerHPpercent[i] = 100;
        }
    }

    /// <summary>
    /// UI텍스트 초기화
    /// </summary>
    public void Instantiate()
    {
        for (int i = 0; i < 4; i++)
        {
            scoreText[i].text = $"0";
            bulletText[i].text = $"20/20";
            PlayerHPpercent[i] = 100;
            hpText[i].text = $"{PlayerHPpercent[i]}%";
            activeDecrease[i] = false;
        }
        killtext.text = $"{GameManager.Inst.TotalkillCount}";
    }

    //점수UI----------------------------------------------------------------------------------
    private bool isScoreAnimating = false;                          //점수 증가 진행 여부
    private Queue<(int, int)> scoreQueue = new Queue<(int, int)>(); //큐 사용하여 순차 실행 관리

    private void Update()
    {
        //프레임 마다 점수 진행 여부 및 큐 남은 개수 확인하여 점수 증가
        if (!isScoreAnimating && scoreQueue.Count > 0)
        {
            var scoreData = scoreQueue.Dequeue();
            StartCoroutine(ScroeTextLerp(scoreData.Item1, scoreData.Item2));
        }
    }

    //점수 추가하고 점수Text 변경
    public void ScoreTextChage(int playerNum, int score, int killCount = 0)
    {
        //튜토리얼이 아닐때
        if (GameManager.Inst.sceneNum != 0)
        {
            //플레이어가 쐈을때 오르는 점수 
            if (playerNum < 4)
            {
                //점수 증가 큐 추가 
                scoreQueue.Enqueue((playerNum, score));

                //킬수 증가
                GameManager.Inst.killCount[playerNum] += killCount; //플레이어 킬수 추가
                GameManager.Inst.TotalkillCount += killCount;       //합산 킬수 추가

                //피격당한 상태가 아닐때 피 1 회복
                if (!activeDecrease[playerNum])
                {
                    PlayerHPpercent[playerNum]++;
                    if(PlayerHPpercent[playerNum] > 100)
                    {
                        PlayerHPpercent[playerNum] = 100;
                    }
                    hpText[playerNum].text = $"{PlayerHPpercent[playerNum]}%";
                }

                killtext.text = $"{GameManager.Inst.TotalkillCount}";
            }
        }
    }
    
    //점수 보간
    private IEnumerator ScroeTextLerp(int playerNum, int score)
    {
        isScoreAnimating = true;

        int startScore = GameManager.Inst.Score[playerNum];
        int endScore = startScore + score;

        float startTime = Time.time;

        while (Time.time < startTime + scoreDuration)
        {
            float progress = (Time.time - startTime) / scoreDuration;
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, progress));
            scoreText[playerNum].text = $"{currentScore}점";
            yield return null;
        }

        scoreText[playerNum].text = $"{endScore}점";  //점수 증가 적립
        GameManager.Inst.Score[playerNum] = endScore; //플레이어 점수 적립
        GameManager.Inst.TotalScore += score;         //합산점수 추가

        isScoreAnimating = false;
    }

    //총알UI----------------------------------------------------------------------------------

    //총알 개수 표시
    public void BulletTextChange(int playerNumber, int currentBullet, int maxBullet = 20)
    {
        bulletText[playerNumber].text = $"{currentBullet}/{maxBullet}";
        if (currentBullet == 0)
        {
            bulletText[playerNumber].text = $"장전중...";
            StartCoroutine(ReloadText(playerNumber));
        }
    }
    //재장전중일때 표시
    IEnumerator ReloadText(int playerNumber)
    {
        yield return new WaitForSeconds(2);
        bulletText[playerNumber].text = $"20/20";
    }

    //체력UI----------------------------------------------------------------------------------


    //체력감소
    public void DecreaseHP()
    {
        selectNum = Random.Range(0,4);    //플레이어 0~3

        //뽑힌 숫자가 현재 플레이 인원보다 작거나 같을때, 체력감소가 진행중이지 않을때
        if ((selectNum <= GameManager.Inst.playerCount - 1)  && !activeDecrease[selectNum])
        {
            StartCoroutine(HpTextLerp(selectNum));
        }
    }

    //체력 변경 보간
    private IEnumerator HpTextLerp(int num)
    {
        activeDecrease[num] = true; //피격 상호작용 진행중

        anim.SetBool("Attack", true);
        int startHP = PlayerHPpercent[num];
        int damage = Random.Range(5, 11); //5~10
        int endHP = Mathf.Clamp(startHP - damage, 0, 100);

        float startTime = Time.time;

        while (Time.time < startTime + hpDuration)
        {
            float progress = (Time.time - startTime) / hpDuration;
            int currentHP = Mathf.RoundToInt(Mathf.Lerp(startHP, endHP, progress));
            hpText[num].text = $"{currentHP}%";

            yield return null;
        }

        PlayerHPpercent[num] = endHP;
        hpText[num].text = $"{endHP}%";

        anim.SetBool("Attack", false);

        activeDecrease[num] = false; //피격 상호작용 진행완료
    }

}
