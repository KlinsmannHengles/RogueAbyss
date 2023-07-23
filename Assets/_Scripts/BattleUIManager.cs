using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }

    [Header("UI Buttons")]
    [SerializeField] private RectTransform attackButtonTransform; 
    [SerializeField] private RectTransform attackButtonFatherTransform; 
    [SerializeField] private RectTransform defendButtonTransform;
    [SerializeField] private RectTransform defendButtonFatherTransform;

    private bool isShaking = false;
    private Vector3 attackButtonInitialPosition;
    private Vector3 defendButtonInitialPosition;
    private float uncertainty = 0.1f;

    [Header("Shaking")]
    [SerializeField] private float shakeStrength;
    [SerializeField] private float shakeDuration;

    [Header("Enemy")]
    public SpriteRenderer enemySprite;
    public EnemyHealthBar enemyHealthBar;

    [Header("Player")]
    public PlayerHealthBar playerHealthBar;  
    public BarBehaviour barBehaviour; // Player Action Bar

    public Image playerHealthBarBorder;
    public Color defendColor;

    public Image playerEnergyBarBorder;

    [Header("VictoryAndLoseScreens")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject loseScreen;

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
        attackButtonInitialPosition = attackButtonTransform.anchoredPosition;
        defendButtonInitialPosition = defendButtonTransform.anchoredPosition;

        // Set Player HealthBar
        playerHealthBar.SetMaxHealth(GameManager.Instance.playerScript.maxHealth);

        // Set Enemy HealthBar
        enemyHealthBar.SetMaxHealth(BattleManager.Instance.actualEnemyBehaviour.maxHealth);

        UiAppearing();

        // Signal to Shake the Enemy
        EnemyBehaviour.enemy += ShakeEnemy;

        Player.attack += JuiceUI;
        Player.defend += DefendUI;
        Player.die += UiDisappearing;
        Player.die += DefeatUIAppearing;

        EnemyBehaviour.die += UiDisappearing;
        EnemyBehaviour.die += VictoryUIAppearing;
    }

    /*public void DestroyThyself()
    {
        Destroy(gameObject);
        Instance = null;
    }*/

    public void ShakeEnemy(EnemyBehaviour _enemy)
    {
        BattleManager.Instance.actualEnemyFatherTransform.DOShakePosition(shakeDuration, shakeStrength);
    }

    public void DefendUI(bool Defend)
    {
        
    }

    public void ShakePlayerHealthBar()
    {
        playerHealthBar.GetComponent<Transform>().DOShakePosition(1f, 50f);
    }

    public void ShakeEnemyHealthBar()
    {
        enemyHealthBar.GetComponent<Transform>().DOShakePosition(1f, 5f);
    }

    // for when you use action points
    public void FastMovingPlayerActionBar()
    {
        barBehaviour.gameObject.GetComponent<RectTransform>().DOAnchorPosX(190f, 0.2f).onComplete = PlayerActionBarBackToPosition;
    }

    public void PlayerActionBarBackToPosition()
    {
        barBehaviour.gameObject.GetComponent<RectTransform>().DOAnchorPosX(180f, 0.2f);
    }

    public void UsePlayerActionBar()
    {
        playerEnergyBarBorder.DOColor(Color.red, 0.1f).onComplete = UsePlayerActionBarSupport;
    }

    public void UsePlayerActionBarSupport()
    {
        playerEnergyBarBorder.DOColor(Color.white, 0.1f);
    }

    public void JuiceUI(int x)
    {
        if (isShaking /*|| Vector2.Distance(attackButtonInitialPosition, attackButtonTransform.anchoredPosition) > 50f*/)
        {
            return;
        }

        attackButtonTransform.DOShakeAnchorPos(1f, 5f);

        enemySprite.DOColor(Color.red, 0.3f).onComplete = BackToNormalColor;

    }  

    // Make the enemy color back to its normal color
    public void BackToNormalColor()
    {
        enemySprite.DOColor(Color.white, 0.3f);
    }

    // Make the buttons appear from outside the screen to inside the screen
    public void UiAppearing()
    {
        attackButtonFatherTransform.DOAnchorPosY(30f, 2f).SetEase(Ease.OutCubic);
        defendButtonFatherTransform.DOAnchorPosY(30f, 2f).SetEase(Ease.OutCubic);

        barBehaviour.gameObject.GetComponent<RectTransform>().DOAnchorPosX(180f, 2f).SetEase(Ease.OutCubic);
        playerHealthBar.gameObject.GetComponent<RectTransform>().DOAnchorPosX(180f, 2f).SetEase(Ease.OutCubic);

        enemyHealthBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-100f, 2f).SetEase(Ease.OutCubic);
    }

    public void UiDisappearing(EnemyState _enemyState)
    {
        attackButtonFatherTransform.DOAnchorPosY(-100f, 2f).SetEase(Ease.InCubic);
        defendButtonFatherTransform.DOAnchorPosY(-100f, 2f).SetEase(Ease.InCubic);

        barBehaviour.gameObject.GetComponent<RectTransform>().DOAnchorPosX(-180f, 2f).SetEase(Ease.InCubic);
        playerHealthBar.gameObject.GetComponent<RectTransform>().DOAnchorPosX(-180f, 2f).SetEase(Ease.InCubic);

        enemyHealthBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(100f, 2f).SetEase(Ease.InCubic);
    }

    public void VictoryUIAppearing(EnemyState _enemyState)
    {
        victoryScreen.SetActive(true);
        StartCoroutine(VictoryUIAppearingSupport());
    }

    public IEnumerator VictoryUIAppearingSupport()
    {
        yield return new WaitForSeconds(2f);
        victoryScreen.GetComponent<CanvasGroup>().DOFade(1f, 2f);
    }

    public void DefeatUIAppearing(EnemyState enemyState)
    {
        loseScreen.SetActive(true);
        StartCoroutine(DefeatUIAppearingSupport());
    }

    public IEnumerator DefeatUIAppearingSupport()
    {
        yield return new WaitForSeconds(2f);
        loseScreen.GetComponent<CanvasGroup>().DOFade(1f, 2f);
    }

    public void VictoryUIDisappearing()
    {
        victoryScreen.GetComponent<CanvasGroup>().DOFade(0f, 2f);
        StartCoroutine(VictoryUIDisappearingSupport());
    }

    public IEnumerator VictoryUIDisappearingSupport()
    {
        yield return new WaitForSeconds(2f);
        victoryScreen.SetActive(false);
    }

    public void DefeatUIDisappearing()
    {
        loseScreen.GetComponent<CanvasGroup>().DOFade(0f, 2f);
        StartCoroutine(DefeatUIDisappearingSupport());
    }

    public IEnumerator DefeatUIDisappearingSupport()
    {
        yield return new WaitForSeconds(2f);
        loseScreen.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Make the attackButtonTransform go back to its position in a juicy way
        attackButtonTransform.anchoredPosition = Vector3.Lerp(attackButtonTransform.anchoredPosition, attackButtonInitialPosition, 10f * Time.deltaTime);

        // This is done to the juicy effect of backing to position don't be forever played because it never reach a int number
        if (attackButtonTransform.anchoredPosition.x > attackButtonInitialPosition.x - uncertainty && attackButtonTransform.anchoredPosition.x < attackButtonInitialPosition.x + uncertainty)
        {
            if (attackButtonTransform.anchoredPosition.y > attackButtonInitialPosition.y - uncertainty && attackButtonTransform.anchoredPosition.y < attackButtonInitialPosition.y + uncertainty)
            {
                attackButtonTransform.anchoredPosition = new Vector3(attackButtonInitialPosition.x, attackButtonInitialPosition.y, attackButtonInitialPosition.z);
            }
        }
    }

    public void RewardSelected()
    {
        // Make animations here
    }

    public void StartNewBattleUI()
    {
        // Make UI Appear
        UiAppearing();

        // Make Enemy Appear
        BattleManager.Instance.actualEnemyBehaviour.EnemyAppear();

        // Set Enemy Health Bar
        enemyHealthBar.SetMaxHealth(BattleManager.Instance.actualEnemyBehaviour.maxHealth);
        enemyHealthBar.SetHealth(BattleManager.Instance.actualEnemyBehaviour.maxHealth);

    }



}
