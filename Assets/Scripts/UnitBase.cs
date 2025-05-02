using UnityEngine;

public enum MovementModes
{
    Idle,
    Walking,
    Running,
}

public class UnitBase : MonoBehaviour
{
    [Header("Health Variables")]
    public float currentHealth;
    public float maxHealth;
    public float currentShield;
    public float maxShield;
    [Header("Speed Variables")]
    [SerializeField] private float movementSpeed;
    [Tooltip("Determines the speed of the unit.")]
    public MovementModes movementState;
    [Header("Attack Variables")]
    [Tooltip("Holds the variables for the unit's attack.")]
    public DamageInfo attackInfo;
    public bool isDead;
    private float timeLastAttackMade;
    protected bool canAttack = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(attackInfo.attackSpeed <= 0)
        {
            attackInfo.attackSpeed = 1f;
        }
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Once enough time has passed...
        if (Time.time > timeLastAttackMade + attackInfo.attackSpeed)
        {
            //the unit can attack again.
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public void TakeDamage(DamageInfo info)
    {
        //if the unit has shields deal damage to the shields first
        if (currentShield > 0f)
        {
            if (info.damageAmount > currentShield)
            {
                //if the shields are less than the damage amount then reduce the damage by the shield amount...
                info.damageAmount -= currentShield;
                currentShield = 0f;
                //then deal damage to the unit's health
                currentHealth -= info.damageAmount;
            }
            else
            {
                //else just deal damage to the shields
                currentShield -= info.damageAmount;
            }
            return;
        }



        currentHealth -= ProcessedDamage(info.damageType, info.damageAmount);
        if (currentHealth <= 0f)
        {
            isDead = true;
        }
    }

    public void HealHealth(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth += amount;
        Mathf.Clamp(currentHealth, 1f, maxHealth);
    }

    public void HealShield(float amount)
    {
        currentShield += amount;
        currentShield = Mathf.Clamp(currentShield, 1f, maxShield);
    }

    public virtual void Attack(UnitBase attackTarget)
    {
        if (attackTarget.isDead == false)
        {
            return;
        }
        if (!canAttack)
        {
            return;
        }
        attackTarget.TakeDamage(attackInfo);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMovementSpeed(MovementModes mode)
    {
        switch (mode)
        {
            case MovementModes.Idle:
                movementSpeed = 0f;
                break;
            case MovementModes.Walking:
                movementSpeed = 1f;
                break;
            case MovementModes.Running:
                movementSpeed = 3f;
                break;
            default:
                movementSpeed = 0f;
                break;
        }
    }

    public float ProcessedDamage(DamageType type, float damageAmount)
    {
        if (type == attackInfo.damageType)
        {
            return damageAmount;
        }
        if ((type == DamageType.Melee && attackInfo.damageType == DamageType.Range) || (type == DamageType.Range && attackInfo.damageType == DamageType.Shield)
            || (type == DamageType.Shield && attackInfo.damageType == DamageType.Melee))
        {
            return damageAmount * 2f;
        }

        return damageAmount;
    }
}