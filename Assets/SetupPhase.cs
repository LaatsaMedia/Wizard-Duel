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
        SpellCategory? spellCategory =
            RunManager.Instance.CurrentSpellCategory;

        if (spellCategory == null)
            return;

        List<Spell> rewards =
            RunManager.Instance.GetRandomSpells(
                spellCategory.Value,
                3);

        foreach (Spell spell in rewards)
        {
            RewardCard card = Instantiate(
                rewardCardPrefab,
                rewardContainer);

            card.Setup(spell);
        }
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

        RunManager.Instance.PlayerBuild.AddSpell(
            selectedCard.Spell);

        RunManager.Instance.GenerateEnemyReward();

        RunManager.Instance.LoadCombat();
    }
}