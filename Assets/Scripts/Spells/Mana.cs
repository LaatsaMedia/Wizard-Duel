using UnityEngine;
using System.Collections;

public class Mana : MonoBehaviour
{
    public float currentMana = 20f;
    public float maxMana = 20f;
    public float manaRegen = 2f;
    public float CurrentManaPercent => currentMana / maxMana;

    public float CurrentMana => currentMana;
    public float MaxMana => maxMana;
    public float ManaPercent => currentMana / maxMana;

    public bool canRegenMana = true;

    private void Update()
    {
        if (!canRegenMana)
            return;

        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }

    public bool TrySpendMana(float amount)
    {
        if (currentMana < amount)
            return false;

        currentMana -= amount;
        return true;
    }

    public void DisableManaRegen(float duration)
    {
        StartCoroutine(DisableManaRegenRoutine(duration));
    }

    private IEnumerator DisableManaRegenRoutine(float duration)
    {
        canRegenMana = false;

        yield return new WaitForSeconds(duration);

        canRegenMana = true;
    }
}