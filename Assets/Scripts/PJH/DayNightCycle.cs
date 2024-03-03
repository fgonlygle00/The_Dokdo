using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("SkyBox")]
    public Material skyboxMaterial;
    public Color dayColor, nightColor;

    [Header("Fog")]
    public Color dayFogColor;
    public Color nightFogColor;

    [Header("Water")]
    public Material waterMaterial;
    public Color dayWaterColor, nightWaterColor;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;    
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        float t = (Mathf.Cos(time * Mathf.PI * 2) + 1) * 0.5f; // time을 사용하여 t 값 계산
        skyboxMaterial.SetColor("_Tint", Color.Lerp(dayColor, nightColor, t)); // t 값에 따라 색상 보간
        waterMaterial.SetColor("_Color", Color.Lerp(dayWaterColor, nightWaterColor, t)); // t 값에 따라 색상 보간

        RenderSettings.fogColor = Color.Lerp(dayFogColor, nightFogColor, t); // 밤과 낮의 fog 색상도 보간
    }

    void UpdateLighting(Light lightSource, Gradient colorCradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorCradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
}
