using UnityEngine;
using UnityEngine.UI;

public class PackSelectionButton : MonoBehaviour
{
    [SerializeField] private PackDefinition pack;
    public PackDefinition Pack => pack;

    [SerializeField] private Button button;

    [Header("Selection")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 selectedScale = new(1.1f, 1.1f, 1f);

    private void Awake()
    {
        button.onClick.AddListener(Select);
    }

    public void Select()
    {
        PackSelectionUI.Instance.Select(this);
    }

    public void SetSelected(bool selected)
    {
        transform.localScale =
            selected
            ? selectedScale
            : normalScale;
    }
}