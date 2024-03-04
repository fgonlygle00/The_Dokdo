using System.Collections;
using TMPro;
using UnityEngine;

public class PurchaseInfoAlarm : MonoBehaviour
{
    public float fadeOutTime;
    public TextMeshProUGUI shortageText;
    public TextMeshProUGUI purchaseText;

    public static PurchaseInfoAlarm instance;
    public void Awake()
    {
        instance = this;
    }

    public void ShortageAlarm()
    {
        StartCoroutine(FadeTextToZeroAlpha(fadeOutTime, shortageText));
    }

    public void PurchaseAlarm()
    {
        StartCoroutine(FadeTextToZeroAlpha(fadeOutTime, purchaseText));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
