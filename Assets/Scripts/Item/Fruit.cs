using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public ItemData Apple;
    public int HowManyPerHit = 1; 
    public int quantity;
    public float dropRate = 0.5f; // �������� ���� Ȯ�� (0~1 ������ ��)

    public void Drop(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < HowManyPerHit; i++)
        {
            if (quantity <= 0) { break; }
            if (Random.value <= dropRate)
            {
                Instantiate(Apple.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }
            quantity -= 1;
        }

        if (quantity <= 0)
            Destroy(gameObject);
    }
}