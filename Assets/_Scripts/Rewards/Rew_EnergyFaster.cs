using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_EnergyFaster : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        BattleUIManager.Instance.barBehaviour.ChangeGrowSpeed(RewardManager.Instance.energySpeed);

        RewardManager.Instance.rewardSelected = true;
    }
}
