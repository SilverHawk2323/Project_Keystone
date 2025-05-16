using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementModes
{
    Idle,
    Walking,
    Running,
}

[RequireComponent(typeof(NavMeshAgent))]
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
    protected float timeLastAttackMade;
    protected bool canAttack = true;
    public List<UnitBase> targets = new List<UnitBase>();
    public int teamNumber;
    protected UnitBase enemyBase;
    protected NavMeshAgent _agent;

    protected void Awake()
    {
        canAttack = true;
        _agent = GetComponent<NavMeshAgent>();
        CommandBaseUnit[] bases = FindObjectsByType<CommandBaseUnit>(FindObjectsSortMode.None);
        for (int i = 0; i < bases.Length; i++)
        {
            if(bases[i].GetTeamNumber() != teamNumber)
            {
                enemyBase = bases[i];
                break;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
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
    protected void Update()
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
        if(targets.Count <= 0)
        {
            _agent.destination = enemyBase.transform.position;
            if (Vector3.Distance(_agent.destination, transform.position) < attackInfo.damageRange)
            {
                Attack(enemyBase);
            }
        }
        else
        {
            _agent.destination = targets[0].transform.position;
            if (Vector3.Distance(_agent.destination, transform.position) <= attackInfo.damageRange)
            {
                Attack(targets[0]);
            }
        }
        
    }

    public void SetTeamNumber(int teamNumber)
    {
        this.teamNumber = teamNumber;
    }

    public int GetTeamNumber()
    {
        return teamNumber;
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
            gameObject.SetActive(false);
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
        if (attackTarget.isDead == true)
        {
            RemoveFromList(attackTarget);
            return;
        }
        if (!canAttack)
        {
            return;
        }
        print("Dealt Damage");
        attackTarget.TakeDamage(attackInfo);
        RemoveFromList(attackTarget);
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

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<UnitBase>(out UnitBase enemy))
        {
            if(enemy.teamNumber != teamNumber && !targets.Contains(enemy))
            {
                targets.Add(enemy);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<UnitBase>(out UnitBase enemy))
        {
            if(enemy.teamNumber != teamNumber)
            {
                targets.Remove(enemy);
            }
        }
    }

    private void RemoveFromList(UnitBase target)
    {
        if (targets.Contains(target) && target.isDead == true)
        {
            targets.Remove(target);
        }
    }
}