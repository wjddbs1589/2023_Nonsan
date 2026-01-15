using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UIElements;
using Cinemachine;

public class HelicopterController : MonoBehaviour
{
    public GameObject target;
    public GameObject bulletPrefab;
    [SerializeField]Transform[] muzzles = new Transform[4];
    bool reloading = false;

    float duration = 1.0f;
    float durationOrigin = 1.0f;

    private void Update()
    {
        transform.DOLookAt(target.transform.position, 0.5f);

        if(!reloading)
        {
            if (duration >= 0)
            {
                duration -= Time.deltaTime;
                foreach (Transform t in muzzles)
                {
                    Instantiate(bulletPrefab, t.transform);
                }
            }
            else
            {
                duration = durationOrigin;
                StartCoroutine(Reload());
            }
        }
    }
    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(1f);
        reloading = false;
    }
}
