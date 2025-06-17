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
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentShield;
    [SerializeField] private float maxShield;
    [SerializeField] private HealthBar healthbar;
    
    [Header("Speed Variables")]
    [SerializeField] private float movementSpeed;
    [Tooltip("Determines the speed of the unit.")]
    [SerializeField] private MovementModes _movementMode;
    public MovementModes movementModeProperty
    {
        get
        {
            return _movementMode;
        }
        set 
        {
            _movementMode = value;
            SetMovementSpeed(_movementMode);
        }
    }
    [Header("Attack Variables")]
    [Tooltip("Holds the variables for the unit's attack.")]
    public DamageInfo attackInfo;
    public bool isDead;
    protected float timeLastAttackMade;
    protected bool canAttack = true;
    public List<UnitBase> targets = new List<UnitBase>();
    [SerializeField] private int teamNumber;
    protected UnitBase enemyBase;
    protected NavMeshAgent _agent;
    public GameObject deathParticle;

    protected void Awake()
    {
        canAttack = true;
        _agent = GetComponent<NavMeshAgent>();
        
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
        healthbar.healthbarSlider.maxValue = maxHealth + maxShield;
        healthbar.UpdateHealthBar(currentHealth + currentShield);
        CommandBaseUnit[] bases = FindObjectsByType<CommandBaseUnit>(FindObjectsSortMode.None);
        for (int i = 0; i < bases.Length; i++)
        {
            if (bases[i].GetTeamNumber() != teamNumber)
            {
                enemyBase = bases[i];
                break;
            }
        }
        
        movementModeProperty = _movementMode;
    }

    // Update is called once per frame
    protected void Update()
    {
        //if we're in the deploy or pause phase make sure the unit is set to idle and stop it from doing anything else in update.
        if (GameManager.gm.state == GameState.Deploy || GameManager.gm.state == GameState.Pause)
        {
            movementModeProperty = MovementModes.Idle;
            return;
        }
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
            //if there are no enemies nearby go attack the base
            movementModeProperty = MovementModes.Running;
            _agent.destination = enemyBase.transform.position;
            if (Vector3.Distance(_agent.destination, transform.position) < attackInfo.damageRange + 2)
            {
                movementModeProperty = MovementModes.Idle;
                Attack(enemyBase);
            }
        }
        else
        {
            //if there's an enemy nearby attack the first enemy in the list
            _agent.destination = targets[0].transform.position;
            if (Vector3.Distance(_agent.destination, transform.position) <= attackInfo.damageRange)
            {
                movementModeProperty = MovementModes.Idle;
                Attack(targets[0]);
            }
            else
            {
                movementModeProperty = MovementModes.Running;
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

    public virtual void TakeDamage(DamageInfo info)
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
            healthbar.UpdateHealthBar(currentHealth + currentShield);
            return;
        }



        currentHealth -= ProcessedDamage(info.damageType, info.damageAmount);
        healthbar.UpdateHealthBar(currentHealth);
        if (currentHealth <= 0f)
        {
            isDead = true;
            ParticleSystem[] deathVFX = deathParticle.GetComponents<ParticleSystem>();
            foreach (ParticleSystem particle in deathVFX)
            {
                particle.Play(true);
            }
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

    public void Attack(UnitBase attackTarget)
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
        timeLastAttackMade = Time.time;
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
        _agent.speed = movementSpeed;
    }

    public float ProcessedDamage(DamageType type, float damageAmount)
    {
        //if the damage the unit takes is the same type as the damage it deals then deal the same amount of damage
        if (type == attackInfo.damageType)
        {
            return damageAmount;
        }
        //if the damage the unit is taking is what it is weak against deal double damage
        if ((type == DamageType.Melee && attackInfo.damageType == DamageType.Range) 
            || (type == DamageType.Range && attackInfo.damageType == DamageType.Shield)
            || (type == DamageType.Shield && attackInfo.damageType == DamageType.Melee))
        {
            return damageAmount * 2f;
        }
        //else return the damage amount without any changes
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