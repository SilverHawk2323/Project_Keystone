using UnityEngine;

public class WarriorCard : CardBase
{
    public override void UseAbility()
    {
        foreach(var unit in GameManager.gm.friendlyUnits)
        {
            unit.attackInfo.damageAmount += 1;
            print("Used Ability");
        }
        Destroy(gameObject);
    }
}
