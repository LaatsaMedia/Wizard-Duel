using UnityEngine;

public class UltimateCharge : MonoBehaviour
{
    [Header("Charge Gain")]
    [SerializeField] private float passiveChargePerSecond = 2f;
    [SerializeField] private float chargePerDamageDealt = 0.6f;
    [SerializeField] private float chargePerDamageTaken = 0.8f;

    public float ChargeGainMultiplier { get; set; } = 1f;

    public float CurrentCharge { get; private set; }

    public bool IsReady => CurrentCharge >= 100f;

    private void Update()
    {
        if (!MatchManager.RoundActive)
            return;

        AddCharge(passiveChargePerSecond * Time.deltaTime);
    }

    public void AddCharge(float amount)
    {
        if (IsReady)
            return;

        amount *= ChargeGainMultiplier;

        CurrentCharge = Mathf.Clamp(
            CurrentCharge + amount,
            0f,
            100f);
    }

    public void AddChargeFromDamageTaken(float damage)
    {
        AddCharge(damage * chargePerDamageTaken);
    }

    public void AddChargeFromDamageDealt(float damage)
    {
        AddCharge(damage * chargePerDamageDealt);
    }

    public void UseUltimate()
    {
        CurrentCharge = 0f;
    }

    public void ResetCharge()
    {
        CurrentCharge = 0f;
    }
}