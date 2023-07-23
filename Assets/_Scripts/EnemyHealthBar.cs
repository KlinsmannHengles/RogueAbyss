using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int _maxHealth)
    {
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
    }

    public void SetHealth(int _health)
    {
        slider.value = _health;
    }

    // diminui o valor da barra
    public void TakeValue(int amount)
    {
        slider.value -= amount;
    }
}
