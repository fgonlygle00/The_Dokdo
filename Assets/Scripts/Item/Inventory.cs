using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static ItemSlot; // 삭제 예정

public class ItemSlot
{
    public ItemData item;
    public int quantity;

    public class ItemData  // 삭제 예정
    {
        internal readonly Sprite icon; // 삭제 예정
        internal bool canStack;  // 삭제 예정
    }
}

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerConditions condition;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    public static Inventory instance;
    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerConditions>();
    }
    private void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }

        ClearSeletecItemWindow();
    }

    private class PlayerController  // 삭제 예정
    {
    }

    private class PlayerConditions  // 삭제 예정
    {
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        void ThrowItem(ItemData item)
        {

        }

        void UpdateUI()
        {

        }

        ItemSlot GetItemStack(ItemData item)
        {
            return null;
        }

        ItemSlot GetEmptySlot()
        {
            return null;
        }
    }

    public void SelectItem(int index)
    {

    }

    private void ClearSeletecItemWindow()
    {

    }

    public void OnUseButton()
    {

    }

    public void OnEquipButton()
    {

    }

    void UnEquip(int index)
    {

    }

    public void OnUnEquipButton()
    {

    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    private void ThrowItem(ItemData item)
    {
        throw new NotImplementedException();
    }

    private void RemoveSelectedItem()
    {

    }

    public void RemoveItem(ItemData item)
    {

    }

    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }
}