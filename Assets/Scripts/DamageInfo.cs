using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public float damageAmount;
    public DamageType damageType;
    public float damageRange;
    public float attackSpeed;
    public bool canBeBlock;
}

public enum DamageType
{
    None,
    Melee,
    Shield,
    Range,
}