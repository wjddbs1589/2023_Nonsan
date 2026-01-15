using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundSoundPlayer : MonoBehaviour
{
    public AudioClip Sound1;
    public AudioClip Sound2;
    public AudioClip Sound3;
    public AudioClip Sound4;
    public AudioClip Sound5;
    public AudioClip Sound6;
    public AudioClip Sound7;
    public AudioClip Sound8;

    float cooltime = 5;
    private void Update()
    {
        cooltime -= Time.deltaTime;
        if(cooltime <= 0)
        {
            explosionSound();
            cooltime = Random.Range(5, 8);
        }
    }

    void explosionSound()
    {
        int num = Random.Range(0, 8);
        switch (num)
        {
            case 0:
                AudioSource.PlayClipAtPoint(Sound1, transform.up * 5);
                break;
            case 1:
                AudioSource.PlayClipAtPoint(Sound2, transform.up * 5);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(Sound3, transform.up * 5);
                break;
            case 3:
                AudioSource.PlayClipAtPoint(Sound4, transform.up * 5);
                break;
            case 4:
                AudioSource.PlayClipAtPoint(Sound5, transform.up * 5);
                break;
            case 5:
                AudioSource.PlayClipAtPoint(Sound6, transform.up * 5);
                break;
            case 6:
                AudioSource.PlayClipAtPoint(Sound6, transform.up * 5);
                break;
            case 7:
                AudioSource.PlayClipAtPoint(Sound6, transform.up * 5);
                break;
            default:
                break;
        }
    }
}
