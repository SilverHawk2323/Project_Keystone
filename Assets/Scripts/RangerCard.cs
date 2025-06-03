using UnityEngine;

public class RangerCard : CardBase
{
    public override void UseAbility()
    {
        foreach (var unit in GameManager.gm.friendlyUnits)
        {
            unit.attackInfo.attackSpeed /= 2f;
        }
        Destroy(gameObject);
    }
}
