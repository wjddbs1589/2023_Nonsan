using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;


public class LookatTarget : MonoBehaviour
{
    //Å¸°Ù Lookat
    public Vector3 offset;
    Transform chest;

    StateController controller;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        controller = GetComponent<StateController>();

    }
    private void LateUpdate()
    {
        chest.LookAt(controller.aimTarget);
        chest.rotation = chest.rotation * Quaternion.Euler(offset);
    }
}
