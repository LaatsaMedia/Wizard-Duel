using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPhase : MonoBehaviour
{
    public static SetupPhase Instance { get; private set; }

    [SerializeField] private RewardCard rewardCardPrefab;
    [SerializeField] private Transform rewardContainer;

    [Header("UI")]
    [SerializeField] private TMP_Text primaryButtonText;
    [SerializeField] private Button rerollButton;

    private RewardCard selectedCard;
    private BuildSlot? selectedBuildSlot;

    private bool IsRewardSelected => selectedCard != null;
    private bool IsBuildSlotSelected => selectedBuildSlot.HasValue;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateRewards();
        UpdatePrimaryButton();
    }

    private void GenerateRewards()
    {
        rerollButton.interactable = true;
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
                : RunManager.Instance.GetRewardSpellCategory(
                    RunManager.Instance.CurrentRewardCategory);

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

    private void GenerateUltimateReward(List<Spell> generatedSpells)
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
        selectedBuildSlot = null;

        foreach (Transform child in rewardContainer)
        {
            Destroy(child.gameObject);
        }

        GenerateRewards();
        UpdatePrimaryButton();
    }

    public void SelectCard(RewardCard card)
    {
        if (selectedCard == card)
        {
            selectedCard.SetSelected(false);
            selectedCard = null;

            SpellbookUI.Instance.SetSelectable(false);

            UpdatePrimaryButton();
            return;
        }

        if (selectedCard != null)
        {
            selectedCard.SetSelected(false);
        }

        selectedCard = card;
        selectedCard.SetSelected(true);

        // NEW
        SpellbookUI.Instance.SetSelectable(true);

        UpdatePrimaryButton();
    }

    public void SelectBuildSlot(BuildSlot slot)
    {
        if (!IsRewardSelected)
            return;

        bool isUltimateSpell =
            selectedCard.Spell.Mastery == SpellMastery.Archmage;

        // Ultimate spells can only go into the Ultimate slot.
        if (isUltimateSpell && slot != BuildSlot.Ultimate)
            return;

        // Normal spells cannot go into the Ultimate slot.
        if (!isUltimateSpell && slot == BuildSlot.Ultimate)
            return;

        if (IsBuildSlotSelected)
            return;

        selectedBuildSlot = slot;

        RunManager.Instance.PlayerBuild.SetSpell(
            slot,
            selectedCard.Spell);

        SpellbookUI.Instance.Refresh(
            RunManager.Instance.PlayerBuild);

        SpellbookUI.Instance.SetSelectable(false);

        rerollButton.interactable = false;

        UpdatePrimaryButton();
    }

    private void UpdatePrimaryButton()
    {
        if (!IsRewardSelected)
        {
            primaryButtonText.text = "Skip";
            return;
        }

        if (RunManager.Instance.CurrentRewardCategory == RewardCategory.Accessory)
        {
            primaryButtonText.text = "Ready";
            return;
        }

        primaryButtonText.text =
            IsBuildSlotSelected
            ? "Ready"
            : "Choose Slot";
    }

public void PrimaryButton()
{
    if (!IsRewardSelected)
    {
        SkipReward();
        return;
    }

    bool requiresBuildSlot =
        RunManager.Instance.CurrentRewardCategory != RewardCategory.Accessory;

    if (requiresBuildSlot && !IsBuildSlotSelected)
    {
        Debug.Log("Choose a slot first.");
        return;
    }

    ConfirmReward();
}

    private void SkipReward()
    {
        RunManager.Instance.GenerateEnemyBuild();
        RunManager.Instance.LoadCombat();
    }

    private void ConfirmReward()
    {
        if (selectedCard.Accessory != null)
        {
            RunManager.Instance.PlayerBuild.AddAccessory(
                selectedCard.Accessory);
        }

        RunManager.Instance.GenerateEnemyBuild();
        RunManager.Instance.LoadCombat();
    }
}