using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Health : HealthManager
{
    public float enemyHealth = 100f;
    public GameObject hitEffect;
    private Animator animator;
    private NavMeshAgent agent;
    EnemyController controller;

    public AudioClip hitSound;
    public AudioClip deadSound;

    int score = 100;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
    }

    public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null, int playerNum = 0)
    {
        // Is the NPC alive?
        if (!dead)
        {
            // Trigger hit animation.
            // if (!anim.IsInTransition(3) && anim.GetCurrentAnimatorStateInfo(3).IsName("No hit"))
            animator.SetTrigger("Hit");
            animator.SetInteger("HitIndex", RandomInt(0, 4));
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, .5f);
            if (hitEffect != null)
                Object.Instantiate<GameObject>(hitEffect, location, Quaternion.LookRotation(-direction), this.transform);
            // Take damage received from current health.
            enemyHealth -= damage;

        }
        // Time to die.
        if (enemyHealth <= 0)
        {
            animator.SetBool("Death", true);
            animator.SetBool("Shoot", false);
            animator.SetInteger("DeathIndex", RandomInt(0, 4));

            if (!dead)
            {
                GameManager.Inst.UImanager.ScoreTextChage(playerNum, score, 1);
                Kill();
            }
                
        }
    }
    public void Kill()
    {
        AudioSource.PlayClipAtPoint(deadSound, Camera.main.transform.position, .5f);
        dead = true;
        controller.dead = true;
        Destroy(this.GetComponent<NavMeshAgent>());

        StartCoroutine(RemoveAllForces());
        DeathAnimPlay();
    }

    public void RoadKill()
    {
        AudioSource.PlayClipAtPoint(deadSound, Camera.main.transform.position, 1);
        dead = true;
        controller.dead = true;
        Destroy(this.GetComponent<NavMeshAgent>());

        StartCoroutine(RemoveAllForces());
        DeathAnimPlay();
    }

    void DeathAnimPlay()
    {
        int num = RandomInt(0, 4);
        switch (num)
        {
            case 0:
                animator.Play("Death0");
                break;
            case 1:
                animator.Play("Death1");
                break;
            case 2:
                animator.Play("Death2");
                break;                
            case 3:
                animator.Play("Death3");
                break;
            default:
                break;
        }        
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
    private int RandomInt(int a, int b)
    {
        int num = Random.Range(a, b);
        return num;
    }
}
