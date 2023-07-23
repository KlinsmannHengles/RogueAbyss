using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum PlayerState { ALIVE, DEAD}
public class Player : MonoBehaviour
{
    public static event Action<int> attack;
    public static event Action<bool> defend;
    public static event Action<EnemyState> die;

    [Header("Initial Stats")]
    public int initialMaxHealth = 10;
    public int initialDefenseCost;
    public int initialAttackCost;
    public int initialAttackPower;

    public int maxHealth;
    public int currentHealth;

    public PlayerState playerState = PlayerState.ALIVE;

    [SerializeField] private int attackCost;
    [SerializeField] private int defendCost;

    public int attackPower;

    [SerializeField] private bool isDefending;

    [SerializeField] private BarBehaviour barBehaviour;
    [SerializeField] private BarDefendBehaviour barDefendBehaviour;

    private void Start()
    {
        attack += TakePlayerActionPoints;
        defend += IsDefending;
    }

    public void Attack(int damage)
    {
        if (attackCost > barBehaviour.slider.value)
        {
            barBehaviour.NoActionPoints();
            return;
        }

        if (attack != null)
            attack(attackCost);

        BattleUIManager.Instance.FastMovingPlayerActionBar();

        BattleManager.Instance.actualEnemyBehaviour.TakeDamage(attackPower);

        AudioManager.Instance.Play("Punch");
    }

    public void Defend(bool _isDefending)
    {
        if (defendCost > barBehaviour.slider.value)
        {
            barBehaviour.NoActionPoints();
            return;
        }

        if (defend != null)
            defend(_isDefending);

        BattleUIManager.Instance.FastMovingPlayerActionBar();

        // Make something here to protect
    }

    public void TakePlayerActionPoints(int amount)
    {
        barBehaviour.TakeValue(amount);
    }

    public void IsDefending(bool _isDefending)
    {
        isDefending = _isDefending;
        barDefendBehaviour.SetValue();

        BattleUIManager.Instance.playerHealthBarBorder.DOColor(BattleUIManager.Instance.defendColor, 0.2f);

        // CHANGE THIS TO A DELEGATE ACTION
        TakePlayerActionPoints(defendCost);
    }

    public void IsDefendingToFalse()
    {
        isDefending = false;

        BattleUIManager.Instance.playerHealthBarBorder.DOColor(Color.white, 0.2f);
    }

    public void TakeDamage(int _dmgToTake)
    {
        if (playerState == PlayerState.DEAD)
            return;

        if (isDefending)
        {
            AudioManager.Instance.Play("DefendedAttack");
            return;
        }           

        currentHealth -= _dmgToTake;

        BattleUIManager.Instance.playerHealthBar.SetHealth(currentHealth);

        BattleUIManager.Instance.ShakePlayerHealthBar();

        AudioManager.Instance.Play("PlayerDamaged");

        if (currentHealth <= 0)
        {
            Debug.Log("CALLING PLAYER DIE");
            playerState = PlayerState.DEAD;
            Die();
        }
    }

    public void Die()
    {
        if (die != null)
            die(BattleManager.Instance.actualEnemyBehaviour.enemyState);

        BattleManager.Instance.EndBattleLose();
    }

    public void ChangeMaxHealth(int _newMaxHealth)
    {
        maxHealth += _newMaxHealth;
    }

    public void ChangeAttackCost(int _newAttackCost)
    {
        attackCost -= _newAttackCost;
    }

    public void ChangeDefendCost(int _newDefendCost)
    {
        defendCost -= _newDefendCost;
    }

    public void ChangeAttackPower(int _newAttackPower)
    {
        attackPower += _newAttackPower;
    }

    public void HealFullHealth()
    {
        currentHealth = maxHealth;
    }
}
