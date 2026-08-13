using UnityEngine;
using UnityEngine.UI;

public class CustomPackIconEntry : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private Sprite selectedIcon;
    private CustomPackIconSelectionPanel panel;

    private void Awake()
    {
        if (button != null)
            button.onClick.AddListener(Select);
    }

    public void Setup(
        Sprite icon,
        CustomPackIconSelectionPanel panel)
    {
        selectedIcon = icon;
        this.panel = panel;

        if (this.icon != null)
            this.icon.sprite = icon;
    }

    private void Select()
    {
        if (panel == null)
            return;

        panel.SelectIcon(selectedIcon);
    }
}