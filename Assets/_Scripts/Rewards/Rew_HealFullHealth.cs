using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_HealFullHealth : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        GameManager.Instance.playerScript.HealFullHealth();
        BattleUIManager.Instance.playerHealthBar.SetHealth(GameManager.Instance.playerScript.maxHealth);

        RewardManager.Instance.rewardSelected = true;
    }
}
