using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{    
    public AudioClip crashSounds;

    private static SoundManager instance; // 사운드 매니저의 인스턴스를 저장할 변수
    // 사운드 매니저의 인스턴스 가져오기
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    // 사운드 매니저 오브젝트가 씬에 없는 경우 생성
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    instance = soundManagerObject.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
}
