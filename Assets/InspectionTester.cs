using UnityEngine;

public class InspectionTester : MonoBehaviour
{
    [SerializeField] private InformationPanel panel;
    [SerializeField] private Spell spell;

    private void Start()
    {
        panel.Display(SpellInspectionBuilder.Build(spell));
    }
}