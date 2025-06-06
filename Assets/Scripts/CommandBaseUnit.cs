using UnityEngine;

public class CommandBaseUnit : UnitBase
{
    public override void TakeDamage(DamageInfo info)
    {
        base.TakeDamage(info);
        if (isDead)
        {
            GameManager.gm.SetGameOverScreen(GetTeamNumber());
        }
    }
}
