using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance { get; private set; }
    public List<Cat> catPool;
    [SerializeField]
    private CatPanel catPanel;
    private int catCount;
    private bool maxCat;
    private bool maxLevelCat;

    public int CatCount
    {
        get { return catCount; }
        set { catCount = value; }
    }
    public bool MaxLevelCat
    {
        get { return maxLevelCat; }
        set 
        { 
            maxLevelCat = value;
            ItemManager.Instance.CheckItem(3);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCats();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ActivateCat()
    {
        if (Production.Instance.ConsumeFood(10))
        {
            foreach (Cat cat in catPool)
            {
                if (!cat.gameObject.activeInHierarchy)
                {
                    catCount++;
                    cat.gameObject.SetActive(true);
                    GameManager.Instance.UpdateResourceUI();
                    if (catCount == catPool.Count)
                    {
                        catPanel.CatCreatButton(false);
                        CatFullActive();
                    }
                    return;
                }
            }
        }
        else
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("자원이 부족합니다");
        }
    }
          
    public void LoadActiveCatCheck()
    {
        foreach (Cat cat in catPool)
        {
            if (!cat.gameObject.activeSelf) return;
        }
        Debug.Log("버튼끔");
        catPanel.CatCreatButton(false);
    }

    private void CatFullActive()
    {
        maxCat = true;
        ItemManager.Instance.CheckItem(1);
    }


    public bool IsCatEveryActive()
    {
        return maxCat;
    }

    public bool IsCatFullLevel()
    {
        return maxLevelCat;
    }

    public void InitializeCats()
    {
        catCount = 0;
        maxLevelCat = false;
        maxCat = false;
        foreach (Cat cat in catPool)
        {
            cat.gameObject.SetActive(false);
            cat.gameObject.transform.position = new Vector3(0, 0, 0);
        }
        catPanel.CatCreatButton(true);
    }
}
