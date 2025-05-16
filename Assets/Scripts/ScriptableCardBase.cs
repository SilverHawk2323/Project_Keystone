using UnityEngine;

[CreateAssetMenu(fileName = "CardBase", menuName = "Scriptable Objects/CardBase")]
public class ScriptableCardBase : ScriptableObject
{
    public int cost;
    public UnitBase unit;
    public int teamNumber;

    
}
