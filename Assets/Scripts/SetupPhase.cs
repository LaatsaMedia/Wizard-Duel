using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPhase : MonoBehaviour
{
    public static SetupPhase Instance { get; private set; }

    [SerializeField] private GameObject selectedDarkness;

    [SerializeField] private RewardCard rewardCardPrefab;
    [SerializeField] private Transform rewardContainer;

    [Header("UI")]
    [SerializeField] private TMP_Text primaryButtonText;
    [SerializeField] private Button rerollButton;

    private RewardCard selectedCard;
    private BuildSlot? selectedBuildSlot;

    private bool IsRewardSelected => selectedCard != null;
    private bool IsBuildSlotSelected => selectedBuildSlot.HasValue;

    private readonly List<Spell> lastShownSpells = new();
    private readonly List<Accessory> lastShownAccessories = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateRewards();
        UpdatePrimaryButton();
        UpdateSelectedDarkness();
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

            case RewardCategory.Ultimate:
                GenerateUltimateRewards();
                break;

            case RewardCategory.Accessory:
                GenerateAccessoryRewards();
                break;
        }
    }

    private void GenerateUltimateRewards()
    {
        Spell spell =
            RunManager.Instance.GetRandomUltimateSpell(
                RunManager.Instance.PlayerBuild);

        if (spell == null)
            return;

        RewardCard card = Instantiate(
            rewardCardPrefab,
            rewardContainer);

        card.Setup(spell);
    }

    private void GenerateSpellRewards()
{
    // Only prevent rewards from the previous roll.
    List<Spell> excludedSpells = new(lastShownSpells);

    // Forget the previous rewards.
    lastShownSpells.Clear();

    // Roll the reward tier ONCE for this reward screen.
    SpellMastery rewardTier =
        RunManager.Instance.GetRewardMastery();

    int normalSpellCount = 3;

    if (RunManager.Instance.ShouldGenerateUltimateReward())
    {
        normalSpellCount = 2;
        GenerateUltimateReward(excludedSpells);
    }

    int generated = 0;
    int attempts = 0;

    while (generated < normalSpellCount && attempts < 20)
    {
        attempts++;

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
                rewardTier,
                excludedSpells);

        if (rewards.Count == 0)
            continue;

        Spell spell = rewards[0];

        excludedSpells.Add(spell);
        lastShownSpells.Add(spell);

        RewardCard card = Instantiate(
            rewardCardPrefab,
            rewardContainer);

        card.Setup(spell);

        generated++;
    }
}

    private void GenerateUltimateReward(List<Spell> excludedSpells)
    {
        List<Spell> ultimates =
            RunManager.Instance.GetUltimateSpells(
                RunManager.Instance.PlayerBuild);

        ultimates.RemoveAll(excludedSpells.Contains);

        if (ultimates.Count == 0)
            return;

        Spell spell = ultimates[
            Random.Range(0, ultimates.Count)];

        excludedSpells.Add(spell);
        lastShownSpells.Add(spell);

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
                3,
                lastShownAccessories);

        lastShownAccessories.Clear();

        foreach (Accessory accessory in rewards)
        {
            lastShownAccessories.Add(accessory);

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
        UpdateSelectedDarkness();
    }

    public void SelectCard(RewardCard card)
    {
        if (selectedCard == card)
        {
            selectedCard.SetSelected(false);
            selectedCard = null;

            SpellbookUI.Instance.SetSelectable(false);

            UpdatePrimaryButton();
            UpdateSelectedDarkness();
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
        UpdateSelectedDarkness();
    }

    private void UpdateSelectedDarkness()
    {
        if (selectedDarkness == null)
            return;

        bool show =
            IsRewardSelected &&
            selectedCard.Spell != null;

        selectedDarkness.SetActive(show);
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