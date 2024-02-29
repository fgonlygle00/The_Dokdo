using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    private bool isRunning = false;
    public float runningStaminaDrainRate = 10f;

    private bool isFacingMonster = false;
    private bool isAffectedByMonster = false;


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
        hunger.Subtract(hunger.decayRate * Time.deltaTime);

        stress.Subtract(stress.decayRate * Time.deltaTime);

        if (hunger.curValue == 0.0f)
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f)
            Die();

        if (!isRunning)
            stamina.Add(stamina.regenRate * Time.deltaTime);

        if (isAffectedByMonster)
        {
            // 시야가 좁아지고 속도가 감소하는 등의 상태 변화 적용
            // 이 부분은 상황에 따라서 구현해야 합니다.
            // fsm 에 스트레스상태넣고 stress 를 받을때 canvas 까만색 이미지
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

    public void getStress(float amount)
    {
        // 몬스터를 바라볼 시, 스트레스 증가
        // 시야 좁아짐
        // 속도 줄어듦
        // 받는 데미지 추가
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

    public void StartRunning()
    {
        if (UseStamina(runningStaminaDrainRate * Time.deltaTime))
        {
            isRunning = true;
        }
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    public void SetFacingMonster(bool value)
    {
        isFacingMonster = value;
    }

    // 외부에서 몬스터의 영향을 받는지 여부를 설정할 수 있는 메서드
    public void SetAffectedByMonster(bool value)
    {
        isAffectedByMonster = value;
    }
}