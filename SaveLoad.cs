using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private string saveFilePath;
    private bool isSaved;

    public bool IsSaved
    {
        get { return isSaved; }
        set { isSaved = value; }
    }

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        CheckIfSaveFileExists();
    }

    private void CheckIfSaveFileExists()
    {
        IsSaved = File.Exists(saveFilePath);
    }

    public void GameSave()
    {
        GameSaveData saveData = new GameSaveData
        {
            catDataList = CatSaveData(),
            woodWorkDataList = WorkSaveData(WorkManager.Instance.WoodWork),
            stoneWorkDataList = WorkSaveData(WorkManager.Instance.StoneWork),
            landDataList = LandSaveData(LandManager.Instance),
            itemDataList = ItemSaveData(ItemManager.Instance)
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Production.Instance.SaveProduction();
        IsSaved = true;
        Debug.Log("저장");
    }

    public void GameLoad()
    {
        if (IsSaved)
        {
            GameManager.Instance.GameLoad = true;
            string json = File.ReadAllText(saveFilePath);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
            LoadCatSaveData(saveData.catDataList);
            LoadWorkSaveData(saveData.woodWorkDataList, WorkManager.Instance.WoodWork);
            LoadWorkSaveData(saveData.stoneWorkDataList, WorkManager.Instance.StoneWork);
            LoadLandSaveData(saveData.landDataList, LandManager.Instance);
            LoadItemSaveData(saveData.itemDataList, ItemManager.Instance);
            Production.Instance.LoadProduction();
            Debug.Log("게임데이터로드");
            GameManager.Instance.GameLoad = false;
            CatManager.Instance.LoadActiveCatCheck();
        }
        else
        {
            SoundManager.Instance.PlaySFX("error");
            UIManager.Instance.ShowError("저장 파일이 없습니다");
        }
    }

    public void GameReset()
    {
        CatManager.Instance.InitializeCats();
        Production.Instance.InitializeProduction();
        WorkManager.Instance.InitializeWorks();
        LandManager.Instance.InitializeLands();
        ItemManager.Instance.InitializeItems();
        UIManager.Instance.InitializeUI();
        GameSave();

        GameManager.Instance.UpdateResourceUI();
        Debug.Log("초기화");
    }

    private List<CatSaveData> CatSaveData()
    {
        List<Cat> cats = CatManager.Instance.catPool;
        List<CatSaveData> catSaveDataList = new List<CatSaveData>();

        foreach (Cat cat in cats)
        {
            CatSaveData saveData = new CatSaveData
            {
                catName = cat.CatName,
                catLevel = cat.CatLevel,
                catAttackDamage = cat.CatAttackDamage,
                catMoveSpeed = cat.CatMoveSpeed,
                catSp = cat.CatSp,
                catHp = cat.CatHp,
                catExp = cat.CatExp,
                catPos = cat.transform.position,
                catActive = cat.gameObject.activeSelf
            };
            catSaveDataList.Add(saveData);
        }
        return catSaveDataList;
    }
    private void LoadCatSaveData(List<CatSaveData> catDataList)
    {
        for (int i = 0; i < catDataList.Count; i++)
        {
            CatSaveData saveData = catDataList[i];
            Cat cat = CatManager.Instance.catPool[i];
            cat.transform.position = saveData.catPos;
            cat.CatName = saveData.catName;
            cat.CatLevel = saveData.catLevel;
            cat.CatAttackDamage = saveData.catAttackDamage;
            cat.CatMoveSpeed = saveData.catMoveSpeed;
            cat.CatSp = saveData.catSp;
            cat.CatHp = saveData.catHp;
            cat.CatExp = saveData.catExp;
            cat.gameObject.SetActive(saveData.catActive);
            if (saveData.catActive) CatManager.Instance.CatCount++;
        }
    }

    private List<WorkSaveData> WorkSaveData(List<Work> workList)
    {
        List<WorkSaveData> workSaveDataList = new List<WorkSaveData>();

        foreach (Work work in workList)
        {
            if (work.gameObject.activeSelf)
            {
                WorkSaveData saveData = new WorkSaveData
                {
                    position = work.transform.position,
                    isWoodWork = workList == WorkManager.Instance.WoodWork
                };
                workSaveDataList.Add(saveData);
            }
        }

        return workSaveDataList;
    }
    private void LoadWorkSaveData(List<WorkSaveData> workDataList, List<Work> workList)
    {
        foreach (WorkSaveData saveData in workDataList)
        {
            Work work = workList.Find(w => !w.gameObject.activeSelf);
            if (work != null)
            {
                work.transform.position = saveData.position;
                work.gameObject.SetActive(true);
            }
        }
    }

    private List<LandSaveData> LandSaveData(LandManager landManager)
    {
        List<LandSaveData> landSaveDataList = new List<LandSaveData>();

        foreach (var landObject in landManager.landParents.GetComponentsInChildren<Land>())
        {
            LandSaveData saveData = new LandSaveData
            {
                isBuild = landObject.IsBuild,
                buildingPrefabName = landObject.BuildingData != null ? landObject.BuildingData.BuildingPrefabName : null,
                buildingLevel = landObject.BuildingLevel,
                buildingNeedProduction = landObject.BuildingNeedProduction,
                buildingProductions = new List<int>(landObject.BuildingProductions)
            };
            landSaveDataList.Add(saveData);
        }

        return landSaveDataList;
    }
    private void LoadLandSaveData(List<LandSaveData> landDataList, LandManager landManager)
    {
        Land[] lands = landManager.landParents.GetComponentsInChildren<Land>();
        for (int i = 0; i < landDataList.Count; i++)
        {
            LandSaveData saveData = landDataList[i];
            Land land = lands[i];
            land.IsBuild = saveData.isBuild;
            
            if (saveData.isBuild != LandState.Empty)
            {
                land.BuildingData = UIManager.Instance.ReturnPrefabNameToBuildingData(saveData.buildingPrefabName);
                land.BuildingLevel = saveData.buildingLevel;
                land.StateChanged();
                land.BuildingProductions = new List<int>(saveData.buildingProductions);
                landManager.buildingStateUpdate(saveData.buildingPrefabName, true);
            }
           
        }
    }

    private List<ItemSaveData> ItemSaveData(ItemManager itemManager)
    {
        List<ItemSaveData> itemSaveDataList = new List<ItemSaveData>();

        foreach (var item in itemManager.GetItems())
        {
            ItemSaveData saveData = new ItemSaveData
            {
                itemNumber = item.ItemNumber,
                isCompleted = item.IsCompleted
            };
            itemSaveDataList.Add(saveData);
        }

        return itemSaveDataList;
    }

    private void LoadItemSaveData(List<ItemSaveData> itemDataList, ItemManager itemManager)
    {
        foreach (var saveData in itemDataList)
        {
            ItemData item = itemManager.SearchItem(saveData.itemNumber);
            if (item != null)
            {
                item.IsCompleted = saveData.isCompleted;
                if (item.IsCompleted)
                {
                    UIManager.Instance.UpdateItemList(item);
                }
            }
        }
    }
}

[System.Serializable]
public class GameSaveData
{
    public List<CatSaveData> catDataList;
    public List<WorkSaveData> woodWorkDataList;
    public List<WorkSaveData> stoneWorkDataList;
    public List<LandSaveData> landDataList;
    public List<ItemSaveData> itemDataList;
}

[System.Serializable]
public class CatSaveData
{
    public string catName;
    public int catLevel;
    public float catAttackDamage;
    public float catMoveSpeed;
    public int catSp;
    public int catHp;
    public int catExp;
    public Vector3 catPos;
    public bool catActive;
}

[System.Serializable]
public class WorkSaveData
{
    public Vector3 position;
    public bool isWoodWork;
}

[System.Serializable]
public class LandSaveData
{
    public LandState isBuild;
    public string buildingPrefabName;
    public int buildingLevel;
    public int buildingNeedProduction;
    public List<int> buildingProductions;
}

[System.Serializable]
public class ItemSaveData
{
    public int itemNumber;
    public bool isCompleted;
}