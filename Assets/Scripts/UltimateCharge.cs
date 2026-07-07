using UnityEngine;

public class UltimateCharge : MonoBehaviour
{
    [SerializeField] private float passiveChargePerSecond = 2f;

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

        CurrentCharge = Mathf.Clamp(
            CurrentCharge + amount,
            0f,
            100f);
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