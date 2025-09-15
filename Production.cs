using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Production : MonoBehaviour
{
    public static Production Instance { get; private set; }

    [SerializeField]
    private int wood;
    [SerializeField]
    private int stone;
    [SerializeField]
    private int iron;
    [SerializeField]
    private int food;
    [SerializeField]
    private int dayCount;
    private int totalProduction;
    private int clickCount;


    public int Wood
    {
        get { return wood; }
        set { wood = value; }
    }

    public int Stone
    {
        get { return stone; }
        set { stone = value; }
    }

    public int Iron
    {
        get { return iron; }
        set { iron = value; }
    }
    public int Food
    {
        get { return food; }
        set { food = value; }
    }

    public int DayCount
    {
        get { return dayCount; }
        set
        {
            dayCount = value;
            ItemManager.Instance.CheckItem(6);
        }
    }

    public int TotalProduction
    {
        get { return totalProduction; }
        set
        {
            totalProduction = value;
            if (totalProduction >= 1000)
            {
                ItemManager.Instance.CheckItem(5);
            }
        }
    }
    public int ClickCount
    {
        get { return clickCount; }
        set { clickCount = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeProduction()
    {
        wood = 10000;
        stone = 10000;
        iron = 10000;
        food = 1000;
        totalProduction = 0;
        clickCount = 0;
        dayCount = 0;
        SaveProduction();
    }

    public void AddWood(int amount)
    {
        wood += amount;
        UpdateTotalProduction(amount);
    }

    public void AddStone(int amount)
    {
        stone += amount;
        UpdateTotalProduction(amount);
    }

    public void AddIron(int amount)
    {
        iron += amount;
        UpdateTotalProduction(amount);
    }

    public void AddFood(int amount)
    {
        food += amount;
        UpdateTotalProduction(amount);
    }

    private void UpdateTotalProduction(int amount)
    {
        TotalProduction += amount;
    }


    public bool ConsumeWood(int amount)
    {
        if (wood >= amount)
        {
            wood -= amount;
            return true;
        }
        return false;
    }

    public bool ConsumeStone(int amount)
    {
        if (stone >= amount)
        {
            stone -= amount;
            return true;
        }
        return false;
    }

    public bool ConsumeIron(int amount)
    {
        if (iron >= amount)
        {
            iron -= amount;
            return true;
        }
        return false;
    }

    public bool ConsumeFood(int amount)
    {
        if (food >= amount)
        {
            food -= amount;
            return true;
        }
        return false;
    }

    public void SaveProduction()
    {
        PlayerPrefs.SetInt("Wood", wood);
        PlayerPrefs.SetInt("Stone", stone);
        PlayerPrefs.SetInt("Iron", iron);
        PlayerPrefs.SetInt("Food", food);
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetInt("ClickCount", clickCount);
        PlayerPrefs.Save();
    }

    public void LoadProduction()
    {
        wood = PlayerPrefs.GetInt("Wood", 0);
        stone = PlayerPrefs.GetInt("Stone", 0);
        iron = PlayerPrefs.GetInt("Iron", 0);
        food = PlayerPrefs.GetInt("Food", 0);
        dayCount = PlayerPrefs.GetInt("DayCount", 0);
        clickCount = PlayerPrefs.GetInt("ClickCount", 0);
    }

    public string ProductionStringKR(string str)
    {
        if (str == "Wood") return "³ª¹«";
        else if (str == "Stone") return "µ¹";
        else if (str == "Iron") return "Ã¶";
        else return "À½½Ä";
    }

    public bool CheckTotalProduction(int amount)
    {
        return totalProduction >= amount;
    }

    public bool DayCheck(int day)
    {
        return dayCount >= day;
    }
}
