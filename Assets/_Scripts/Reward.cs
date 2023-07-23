using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward: MonoBehaviour
{
    public string rewardName;
    public string description;

    public virtual void RewardSelected()
    {
        
    }
}
