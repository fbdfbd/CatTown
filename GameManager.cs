using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.Jobs;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Slider timeSlider;
    public float dayDuration = 1.5f;
    public float nightDuration = 1.5f;
    private float totalDuration;
    private float currentTime;

    private int clickCount;
    public TMP_Text dayCountText;
    public TMP_Text woodText;
    public TMP_Text stoneText;
    public TMP_Text ironText;
    public TMP_Text foodText;

    private bool gameLoad;

    [SerializeField]
    private SaveLoad saveLoad;
    public int ClickCount
    { 
        get { return clickCount; }
        set 
        { 
            clickCount = value;
            ItemManager.Instance.CheckItem(4);
        }
    }

    public bool GameLoad
    {
        get { return gameLoad; }
        set { gameLoad = value; }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (saveLoad.IsSaved)
        {
            saveLoad.GameLoad();
        }
        else
        {
            Production.Instance.InitializeProduction();
        }
        UpdateResourceUI();

        totalDuration = (dayDuration + nightDuration) * 60;
        StartCoroutine(DayNightCycleRoutine());
    }

    private IEnumerator DayNightCycleRoutine()
    {
        while (true)
        {
            SoundManager.Instance.PlayDayBGM();

            float dayTime = dayDuration * 60;
            for (float t = 0; t <= dayTime; t += Time.deltaTime)
            {
                currentTime = t / totalDuration;
                timeSlider.value = currentTime;
                yield return null;
            }

            SoundManager.Instance.PlayNightBGM();
            float nightTime = nightDuration * 60;
            for (float t = 0; t <= nightTime; t += Time.deltaTime)
            {
                currentTime = (dayDuration * 60 + t) / totalDuration;
                timeSlider.value = currentTime;
                yield return null;
            }

            Production.Instance.DayCount++;
            dayCountText.text = $"DAY {Production.Instance.DayCount}";
            PlayerPrefs.SetInt("DayCount", Production.Instance.DayCount);

            currentTime = 0;
            timeSlider.value = currentTime;
        }
    }

    public void SaveGameAndExit()
    {
        saveLoad.GameSave();
        Application.Quit();

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    private void OnApplicationQuit()
    {
        Production.Instance.SaveProduction();
    }

    public void UpdateResourceUI()
    {
        woodText.text = $"나무:{Production.Instance.Wood}";
        stoneText.text = $"돌:{Production.Instance.Stone}";
        ironText.text = $"철:{Production.Instance.Iron}";
        foodText.text = $"음식:{Production.Instance.Food}";
        dayCountText.text = $"DAY {Production.Instance.DayCount}";
    }

    public bool ClickCountCheck(int cnt)
    {
        if (cnt <= clickCount) return true;
        else return false;
    }
}
