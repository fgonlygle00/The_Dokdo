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
            // �þ߰� �������� �ӵ��� �����ϴ� ���� ���� ��ȭ ����
            // �� �κ��� ��Ȳ�� ���� �����ؾ� �մϴ�.
            // fsm �� ��Ʈ�������³ְ� stress �� ������ canvas ��� �̹���
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
        // ���͸� �ٶ� ��, ��Ʈ���� ����
        // �þ� ������
        // �ӵ� �پ��
        // �޴� ������ �߰�
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
        Debug.Log("�÷��̾ �׾���.");
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

    // �ܺο��� ������ ������ �޴��� ���θ� ������ �� �ִ� �޼���
    public void SetAffectedByMonster(bool value)
    {
        isAffectedByMonster = value;
    }
}