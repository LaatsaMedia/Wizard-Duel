using TMPro;
using UnityEngine;

public class ControlBindingUI : MonoBehaviour
{
    [SerializeField] private PlayerAction action;
    [SerializeField] private TMP_Text bindingText;

    private bool waitingForInput;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if (!waitingForInput)
            return;

        // Mouse buttons
        for (int i = 0; i <= 2; i++)
        {
            if (!Input.GetMouseButtonDown(i))
                continue;

            InputSettings.SetBinding(
                action,
                new InputBinding
                {
                    useMouse = true,
                    mouseButton = i,
                    key = KeyCode.None
                });

            FinishBinding();
            return;
        }

        // Keyboard keys
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (!Input.GetKeyDown(key))
                continue;

            InputSettings.SetBinding(
                action,
                new InputBinding
                {
                    useMouse = false,
                    mouseButton = -1,
                    key = key
                });

            FinishBinding();
            return;
        }
    }

    public void Rebind()
    {
        waitingForInput = true;
        bindingText.text = "Press any key...";
    }

    private void FinishBinding()
    {
        waitingForInput = false;
        Refresh();
    }

    private void Refresh()
    {
        bindingText.text =
            InputSettings.GetBindingName(action);
    }
}