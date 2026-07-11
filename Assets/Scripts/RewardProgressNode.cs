using UnityEngine;
using UnityEngine.UI;

public class RewardProgressNode : MonoBehaviour
{
    [SerializeField] private Image border;
    [SerializeField] private Image icon;

    [SerializeField] private Sprite normalBorder;
    [SerializeField] private Sprite currentBorder;

    [SerializeField] private RewardCategoryVisual[] visuals;

    public void Setup(
        RewardCategory category,
        bool current)
    {
        border.sprite = current
            ? currentBorder
            : normalBorder;

        foreach (RewardCategoryVisual visual in visuals)
        {
            if (visual.category != category)
                continue;

            icon.sprite = visual.icon;
            return;
        }
    }
}