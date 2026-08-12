using UnityEngine;
using UnityEngine.UI;

public class PackInspectionButton : MonoBehaviour
{
    [SerializeField] private PackDefinition pack;
    [SerializeField] private PackSelectionButton selectionButton;
    [SerializeField] private PackPreviewPanel packPreviewPanel;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(Inspect);
    }

    public void Inspect()
    {
        if (pack == null)
            return;

        packPreviewPanel.Open(pack, selectionButton);
    }
}