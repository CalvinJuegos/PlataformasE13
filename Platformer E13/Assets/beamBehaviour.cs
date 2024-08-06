using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamBehaviour : MonoBehaviour
{
    public Animator animator;
    public Collider2D collider;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
        collider = this.GetComponent<Collider2D>();
    }

    public void StartDealingDamage()
    {
        Debug.Log("DamageStarted");
        GetComponent<Collider2D>().enabled = true;
    }

    public void DestroyBeam()
    {
        Destroy(this.gameObject);
    }
}
