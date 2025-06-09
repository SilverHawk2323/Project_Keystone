using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _reduceSpeed = 2f;
    public Slider healthbarSlider;
    private float target;

    public void UpdateHealthBar(float currentHealth)
    {
        target = currentHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - GameManager.gm.mc.transform.position);
        healthbarSlider.value = Mathf.MoveTowards(healthbarSlider.value, target, _reduceSpeed * Time.deltaTime);
    }
}
