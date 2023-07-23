using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarDefendBehaviour : MonoBehaviour
{
    public Slider slider;

    public void SetValue()
    {
        slider.value = slider.maxValue;
    }

    public void FixedUpdate()
    {
        slider.value -= 1f;
    }

    public void TakeValue(int amount)
    {
        slider.value -= amount;
    }

    public void NotDefending()
    {
        if (slider.value <= 0)
        {
            GameManager.Instance.playerScript.IsDefendingToFalse();
        }
    }
}
