using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LookAtBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    private void Awake()
    {
        if (bullet == null)
        {
            bullet = GameObject.Find("sniperBullet").gameObject;
        }
    }
    void Update()
    {
        transform.DOLookAt(bullet.transform.position, 0.1f);
    }
}
