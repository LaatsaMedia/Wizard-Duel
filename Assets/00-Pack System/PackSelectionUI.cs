using UnityEngine;
using UnityEngine.UI;

public class PackSelectionUI : MonoBehaviour
{
    public static PackSelectionUI Instance { get; private set; }

    [SerializeField] private Button startButton;

    private PackSelectionButton selectedButton;

    private void Awake()
    {
        Instance = this;

        startButton.interactable = false;
    }

    public void Select(PackSelectionButton button)
    {
        if (selectedButton != null)
        {
            selectedButton.SetSelected(false);
        }

        selectedButton = button;
        selectedButton.SetSelected(true);

        GameSettings.SelectedPack = button.Pack;
        GameSettings.SelectedCustomPack = null;

        startButton.interactable = true;
    }

    public void SelectPack(PackDefinition pack)
    {
        if (pack == null)
            return;

        PackSelectionButton[] buttons =
            FindObjectsOfType<PackSelectionButton>(true);

        foreach (PackSelectionButton button in buttons)
        {
            if (button.Pack != pack)
                continue;

            Select(button);
            return;
        }

        Debug.LogWarning(
            $"PackSelectionUI: No PackSelectionButton found for {pack.PackName}.");
    }

    public void SelectCustomPack(CustomPackData pack)
    {
        if (pack == null)
            return;

        if (!pack.IsComplete)
            return;

        if (selectedButton != null)
        {
            selectedButton.SetSelected(false);
            selectedButton = null;
        }

        GameSettings.SelectedPack = null;
        GameSettings.SelectedCustomPack = pack;

        startButton.interactable = true;
    }
}