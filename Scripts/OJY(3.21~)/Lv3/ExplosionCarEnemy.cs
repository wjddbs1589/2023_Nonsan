using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCarEnemy : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    public void ExplosionEnemy()
    {
        transform.parent = null;
        anim = GetComponent<Animator>();
        anim.Play("Explosion");
    }
}
