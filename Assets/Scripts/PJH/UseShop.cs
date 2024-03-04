using UnityEngine;

public class UseShop : MonoBehaviour
{
    public GameObject shopWindow;
    public ItemData[] itemData;

    private void OnTriggerEnter(Collider other)
    {
        shopWindow.SetActive(true);
        PlayerController.instance.ToggleCursor(true);
        PurchaseInfoAlarm.instance.AlarmTextAlphaZero();
    }

    private void OnTriggerExit(Collider other)
    {
        shopWindow.SetActive(false);
        PlayerController.instance.ToggleCursor(false);
    }


    // ���x1 = ����x2
    public void ShopList1()
    {
        if (shopWindow.activeInHierarchy)
        {
            int woodIndex = -1;

            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null)
                {
                    if (Inventory.instance.slots[i].item.displayName == "��������" && woodIndex == -1)
                        woodIndex = i;
                }
            }

            if (woodIndex != -1) // ���� �������� ���������� �ִٴ� �Ҹ�
            {
                if (Inventory.instance.slots[woodIndex].quantity >= 2)
                {
                    Inventory.instance.slots[woodIndex].quantity -= 2;
                    Inventory.instance.AddItem(itemData[0]);

                    PurchaseInfoAlarm.instance.PurchaseAlarm();

                    RemoveItem(woodIndex);
                    return;
                }
            }

            PurchaseInfoAlarm.instance.ShortageAlarm();
        }
    }

    // �ٳ���x1 = ��x1
    public void ShopList2()
    {
        if (shopWindow.activeInHierarchy)
        {
            int stoneIndex = -1;

            for (int i = 0; i < Inventory.instance.slots.Length; i++)
            {
                if (Inventory.instance.slots[i].item != null)
                {
                    if (Inventory.instance.slots[i].item.displayName == "��" && stoneIndex == -1)
                        stoneIndex = i;
                }
            }

            if (stoneIndex != -1) // ���� �������� ���������� �ִٴ� �Ҹ�
            {
                if (Inventory.instance.slots[stoneIndex].quantity >= 1)
                {
                    Inventory.instance.slots[stoneIndex].quantity--;
                    Inventory.instance.AddItem(itemData[1]);

                    PurchaseInfoAlarm.instance.PurchaseAlarm();

                    RemoveItem(stoneIndex);
                    return;
                }
            }

            PurchaseInfoAlarm.instance.ShortageAlarm();
        }
    }

    // ���x1 = ����x5 + ��x3
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
                    if (Inventory.instance.slots[i].item.displayName == "��������" && woodIndex == -1)
                        woodIndex = i;
                    if (Inventory.instance.slots[i].item.displayName == "��" && stoneIndex == -1)
                        stoneIndex = i;
                }
            }

            if (woodIndex != -1 && stoneIndex != -1) // ���� �������� ���������� �ִٴ� �Ҹ�
            {
                if (Inventory.instance.slots[woodIndex].quantity >= 5 && Inventory.instance.slots[stoneIndex].quantity >= 3)
                {
                    Inventory.instance.slots[woodIndex].quantity -= 5;
                    Inventory.instance.slots[stoneIndex].quantity -= 3;
                    Inventory.instance.AddItem(itemData[2]);

                    PurchaseInfoAlarm.instance.PurchaseAlarm();

                    RemoveItem(woodIndex);
                    RemoveItem(stoneIndex);
                    return;
                }
            }

            PurchaseInfoAlarm.instance.ShortageAlarm();
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
