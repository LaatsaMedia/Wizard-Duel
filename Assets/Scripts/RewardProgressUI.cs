using UnityEngine;

public class RewardProgressUI : MonoBehaviour
{
    [SerializeField] private RewardProgressNode[] nodes;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        RewardProgression progression =
            RunManager.Instance.RewardProgression;

        int currentIndex =
            RunManager.Instance.CurrentRewardIndex;

        for (int i = 0; i < nodes.Length; i++)
        {
            int rewardIndex = currentIndex + i;

            if (rewardIndex >= progression.rewardOrder.Length)
            {
                nodes[i].gameObject.SetActive(false);
                continue;
            }

            nodes[i].gameObject.SetActive(true);

            nodes[i].Setup(
                progression.rewardOrder[rewardIndex],
                i == 0);
        }
    }
}