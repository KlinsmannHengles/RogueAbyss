using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player playerScript;

    public GameObject endScreenObject;
    public CanvasGroup endScreenCanvasGroup;

    public SpriteRenderer actualBackground;
    public SpriteRenderer firstBackground;
    public SpriteRenderer secondBackground;

    public GameObject restartButtonObject;
    public CanvasGroup restartButtonCanvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        actualBackground = firstBackground;
        BattleManager.Instance.StartBattle();
    }

    public void Restart()
    {
        // Make the LoseUI disappear
        BattleUIManager.Instance.DefeatUIDisappearing();

        // If it is the first Enemy
        if (BattleManager.Instance.actualEnemyBehaviour != BattleManager.Instance.enemiesFather[0].GetComponent<EnemyBehaviour>())
        {
            // Make the enemy who killed you disappear
            BattleManager.Instance.actualEnemyBehaviour.EnemyDisappear();
        }     

        // Make the UI appear again // Now I do this calling StartNewBattleUI
        //BattleUIManager.Instance.UiAppearing();

        // Reset BattleManager Level
        BattleManager.Instance.gameLevel = GameLevel.FIRST_LEVEL;

        // ===== RESET PLAYER THINGS =====

        // Reset Player State
        playerScript.playerState = PlayerState.ALIVE;

        // Reset Player Health
        playerScript.maxHealth = playerScript.initialMaxHealth;
        playerScript.currentHealth = playerScript.initialMaxHealth;

        // Reset Player Health Bar
        BattleUIManager.Instance.playerHealthBar.SetMaxHealth(playerScript.initialMaxHealth);
        BattleUIManager.Instance.playerHealthBar.SetHealth(playerScript.initialMaxHealth);

        // Reset Player Energy
        BattleUIManager.Instance.barBehaviour.growSpeed = BattleUIManager.Instance.barBehaviour.initialgrowSpeed;

        // Reset Player Energy Bar
        BattleUIManager.Instance.barBehaviour.SetValue(0);

        // Reset Initial Costs
        GameManager.Instance.playerScript.initialDefenseCost = GameManager.Instance.playerScript.initialDefenseCost;
        GameManager.Instance.playerScript.initialAttackCost = GameManager.Instance.playerScript.initialAttackCost;

        // Reset Attack Power
        GameManager.Instance.playerScript.attackPower = GameManager.Instance.playerScript.initialAttackPower;

        // ===== RESET ENEMY THINGS =====

        // Reset Last Enemy Health
        //BattleManager.Instance.actualEnemyBehaviour.currentHealth = BattleManager.Instance.actualEnemyBehaviour.initialMaxHealth;

        // Reset All Enemies State and Health
        foreach (GameObject enemy in BattleManager.Instance.enemiesFather)
        {
            enemy.GetComponent<EnemyBehaviour>().enemyState = EnemyState.ALIVE;
            enemy.GetComponent<EnemyBehaviour>().currentHealth = enemy.GetComponent<EnemyBehaviour>().maxHealth;
        }

        // Turn the first enemy the actual enemy
        BattleManager.Instance.actualEnemyBehaviour = BattleManager.Instance.enemiesFather[0].GetComponent<EnemyBehaviour>();
        BattleManager.Instance.actualEnemyFatherTransform = BattleManager.Instance.enemiesFather[0].GetComponent<Transform>();
        BattleUIManager.Instance.enemySprite = BattleManager.Instance.enemiesFather[0].GetComponentInChildren<SpriteRenderer>();

        // Reset Enemy Health Bar
        BattleUIManager.Instance.enemyHealthBar.SetMaxHealth(BattleManager.Instance.actualEnemyBehaviour.maxHealth);
        BattleUIManager.Instance.enemyHealthBar.SetHealth(BattleManager.Instance.actualEnemyBehaviour.maxHealth);

        // Call the UI
        BattleUIManager.Instance.StartNewBattleUI();
    }

    public void ActiveEndScreen()
    {
        endScreenObject.SetActive(true);
        endScreenCanvasGroup.DOFade(1f, 11f);
        AudioManager.Instance.Play("GrandFinale");
        ChangeBackground();
        StartCoroutine(ActiveNewBeginningButton());
    }

    public void DisableEndScreen()
    {
        endScreenCanvasGroup.DOFade(0f, 2f);
        StartCoroutine(DisableEndScreenSupport());
    }

    public IEnumerator DisableEndScreenSupport()
    {
        yield return new WaitForSeconds(2f);
        endScreenObject.SetActive(false);
    }

    public IEnumerator ActiveNewBeginningButton()
    {
        yield return new WaitForSeconds(13f);
        restartButtonObject.SetActive(true);
        restartButtonCanvasGroup.DOFade(1f, 2f);
    }

    public void ChangeBackground()
    {
        if (actualBackground == firstBackground)
        {
            secondBackground.gameObject.SetActive(true);
            firstBackground.DOFade(0f, 11f);
            secondBackground.DOFade(1f, 11f);
            actualBackground = secondBackground;
            StartCoroutine(ChangeBackgroundSupport(firstBackground));
        } else if (actualBackground == secondBackground)
        {
            firstBackground.gameObject.SetActive(true);
            firstBackground.DOFade(1f, 11f);
            secondBackground.DOFade(0f, 11f);
            actualBackground = firstBackground;
            StartCoroutine(ChangeBackgroundSupport(secondBackground));
        } else
        {
            Debug.Log("Background não compatível para mudança em ChangeBackground");
        }
    }

    public IEnumerator ChangeBackgroundSupport(SpriteRenderer _background)
    {
        yield return new WaitForSeconds(11f);
        _background.gameObject.SetActive(false);
    } 

}
