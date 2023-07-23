using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance { get; private set; }

    public GameObject rewardScreen;

    public bool rewardSelected = false;

    [Header("RewardSlots")]
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private TextMeshProUGUI slot1Text;
    [SerializeField] private TextMeshProUGUI slot2Text;
    [SerializeField] private TextMeshProUGUI slot3Text;
    [SerializeField] private TextMeshProUGUI slot1Description;
    [SerializeField] private TextMeshProUGUI slot2Description;
    [SerializeField] private TextMeshProUGUI slot3Description;

    [Header("RewardValues")]
    public int maxHealth;
    public int AttackCost;
    public int DefendCost;
    public int AttackPower;
    public float energySpeed; // speed by witch it increases in FixedUpdate()

    public GameObject[] rewards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RewardUIAppearing()
    {
        // If you defeated Boss
        if (BattleManager.Instance.enemiesFather[3].GetComponent<EnemyBehaviour>().enemyState == EnemyState.DEAD)
        {
            GameManager.Instance.ActiveEndScreen();
            return;
        }

        ShuffleRewards();

        slot1Text.text = rewards[0].GetComponent<Reward>().rewardName;
        slot2Text.text = rewards[1].GetComponent<Reward>().rewardName;
        slot3Text.text = rewards[2].GetComponent<Reward>().rewardName;

        slot1Description.text = rewards[0].GetComponent<Reward>().description;
        slot2Description.text = rewards[1].GetComponent<Reward>().description;
        slot3Description.text = rewards[2].GetComponent<Reward>().description;

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();

        button1.onClick.AddListener(rewards[0].GetComponent<Reward>().RewardSelected);
        button2.onClick.AddListener(rewards[1].GetComponent<Reward>().RewardSelected);
        button3.onClick.AddListener(rewards[2].GetComponent<Reward>().RewardSelected);

        rewardSelected = false;

        rewardScreen.SetActive(true);
        rewardScreen.GetComponent<CanvasGroup>().DOFade(1f, 2f);

        AudioManager.Instance.Play("RewardScreen");
    }

    public void RewardUIDisappearing()
    {
        rewardScreen.GetComponent<CanvasGroup>().DOFade(0f, 2f);
        StartCoroutine(RewardUIDisappearingSupport());
    }

    private IEnumerator RewardUIDisappearingSupport()
    {
        yield return new WaitForSeconds(2f);
        rewardScreen.SetActive(false);
    }

    public void ARewardWasSelected()
    {

    }

    public void ShuffleRewards()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            GameObject _reward = rewards[i];
            int j = Random.Range(i, rewards.Length);
            rewards[i] = rewards[j];
            rewards[j] = _reward;
        }
    }

}
