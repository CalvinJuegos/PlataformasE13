using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagable : MonoBehaviour
{
    Animator animator;
    // MISSING POISE RECOVERY OVER TIME and DEATH

    [SerializeField]
    private int _vitalPoints;
    public float VitalPoints
    {
        get
        {
            return _vitalPoints;
        }
        set
        {
            _vitalPoints = (int)value;

            // Vital Points <= 0 = Dead
            if (_vitalPoints <= 0)
            {
                IsAlive = false;
                // Call death function
            }
        }
    }


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
        }
    }
    [SerializeField]
    private float _maxPoise = 100;
    public float MaxPoise
    {
        get
        {
            return _maxPoise;
        }
        set
        {
            _maxPoise = value;
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

            // Health < 0 = Stun
            if (_health < 0)
            {
                handleStun();
            }
        }
    }

    [SerializeField]
    private float _poise = 100;
    public float Poise
    {
        get
        {
            return _poise;
        }
        set
        {
            _poise = value;

            // Poise < 0 = Stun
            if (_poise < 0)
            {
                handleStun();
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
    private bool _isStunned = true;
    public bool IsStunned
    {
        get
        {
            return _isStunned;
        }
        set
        {
            _isStunned = value;
            // Stun needs to be implemented as isAlive in animator variable
            // Set animator
            //animator.SetBool(animatorStrings.isAlive, value);
            Debug.Log("IsStunned set" + value);
        }
    }

    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;
    private float invencibilityTime = 0.25f;
    private bool hitWhileStun;
    public float stunDuration;
    public float percentRecovered = 0.15f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void enemyHit(float damage,float poiseDamage)
    {
        if(IsStunned && IsAlive && !isInvincible)
        {
            hitWhileStun = true;
           
        }else if (IsAlive && !isInvincible)
        {
            // Normal attack, deals health damage and poise damage
            Poise -= poiseDamage;
            Health -= damage;
            isInvincible = true;
        }
    }

    public void handleStun()
    {
        if (VitalPoints > 0)
        {
            // Restrict movement and animation stun
            //here
            stunned(stunDuration);
            IsStunned = true;
            Debug.Log("Is Stunned?"+IsStunned);
            
        }
    }

    public IEnumerator stunned(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Check the cancel condition
            if (hitWhileStun)
            {
                Debug.Log("Exiting Stun!");
                // -1 Vital Point, no longer stunned
                VitalPoints -= 1;
                isInvincible = true;
                IsStunned = false;
                // Reset health
                _health = MaxHealth;
                yield break; // Exit the coroutine
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        // Reset health out of time %
        // if not acted while stunned recover percentage of current health bar
        _health = MaxHealth*percentRecovered;
        Debug.Log("Health reset");
        yield return null;
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
