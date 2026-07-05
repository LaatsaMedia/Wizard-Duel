using UnityEngine;

[System.Serializable]
public class AccessoryModifier
{
    [SerializeField] private AccessoryEffect effect;
    public AccessoryEffect Effect => effect;

    [SerializeField] private float value;
    public float Value => value;
}