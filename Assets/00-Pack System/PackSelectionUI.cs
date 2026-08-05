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

        startButton.interactable = true;
    }
}