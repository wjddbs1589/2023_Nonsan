using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    Animator anim;
    //public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetBool("Rotation", true);
        //transform.DORotate(new Vector3(0, 1*Time.deltaTime, 0), 1f);
        //transform.DOMove(new Vector3(0, 1, 0), 1f);
    }

    
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, 1*Time.deltaTime, 0);
    }
}
