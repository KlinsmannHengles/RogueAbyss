using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_AttackCostUp : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        GameManager.Instance.playerScript.ChangeAttackCost(RewardManager.Instance.AttackCost);

        RewardManager.Instance.rewardSelected = true;
    }
}
