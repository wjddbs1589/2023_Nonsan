using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    PlayerSetting playerSetting;
    public bool UseCutScene = false;  //라운드 사이 컷신에 사용되는지 여부
    public GameObject RankingBoard;

    private void Awake()
    {
        playerSetting = GetComponentInParent<PlayerSetting>();
    }
    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
        if (UseCutScene)
        {
            StartCoroutine(startRanking());
        }
    }

    IEnumerator startRanking()
    {
        yield return new WaitForSeconds(0.5f);
        RankingBoard.SetActive(true);
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        if (UseCutScene)
        {
            playerSetting.SetActiveLoadingImage();
        }
        transform.parent.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (UseCutScene)
        {
            RankingBoard.SetActive(false);
        }
    }

}
