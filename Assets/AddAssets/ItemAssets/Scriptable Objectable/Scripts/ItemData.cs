using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Resource,  // �ڿ�
    Equipable, // ����
    Consumable // �Ҹ�
}

public enum ConsumableType
{
    Health, // ����
    Hunger, // �����
    Stress, // ��Ʈ����
    Stemina // ���׹̳�
}

public class ItemDataConsumable // � Ÿ�Կ� � �ɷ�ġ
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject // ���
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
    internal object consumables;
}
