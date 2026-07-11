using System.Collections.Generic;
using UnityEngine;

public static class InputSettings
{
    private static readonly Dictionary<PlayerAction, InputBinding> bindings =
        new Dictionary<PlayerAction, InputBinding>();

    static InputSettings()
    {
        LoadBindings();
    }

    private static void LoadBindings()
    {
        LoadBinding(
            PlayerAction.Primary,
            true,
            0,
            KeyCode.None);

        LoadBinding(
            PlayerAction.Secondary,
            true,
            1,
            KeyCode.None);

        LoadBinding(
            PlayerAction.Third,
            false,
            -1,
            KeyCode.E);

        LoadBinding(
            PlayerAction.Ultimate,
            false,
            -1,
            KeyCode.F);
    }

    private static void LoadBinding(
        PlayerAction action,
        bool defaultUseMouse,
        int defaultMouseButton,
        KeyCode defaultKey)
    {
        bool useMouse =
            PlayerPrefs.GetInt(
                $"{action}_UseMouse",
                defaultUseMouse ? 1 : 0) == 1;

        int mouseButton =
            PlayerPrefs.GetInt(
                $"{action}_MouseButton",
                defaultMouseButton);

        KeyCode key =
            (KeyCode)PlayerPrefs.GetInt(
                $"{action}_Key",
                (int)defaultKey);

        bindings[action] = new InputBinding
        {
            useMouse = useMouse,
            mouseButton = mouseButton,
            key = key
        };
    }

    public static bool GetButtonDown(PlayerAction action)
    {
        InputBinding binding = bindings[action];

        if (binding.useMouse)
            return Input.GetMouseButtonDown(binding.mouseButton);

        return Input.GetKeyDown(binding.key);
    }

    public static InputBinding GetBinding(PlayerAction action)
    {
        return bindings[action];
    }

    public static void SetBinding(
        PlayerAction action,
        InputBinding binding)
    {
        bindings[action] = binding;

        PlayerPrefs.SetInt(
            $"{action}_UseMouse",
            binding.useMouse ? 1 : 0);

        PlayerPrefs.SetInt(
            $"{action}_MouseButton",
            binding.mouseButton);

        PlayerPrefs.SetInt(
            $"{action}_Key",
            (int)binding.key);

        PlayerPrefs.Save();
    }

    public static string GetBindingName(PlayerAction action)
    {
        InputBinding binding = bindings[action];

        if (binding.useMouse)
            return $"Mouse {binding.mouseButton}";

        return binding.key.ToString();
    }
}