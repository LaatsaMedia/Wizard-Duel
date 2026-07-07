using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private SpellCaster spellCaster;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            spellCaster.CastPrimary();

        if (Input.GetMouseButton(1))
            spellCaster.CastSecondary();

        if (Input.GetKey(KeyCode.E))
            spellCaster.CastThird();

        if(Input.GetKeyDown(KeyCode.F))
            spellCaster.CastUltimate();
    }
}