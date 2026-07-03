using System.Collections.Generic;
using UnityEngine;

public class SetupPhase : MonoBehaviour
{
    public static SetupPhase Instance { get; private set; }

    [SerializeField] private RewardCard rewardCardPrefab;
    [SerializeField] private Transform rewardContainer;

    private RewardCard selectedCard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateRewards();
    }

    private void GenerateRewards()
    {
        switch (RunManager.Instance.CurrentRewardCategory)
        {
            case RewardCategory.Offensive:
            case RewardCategory.Utility:
            case RewardCategory.Disable:
            case RewardCategory.Defensive:
            case RewardCategory.Any:
                GenerateSpellRewards();
                break;

            case RewardCategory.Accessory:
                GenerateAccessoryRewards();
                break;
        }
    }

    private void GenerateSpellRewards()
    {
        SpellCategory? spellCategory =
            RunManager.Instance.CurrentSpellCategory;

        if (spellCategory == null)
            return;

        List<Spell> rewards =
            RunManager.Instance.GetRandomSpells(
                spellCategory.Value,
                RunManager.Instance.PlayerBuild,
                3);

        foreach (Spell spell in rewards)
        {
            RewardCard card = Instantiate(
                rewardCardPrefab,
                rewardContainer);

            card.Setup(spell);
        }
    }

    private void GenerateAccessoryRewards()
    {
        List<Accessory> rewards =
            RunManager.Instance.GetRandomAccessories(
                RunManager.Instance.PlayerBuild,
                3);

        foreach (Accessory accessory in rewards)
        {
            RewardCard card = Instantiate(
                rewardCardPrefab,
                rewardContainer);

            card.Setup(accessory);
        }
    }

    public void Reroll()
    {
        selectedCard = null;

        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }

        GenerateRewards();
    }

    public void SelectCard(RewardCard card)
    {
        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        selectedCard = card;
        selectedCard.SetSelected(true);
    }

    public void Ready()
    {
        if (selectedCard == null)
            return;

        switch (RunManager.Instance.CurrentRewardCategory)
        {
            case RewardCategory.Offensive:
            case RewardCategory.Utility:
            case RewardCategory.Disable:
            case RewardCategory.Defensive:
            case RewardCategory.Any:
                RunManager.Instance.PlayerBuild.AddSpell(
                    selectedCard.Spell);
                break;

            case RewardCategory.Accessory:
                RunManager.Instance.PlayerBuild.AddAccessory(
                    selectedCard.Accessory);
                break;
        }

        RunManager.Instance.GenerateEnemyReward();
        RunManager.Instance.LoadCombat();
    }
}