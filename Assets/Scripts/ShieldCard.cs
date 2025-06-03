using UnityEngine;

public class ShieldCard : CardBase
{
    public override void UseAbility()
    {
        foreach(var unit in GameManager.gm.friendlyUnits)
        {
            unit.HealShield(4f);
        }
        Destroy(gameObject);
    }
}
