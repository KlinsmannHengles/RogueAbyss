using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rew_MaxHealthUp : Reward
{
    public override void RewardSelected()
    {
        if (RewardManager.Instance.rewardSelected)
            return;

        GameManager.Instance.playerScript.ChangeMaxHealth(RewardManager.Instance.maxHealth);
        /*BattleUIManager.Instance.playerHealthBar.SetMaxHealth(GameManager.Instance.playerScript.maxHealth);
        BattleUIManager.Instance.playerHealthBar.Heal(RewardManager.Instance.maxHealth);*/

        // Need to change the bar?

        RewardManager.Instance.rewardSelected = true;
    }
}
