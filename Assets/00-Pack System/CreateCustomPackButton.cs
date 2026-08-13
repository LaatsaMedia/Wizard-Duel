using UnityEngine;
using UnityEngine.UI;

public class CreateCustomPackButton : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button button;

    [Header("Editor")]
    [SerializeField] private CustomPackEditor customPackEditor;

    private void Awake()
    {
        if (button != null)
            button.onClick.AddListener(CreatePack);
    }

    public void CreatePack()
    {
        CustomPackData newPack =
            ScriptableObject.CreateInstance<CustomPackData>();

        newPack.packName = "New Custom Pack";
        newPack.description = "";
        newPack.icon = null;

        customPackEditor.Open(newPack, null);
    }
}