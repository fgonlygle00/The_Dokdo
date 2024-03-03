using UnityEngine;
using static UnityEditor.Progress;

public class UseShop : MonoBehaviour
{
    public GameObject shopWindow;
    public ItemData[] itemData;

    private void OnTriggerEnter(Collider other)
    {
        shopWindow.SetActive(true);
        PlayerController.instance.ToggleCursor(true);
    }

    private void OnTriggerExit(Collider other)
    {
        shopWindow.SetActive(false);
        PlayerController.instance.ToggleCursor(false);
    }

    // 사과x1 = 나무x2
    public void ShopList1()
    {
        if (shopWindow.activeInHierarchy)
        {
            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null && Inventory.instance.slots[i].item.displayName == "나뭇가지")
                {
                    if (Inventory.instance.slots[i].quantity >= 2)
                    {
                        Inventory.instance.slots[i].quantity -= 2;
                        Inventory.instance.AddItem(itemData[0]);
                        RemoveItem(i);
                        return;
                    }
                    else Debug.Log("나뭇가지가 부족합니다");
                }
            }
        }
    }

    // 바나나x1 = 돌x1
    public void ShopList2()
    {
        if (shopWindow.activeInHierarchy)
        {
            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null && Inventory.instance.slots[i].item.displayName == "돌")
                {
                    if (Inventory.instance.slots[i].quantity >= 1)
                    {
                        Inventory.instance.slots[i].quantity--;
                        Inventory.instance.AddItem(itemData[0]);
                        RemoveItem(i);
                        return;
                    }
                    else Debug.Log("돌이 부족합니다");
                }
            }
        }
    }

    // 고기x1 = 나무x5 + 돌x3
    public void ShopList3()
    {
        if (shopWindow.activeInHierarchy)
        {
            int woodIndex = -1;
            int stoneIndex = -1;

            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null)
                {
                    if (Inventory.instance.slots[i].item.displayName == "나뭇가지")
                    {
                        woodIndex = i;
                    }
                    else if (Inventory.instance.slots[i].item.displayName == "돌")
                    {
                        stoneIndex = i;
                    }
                }

                if (woodIndex != -1 && stoneIndex != -1) // 두 아이템이 모두 인벤토리에 있는 경우
                {
                    if (Inventory.instance.slots[woodIndex].quantity >= 5 && Inventory.instance.slots[stoneIndex].quantity >= 3)
                    {
                        Inventory.instance.slots[woodIndex].quantity -= 5;
                        Inventory.instance.slots[stoneIndex].quantity -= 3;
                        Inventory.instance.AddItem(itemData[0]);
                        RemoveItem(woodIndex);
                        RemoveItem(stoneIndex);
                        return;
                    }
                }
            }
        }
    }

    private void RemoveItem(int index)
    {
        if (Inventory.instance.slots[index].quantity <= 0)
        {
            Inventory.instance.slots[index].item = null;
            Inventory.instance.ClearSeletecItemWindow();
        }
        Inventory.instance.UpdateUI();
    }
}
