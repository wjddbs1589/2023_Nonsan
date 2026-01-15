using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EnemyListUI : MonoBehaviour
{
    GameObject[] EnemyList;
    public float timer = 5.0f;
    public GameObject TimerImage;
    public GameObject UIname;

    float nowTime = 0;
    int pageNumber = 0;

    bool start = false;

    Animator anim;

    public PlayableDirector PD;

    private void Awake()
    {
        EnemyList = new GameObject[transform.childCount];
        for(int i = 0;i< EnemyList.Length; i++)
        {
            EnemyList[i] = transform.GetChild(i).gameObject;
        }
        anim = GetComponent<Animator>();
        PD.Pause();
    }
    private void Start()
    {
        StartCoroutine(StartPage());
    }

    private void Update()
    {
        if (start)
        {
            //정보 페이지가 남아 있을때
            if (pageNumber < transform.childCount)
            {
                EnemyList[pageNumber].SetActive(true);
                if (nowTime <= 5)
                {
                    nowTime += Time.deltaTime;
                    float rate = nowTime / timer;
                    TimerImage.GetComponent<Image>().fillAmount = Mathf.Lerp(1.0f, 0.0f, rate);
                    if (nowTime >= timer)
                    {
                        nowTime = 0;
                        EnemyList[pageNumber].SetActive(false);
                        pageNumber++;
                    }
                }
            }
            else //페이지를 다 봤을때
            {
                anim.SetTrigger("End");
                start = false;
                TimerImage.SetActive(false);
                UIname.SetActive(false);
                PD.Play();
                Destroy(gameObject, 1.0f);

            }
        }
    }
    IEnumerator StartPage()
    {
        yield return new WaitForSeconds(1.0f);
        start = true;
        TimerImage.SetActive(true); 
        UIname.SetActive(true);
    }
}
