using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static bool InputEnabled = true;
    [SerializeField] private SpellCaster spellCaster;

    private void Update()
    {
        if (!InputEnabled)
            return;

        if (InputSettings.GetButtonDown(PlayerAction.Primary))
            spellCaster.CastPrimary();

        if (InputSettings.GetButtonDown(PlayerAction.Secondary))
            spellCaster.CastSecondary();

        if (InputSettings.GetButtonDown(PlayerAction.Third))
            spellCaster.CastThird();

        if (InputSettings.GetButtonDown(PlayerAction.Ultimate))
            spellCaster.CastUltimate();
    }
}