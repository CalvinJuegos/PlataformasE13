using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthDisplay : MonoBehaviour
{
    public Slider slider;
    


    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetBossHealth(float health)
    {
        slider.value = health;
        Debug.Log("SLIDER SLIDER"+slider.value);
    }

    // Threshhold for critical effects
    //public void CriticalHealth()
}