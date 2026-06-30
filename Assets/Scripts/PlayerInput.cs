using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private SpellCaster spellCaster;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            spellCaster.CastPrimary();

        if (Input.GetMouseButtonDown(1))
            spellCaster.CastSecondary();

        if (Input.GetKeyDown(KeyCode.E))
            spellCaster.CastThird();
    }
}