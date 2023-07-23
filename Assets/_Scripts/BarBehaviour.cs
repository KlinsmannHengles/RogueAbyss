using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarBehaviour : MonoBehaviour
{
    public Slider slider;

    public float initialgrowSpeed;
    public float growSpeed; // speed of energy increase: 0.2 is recomended

    public void SetValue(int value)
    {
        slider.value = value;
    }

    public void FixedUpdate()
    {
        slider.value += growSpeed;     
    }

    // diminui o valor da barra
    public void TakeValue(int amount)
    {
        slider.value -= amount;
    }

    public void NoActionPoints()
    {
        BattleUIManager.Instance.UsePlayerActionBar();
    }

    public void ChangeGrowSpeed(float _grow)
    {
        growSpeed += _grow;
    }

}
