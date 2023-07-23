using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;
using DG.Tweening;

public enum EnemyState { ALIVE, DEAD}
public class EnemyBehaviour : MonoBehaviour
{
    public static event Action<EnemyBehaviour> enemy;
    public static event Action<EnemyState> die;

    [Header("Initial Stats")]
    public int initialMaxHealth;

    public GameObject enemyObject;
    public int maxHealth;
    public int currentHealth;
    [SerializeField] private bool isAttacking = false; // I can take out the SerializeField
    [SerializeField] private Animator animator;
    [SerializeField] private int attackPower;
    public EnemyState enemyState;

    // Random Attack
    public float firingRate;
    public float rateVariability;

    public int Health { get { return currentHealth; } }

    private void Start()
    {
        RandomizeAttackRate();
        enemyState = EnemyState.ALIVE;
    }

    public void TakeDamage(int damage)
    {
        if (enemy != null)
            enemy(this);

        currentHealth -= damage;

        BattleUIManager.Instance.enemyHealthBar.SetHealth(currentHealth);

        BattleUIManager.Instance.ShakeEnemyHealthBar();

        AudioManager.Instance.Play("EnemyDamaged");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (die != null)
            die(enemyState);

        enemyState = EnemyState.DEAD;

        enemyObject.GetComponent<SpriteRenderer>().DOFade(0f, 4f);
        StartCoroutine(DieSupport());
        BattleManager.Instance.EndBattleWin();
    }

    public IEnumerator DieSupport()
    {
        yield return new WaitForSeconds(4f);

        this.gameObject.SetActive(false);
    }

    public void EnemyAppear()
    {
        this.gameObject.SetActive(true);
        enemyObject.GetComponent<SpriteRenderer>().DOFade(255f, 4f);
    }

    public void EnemyDisappear()
    {
        enemyObject.GetComponent<SpriteRenderer>().DOFade(0f, 0.2f);
        StartCoroutine(EnemyDisappearSupport());
    }

    private IEnumerator EnemyDisappearSupport()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
    }

    public void Attack()
    {
        if (enemyState == EnemyState.DEAD)
            return;

        animator.SetBool("isAttacking", true);
        isAttacking = true;
    }

    public void SetIsAttackingToFalse()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false; 
    }

    public void RandomizeAttackRate()
    {
        float nextAttack = UnityEngine.Random.Range(firingRate, firingRate * rateVariability);

        Invoke("Attack", nextAttack);

        Invoke("RandomizeAttackRate", nextAttack);

    }

    // Chamada por um AnimationEvent
    public void DealDamageOnPlayer()
    {
        // O jogador leva dano no ponto máximo da animação
        GameManager.Instance.playerScript.TakeDamage(attackPower);
    }

}
