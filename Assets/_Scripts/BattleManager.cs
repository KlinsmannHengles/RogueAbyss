using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameLevel { FIRST_LEVEL, SECOND_LEVEL, THIRD_LEVEL, BOSS_LEVEL}
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Essencial")]
    public GameLevel gameLevel = GameLevel.FIRST_LEVEL;

    [Header("Enemy")]
    public EnemyBehaviour actualEnemyBehaviour;
    public Transform actualEnemyFatherTransform;

    public GameObject[] enemiesFather;

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

    public void StartBattle()
    {
        AudioManager.Instance.Play("Battle01");
    }

    // Hits when you click "Continue" after selecting a reward
    public void StartNewBattle()
    {
        // Change Game Level to the next (It change the enemy too)
        NextGameLevel();

        // Set EnemyStatus


        // Sort New Background

        // Active Battle Song
        BackBattleSong();


        BattleUIManager.Instance.StartNewBattleUI();
    }

    public void EndBattleWin()
    {
        AudioManager.Instance.sounds[0].source.DOFade(0f, 4f);
        StartCoroutine(PlayVictorySound());
    }

    public void EndBattleLose()
    {
        AudioManager.Instance.sounds[0].source.DOFade(0f, 4f);
        StartCoroutine(PlayDefeatSound());
    }

    public IEnumerator PlayVictorySound()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.Play("Victory");
    }

    public IEnumerator PlayDefeatSound()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.Play("Defeat");
    }

    public void BackBattleSong()
    {
        AudioManager.Instance.sounds[0].source.DOFade(1f, 2f);
    }

    public void NextGameLevel()
    {
        switch (gameLevel)
        {
            case GameLevel.FIRST_LEVEL:
                gameLevel = GameLevel.SECOND_LEVEL;
                actualEnemyBehaviour = enemiesFather[1].GetComponent<EnemyBehaviour>();
                actualEnemyFatherTransform = enemiesFather[1].GetComponent<Transform>();
                BattleUIManager.Instance.enemySprite = enemiesFather[1].GetComponentInChildren<SpriteRenderer>();
                break;
            case GameLevel.SECOND_LEVEL:
                gameLevel = GameLevel.THIRD_LEVEL;
                actualEnemyBehaviour = enemiesFather[2].GetComponent<EnemyBehaviour>();
                actualEnemyFatherTransform = enemiesFather[2].GetComponent<Transform>();
                BattleUIManager.Instance.enemySprite = enemiesFather[2].GetComponentInChildren<SpriteRenderer>();
                break;
            case GameLevel.THIRD_LEVEL:
                gameLevel = GameLevel.BOSS_LEVEL;
                actualEnemyBehaviour = enemiesFather[3].GetComponent<EnemyBehaviour>();
                actualEnemyFatherTransform = enemiesFather[3].GetComponent<Transform>();
                BattleUIManager.Instance.enemySprite = enemiesFather[3].GetComponentInChildren<SpriteRenderer>();
                break;
            case GameLevel.BOSS_LEVEL:
                Debug.Log("IT WAS THE LAST LEVEL");
                break;
        }
    }
}
