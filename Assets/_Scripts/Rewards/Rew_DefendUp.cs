using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_DefendUp : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        GameManager.Instance.playerScript.ChangeDefendCost(RewardManager.Instance.DefendCost);

        RewardManager.Instance.rewardSelected = true;
    }
}
