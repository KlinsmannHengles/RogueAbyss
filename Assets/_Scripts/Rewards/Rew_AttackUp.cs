using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_AttackUp : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        // Change Attack Power
        GameManager.Instance.playerScript.ChangeAttackPower(RewardManager.Instance.AttackPower);

        RewardManager.Instance.rewardSelected = true;
    }
}
