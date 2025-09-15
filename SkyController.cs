using UnityEngine;
using System.Collections;

public class SkyController : MonoBehaviour
{
    public Material skyMaterial;
    public Light directionalLight;
    public Color dayTopColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    public Color dayBottomColor = new Color(0.0f, 0.5f, 1.0f, 1.0f);
    public Color nightTopColor = new Color(0.0f, 0.0f, 0.2f, 1.0f);
    public Color nightBottomColor = new Color(0.0f, 0.0f, 0.1f, 1.0f);

    private void Start()
    {
        StartCoroutine(CycleDayNight());
    }

    private IEnumerator CycleDayNight()
    {
        while (true)
        {
            float dayDuration = GameManager.Instance.dayDuration * 60;
            float nightDuration = GameManager.Instance.nightDuration * 60;

            // ³·
            yield return StartCoroutine(ChangeSkyAndLight(nightTopColor, dayTopColor, nightBottomColor, dayBottomColor, 17000f, 4000f, 10f));
            yield return new WaitForSeconds(dayDuration);

            // ¹ã
            yield return StartCoroutine(ChangeSkyAndLight(dayTopColor, nightTopColor, dayBottomColor, nightBottomColor, 4000f, 17000f, 10f));
            yield return new WaitForSeconds(nightDuration);
        }
    }

    private IEnumerator ChangeSkyAndLight(Color fromTopColor, Color toTopColor, Color fromBottomColor, Color toBottomColor, float fromTemperature, float toTemperature, float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float timeOfDay = t / duration;
            skyMaterial.SetColor("_TopColor", Color.Lerp(fromTopColor, toTopColor, timeOfDay));
            skyMaterial.SetColor("_BottomColor", Color.Lerp(fromBottomColor, toBottomColor, timeOfDay));

            float currentTemperature = Mathf.Lerp(fromTemperature, toTemperature, timeOfDay);
            directionalLight.colorTemperature = currentTemperature;

            yield return null;
        }

        skyMaterial.SetColor("_TopColor", toTopColor);
        skyMaterial.SetColor("_BottomColor", toBottomColor);
        directionalLight.colorTemperature = toTemperature;
    }
}
