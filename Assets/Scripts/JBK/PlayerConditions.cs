using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

//public interface IDamagable
//{
//    void TakePhysicalDamage(int damageAmount);
//}

[System.Serializable]
public class Condition
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }

}

public class PlayerConditions : MonoBehaviour, IDamagable
{
    public Condition health;
    public Condition stress;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay;

    public UnityEvent onTakeDamage;

    private bool isFacingMonster = false;

    public float lineSize = 16f;

    public Volume vg;

    public float vignetteIntensityIncrement = 0.1f;

    void Start()
    {
        health.curValue = health.startValue;
        stress.curValue = stress.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * lineSize, Color.yellow);

        hunger.Subtract(hunger.decayRate * Time.deltaTime);

        stress.Subtract(stress.decayRate * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && stamina.curValue > 0f)
        {
            stamina.Subtract(stamina.decayRate * Time.deltaTime);
        }
        else
        {
            stamina.Add(stamina.regenRate * Time.deltaTime);
        }

        if (hunger.curValue == 0.0f)
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f)
            Die();

        if (isFacingMonster == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, lineSize))
            {
                if (hit.collider.CompareTag("Monster"))
                {
                    if (vg.profile.TryGet(out Vignette vignette)) // for e.g set vignette intensity to .4f
                    {
                        vignette.intensity.value += vignetteIntensityIncrement * Time.deltaTime;
                        // stamina.Add(stamina.regenRate * Time.deltaTime);

                    }
                }
                else
                {
                    if (vg.profile.TryGet(out Vignette vignette)) // for e.g set vignette intensity to .4f
                    {
                        vignette.intensity.value -= vignetteIntensityIncrement * Time.deltaTime;
                        // stamina.Subtract(stamina.regenRate * Time.deltaTime);
                    }
                }
            }
        }

        health.uiBar.fillAmount = health.GetPercentage();
        stress.uiBar.fillAmount = stress.GetPercentage();
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public bool getStress(float amount)
    {
        if (stress.curValue + amount < 0)
            return false;

        stress.Add(amount);
        return true;
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
            return false;

        stamina.Subtract(amount);
        return true;
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}