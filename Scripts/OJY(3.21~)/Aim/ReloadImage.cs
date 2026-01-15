using System.Collections;
using UnityEngine;

public class ReloadImage : MonoBehaviour
{
    int siblingIndex = 0;
    bool animated = false;
    Animator anim;
    Transform parent;
    int haveBullet = 0;
    private void Awake()
    {
        parent = transform.parent;
        if (parent != null)
        {
            siblingIndex = transform.GetSiblingIndex();
        }

        anim = GetComponent<Animator>();

    }

    public void OnEmptyBullet()
    {
        StartCoroutine(EmptyBullet());
    }

    IEnumerator EmptyBullet()
    {
        anim.SetBool("Reload", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("Reload", false);
    }

    

}

