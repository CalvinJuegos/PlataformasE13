using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossUGTHealth : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float _maxHealth = 1000;
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private float _health = 1000;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            // Health < 0 = Dead
            if (_health < 0)
            {
                Debug.Log("Player is DEAD");
                IsAlive = false;

                // Call handleDeath();
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(animatorStrings.isAlive, value);
            Debug.Log("IsAlive set" + value);
        }
    }

    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;
    private float invencibilityTime = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    
}
