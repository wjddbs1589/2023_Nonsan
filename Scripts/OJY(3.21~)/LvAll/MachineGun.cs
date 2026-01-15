using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;

//중간보스 머신건 적 총 쏘는 애니메이션
public class MachineGun : MonoBehaviour
{
    public GameObject Target;
    public GameObject flash;
    public GameObject tracer;
    public GameObject gun;
    public float interval;
    public GameObject audioSource;

    //Transform chest;
    Animator anim;
    //public Vector3 offset;

    float attackDuration = 0.0f;      
    float totalAttackDuration = 2.0f; 
    bool reloading = false;
    bool dead = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        //chest = anim.GetBoneTransform(HumanBodyBones.Chest);
        StartCoroutine(Flash(interval));
    }

    private void LateUpdate()
    {
        if (!GameManager.Inst.gameEnd)
        {
            transform.LookAt(Target.transform);
            //chest.LookAt(Target.transform);
            //chest.rotation = chest.rotation * Quaternion.Euler(offset);

            if (!reloading)
            {
                attackDuration -= Time.deltaTime;
                if (attackDuration <= 0)
                {
                    reloading = true;
                }
            }
            else
            {
                attackDuration += Time.deltaTime;
                if (attackDuration >= 1)
                {
                    reloading = false;
                    attackDuration = totalAttackDuration;
                }
            }
        }
    }

    IEnumerator Flash(float _interval)
    {
        while (true)
        {
            if (!dead && !reloading)
            {
                flash.SetActive(true);
                GameObject soundObj = Instantiate(audioSource);
                soundObj.transform.position = transform.position;
                GameObject tracerObj = Instantiate(tracer, flash.transform);
                tracerObj.transform.position = flash.transform.position;
                yield return new WaitForSeconds(_interval);
                Destroy(tracerObj.gameObject, 5.0f);
                flash.SetActive(false);
            }
            yield return new WaitForSeconds(_interval);
        }        
    }

    public void PlayDeadAnimation()
    {
        dead = true;
        GameManager.Inst.TotalScore += 100;
        GameManager.Inst.TotalkillCount++;
        StartCoroutine(RemoveAllForces());
        Destroy(gun);
        anim.SetBool("Dead", true);
    }
    IEnumerator RemoveAllForces()
    {
        foreach (Rigidbody member in GetComponentsInChildren<Rigidbody>())
        {
            member.isKinematic = false;
            member.velocity = Vector3.zero;
        }
        foreach (Collider member in GetComponentsInChildren<Collider>())
        {
            member.enabled = false;
        }
        yield return new WaitForSeconds(3);
        foreach (MonoBehaviour mb in this.GetComponents<MonoBehaviour>())
        {
            if (this != mb)
            {
                Destroy(mb);
            }
        }
    }
}
