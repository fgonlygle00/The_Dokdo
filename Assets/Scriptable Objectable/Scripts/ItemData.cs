using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Resource,  // 자원
    Equipable, // 장착
    Consumable // 소모
}

public enum ConsumableType
{
    Health, // 아픔
    Hunger, // 배고픔
    Stress, // 스트레스
    Stemina // 스테미나
}

[System.Serializable]
public class ItemDataConsumable // 어떤 타입에 어떤 능력치
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject // 상속
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

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
    internal int Count;
}
