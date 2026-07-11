using TMPro;
using UnityEngine;

public class RunHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text livesText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        RunManager run = RunManager.Instance;

        if (run == null)
            return;

        if (run.CurrentStage == run.MaxRounds)
        {
            roundText.text = "Final Round";
        }
        else
        {
            roundText.text =
                $"{run.CurrentStage}/{run.MaxRounds}";
        }

        livesText.text =
            $"{run.CurrentLives}/{run.MaxLives}";
    }
}