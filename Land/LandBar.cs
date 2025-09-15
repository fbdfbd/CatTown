using System.Collections;
using UnityEngine;
using TMPro;

public class LandBar : MonoBehaviour
{
    [SerializeField]
    private GameObject bar;
    private TMP_Text barText;

    [SerializeField]
    private float fillTime = 100f;

    private Coroutine currentCoroutine = null;
    private const string EMPTY_BAR = "¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á";

    private Land land;

    private void Start()
    {
        barText = bar.GetComponent<TMP_Text>();
        land = GetComponent<Land>();
        InitializeBar();
    }

    private void InitializeBar()
    {
        barText.text = EMPTY_BAR;
        barText.color = new Color32(100, 100, 100, 255);
    }

    public void StartFilling(float customFillTime = -1f)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        if (customFillTime > 0f)
        {
            fillTime = customFillTime;
        }

        currentCoroutine = StartCoroutine(FillGauge());
        bar.SetActive(true);
    }

    private IEnumerator FillGauge()
    {
        float elapsedTime = 0f;
        int totalSegments = EMPTY_BAR.Length;

        while (elapsedTime < fillTime)
        {
            float percentage = elapsedTime / fillTime;
            int filledSegments = Mathf.FloorToInt(percentage * totalSegments);

            string filledText = $"<color=#00FF00>{EMPTY_BAR.Substring(0, filledSegments)}</color>";
            string remainingText = EMPTY_BAR.Substring(filledSegments);
            barText.text = filledText + remainingText;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        barText.text = $"<color=#00FF00>{EMPTY_BAR}</color>";
        yield return new WaitForSeconds(1f);
        bar.SetActive(false);

        land.IsBuild = (land.IsBuild == LandState.Construction) ? LandState.Building : LandState.Empty;
    }
}
