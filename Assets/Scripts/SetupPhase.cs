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
        List<Spell> generatedSpells = new();

        int normalSpellCount = 3;

        if (RunManager.Instance.ShouldGenerateUltimateReward())
        {
            normalSpellCount = 2;

            GenerateUltimateReward(generatedSpells);
        }

        for (int i = 0; i < normalSpellCount; i++)
        {
            SpellCategory category =
                RunManager.Instance.CurrentRewardCategory == RewardCategory.Any
                ? RunManager.Instance.GetRandomSpellCategory()
                : RunManager.Instance.GetRewardSpellCategory();

            List<Spell> rewards =
                RunManager.Instance.GetRandomSpells(
                    category,
                    RunManager.Instance.PlayerBuild,
                    1,
                    generatedSpells);

            if (rewards.Count == 0)
                continue;

            Spell spell = rewards[0];

            generatedSpells.Add(spell);

            RewardCard card = Instantiate(
                rewardCardPrefab,
                rewardContainer);

            card.Setup(spell);
        }
    }

    private void GenerateUltimateReward(
    List<Spell> generatedSpells)
    {
        Spell spell =
            RunManager.Instance.GetRandomUltimateSpell(
                RunManager.Instance.PlayerBuild);

        if (spell == null)
            return;

        generatedSpells.Add(spell);

        RewardCard card = Instantiate(
            rewardCardPrefab,
            rewardContainer);

        card.Setup(spell);
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

                if (selectedCard.Spell.Mastery == SpellMastery.Archmage)
                {
                    RunManager.Instance.PlayerBuild.SetUltimateSpell(
                        selectedCard.Spell);
                }
                else
                {
                    RunManager.Instance.PlayerBuild.AddSpell(
                        selectedCard.Spell);
                }

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