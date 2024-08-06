using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    Animator animator;

    public HealthDisplay healthBar;

    [SerializeField]
    private float _maxHealth = 100;
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
            //healthBar.SetMaxHealth(value);
        }
    }

    [SerializeField]
    private float _health = 100;
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
    public bool isInvincible = false;
    private float timeSinceHit = 0;
    private float invencibilityTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthBar = FindObjectOfType<HealthDisplay>();
        healthBar.SetMaxHealth(MaxHealth);

    }

    public void Hit(float damage)
    {
        if(IsAlive && !isInvincible)
        {
            Debug.Log("Hit for:" + damage);
            Health -= damage;
            isInvincible = true;
            healthBar.SetHealth(Health);
        }
    }

    public void handleDeath()
    {
        // Show you die screen in ANIMATOR

        // Load the first scene in HUB and Lower max Health 

    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invencibilityTime)
            {
                // Quitar invencibilidad
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
}
